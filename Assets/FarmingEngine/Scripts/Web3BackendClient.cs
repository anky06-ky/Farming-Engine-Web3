using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace FarmingEngine
{
    /// <summary>
    /// Simple HTTP client stub to talk to an external Web3 backend (for Slush / Sui).
    /// In standalone builds, the game talks to a local/remote service, and that service
    /// actually calls Sui SDK, Slush Wallet, etc.
    /// </summary>
    public class Web3BackendClient : MonoBehaviour
    {
        [Header("Backend config")]
        [Tooltip("Base URL of your Web3 backend service. For Render: https://YOUR-SERVICE.onrender.com/api")]
        public string backendBaseUrl = "https://web3-backend-hdsw.onrender.com/api";

        private static Web3BackendClient _instance;

        public static Web3BackendClient Get()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Web3BackendClient>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("Web3BackendClient");
                    _instance = go.AddComponent<Web3BackendClient>();
                    DontDestroyOnLoad(go);
                }
            }
            
            // Force update URL if it's missing /api
            if (!string.IsNullOrEmpty(_instance.backendBaseUrl) && !_instance.backendBaseUrl.Contains("/api"))
            {
                // Fix URL if it's missing /api
                if (_instance.backendBaseUrl.EndsWith("/"))
                {
                    _instance.backendBaseUrl = _instance.backendBaseUrl.TrimEnd('/') + "/api";
                }
                else
                {
                    _instance.backendBaseUrl = _instance.backendBaseUrl + "/api";
                }
                Debug.Log("[Web3] Fixed backendBaseUrl to: " + _instance.backendBaseUrl);
            }
            
            return _instance;
        }

        [Serializable]
        public class MintRequest
        {
            public string walletAddress;
            public string itemId;
        }

        [Serializable]
        public class MintResponse
        {
            public string objectId;
        }

        [Serializable]
        public class NFTInfo
        {
            public string objectId;
            public string itemId;
            public string nftType;
            public string collection;
        }

        [Serializable]
        public class NFTInfoList
        {
            public List<NFTInfo> items;
        }

        // ---- Public API ----

        public void MintNFT(string itemId, Action<string> onSuccess, Action<string> onError = null)
        {
            string wallet = PlayerData.Get()?.GetWalletAddress() ?? "";
            if (string.IsNullOrEmpty(wallet))
            {
                onError?.Invoke("Wallet address is empty. Set PlayerData.wallet_address first.");
                return;
            }
            
            // Validate wallet address format before sending
            string trimmedWallet = wallet.Trim();
            if (trimmedWallet.Length != 66)
            {
                string error = "Invalid wallet address length: " + trimmedWallet.Length + " (expected 66). Address: " + trimmedWallet;
                Debug.LogError("[Web3] " + error);
                onError?.Invoke(error);
                return;
            }
            
            if (!trimmedWallet.StartsWith("0x") || !System.Text.RegularExpressions.Regex.IsMatch(trimmedWallet, @"^0x[a-fA-F0-9]{64}$"))
            {
                string error = "Invalid wallet address format. Must be: 0x followed by 64 hex characters. Got: " + trimmedWallet;
                Debug.LogError("[Web3] " + error);
                onError?.Invoke(error);
                return;
            }

            MintRequest req = new MintRequest { walletAddress = trimmedWallet, itemId = itemId };
            string url = backendBaseUrl.TrimEnd('/') + "/mint";
            string json = JsonUtility.ToJson(req);

            Debug.Log("[Web3] MintNFT request - URL: " + url);
            Debug.Log("[Web3] MintNFT request - Wallet: " + trimmedWallet + " (length: " + trimmedWallet.Length + ")");
            Debug.Log("[Web3] MintNFT request - ItemId: " + itemId);
            Debug.Log("[Web3] MintNFT request - JSON: " + json);

            StartCoroutine(PostJson(url, json, (responseJson) =>
            {
                MintResponse resp = JsonUtility.FromJson<MintResponse>(responseJson);
                if (resp != null && !string.IsNullOrEmpty(resp.objectId))
                    onSuccess?.Invoke(resp.objectId);
                else
                    onError?.Invoke("Mint response invalid: " + responseJson);
            }, onError));
        }

        public void FetchOwnedNFTs(Action<NFTInfoList> onSuccess, Action<string> onError = null)
        {
            string wallet = PlayerData.Get()?.GetWalletAddress() ?? "";
            if (string.IsNullOrEmpty(wallet))
            {
                onError?.Invoke("Wallet address is empty. Set PlayerData.wallet_address first.");
                return;
            }

            string url = backendBaseUrl.TrimEnd('/') + "/nfts?wallet=" + UnityWebRequest.EscapeURL(wallet);
            Debug.Log("[Web3] FetchOwnedNFTs calling: " + url);

            StartCoroutine(GetJson(url, (responseJson) =>
            {
                Debug.Log("[Web3] FetchOwnedNFTs response: " + responseJson);
                NFTInfoList list = JsonUtility.FromJson<NFTInfoList>(responseJson);
                if (list != null && list.items != null)
                {
                    Debug.Log("[Web3] FetchOwnedNFTs success: " + list.items.Count + " items");
                }
                onSuccess?.Invoke(list);
            }, (error) =>
            {
                Debug.LogError("[Web3] FetchOwnedNFTs error: " + error);
                onError?.Invoke(error);
            }));
        }

        // ---- Internal HTTP helpers ----

        private IEnumerator PostJson(string url, string json, Action<string> onSuccess, Action<string> onError)
        {
            using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");

                yield return www.SendWebRequest();

#if UNITY_2020_2_OR_NEWER
                if (www.result == UnityWebRequest.Result.Success)
#else
                if (!www.isNetworkError && !www.isHttpError)
#endif
                {
                    onSuccess?.Invoke(www.downloadHandler.text);
                }
                else
                {
                    string errorMsg = www.error ?? "Unknown error";
                    string responseText = www.downloadHandler?.text ?? "";
                    Debug.LogError("[Web3] POST error - URL: " + url);
                    Debug.LogError("[Web3] POST error - Response Code: " + www.responseCode);
                    Debug.LogError("[Web3] POST error - Response: " + responseText);
                    onError?.Invoke(www.method + " " + url + " failed: HTTP/1.1 " + www.responseCode + " " + errorMsg + (string.IsNullOrEmpty(responseText) ? "" : " - " + responseText));
                }
            }
        }

        private IEnumerator GetJson(string url, Action<string> onSuccess, Action<string> onError)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

#if UNITY_2020_2_OR_NEWER
                if (www.result == UnityWebRequest.Result.Success)
#else
                if (!www.isNetworkError && !www.isHttpError)
#endif
                {
                    onSuccess?.Invoke(www.downloadHandler.text);
                }
                else
                {
                    onError?.Invoke("GET " + url + " failed: " + www.error);
                }
            }
        }
    }
}


