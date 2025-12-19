using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace FarmingEngine
{
    /// <summary>
    /// Client for zkLogin backend API.
    /// Handles OAuth flow and Sui wallet address generation.
    /// </summary>
    public class ZkLoginBackendClient : MonoBehaviour
    {
        [Header("Backend config")]
        [Tooltip("Base URL of your Web3 backend service. For Render: https://YOUR-SERVICE.onrender.com/api")]
        public string backendBaseUrl = "https://web3-backend-hdsw.onrender.com/api";

        private static ZkLoginBackendClient _instance;

        public static ZkLoginBackendClient Get()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ZkLoginBackendClient>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("ZkLoginBackendClient");
                    _instance = go.AddComponent<ZkLoginBackendClient>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }

        [Serializable]
        public class ZkLoginRequest
        {
            public string provider; // "google", "facebook", "twitch"
        }

        [Serializable]
        public class ZkLoginResponse
        {
            public string walletAddress;
            public string authUrl; // URL to redirect user for OAuth
            public string sessionId; // Session ID for polling
        }

        [Serializable]
        public class ZkLoginStatusResponse
        {
            public string status; // "pending", "success", "error"
            public string walletAddress;
            public string error;
        }

        /// <summary>
        /// Start zkLogin flow with the specified provider.
        /// This will:
        /// 1. Get OAuth URL from backend
        /// 2. Open browser for user to login
        /// 3. Poll backend for completion
        /// 4. Return wallet address on success
        /// </summary>
        public void StartZkLogin(string provider, Action<string> onSuccess, Action<string> onError = null)
        {
            StartCoroutine(ZkLoginFlow(provider, onSuccess, onError));
        }

        private IEnumerator ZkLoginFlow(string provider, Action<string> onSuccess, Action<string> onError)
        {
            // Step 1: Initiate zkLogin
            // Note: Netlify Functions uses file names, not nested paths
            // So zklogin-init.ts maps to /zklogin-init endpoint
            ZkLoginRequest req = new ZkLoginRequest { provider = provider };
            string initUrl = backendBaseUrl.TrimEnd('/') + "/zklogin-init";
            string json = JsonUtility.ToJson(req);

            Debug.Log("[ZkLogin] Initiating login with provider: " + provider);

            string responseJson = "";
            yield return StartCoroutine(PostJson(initUrl, json, (response) =>
            {
                responseJson = response;
            }, (error) =>
            {
                Debug.LogError("[ZkLogin] Init failed: " + error);
                onError?.Invoke("Failed to initiate login: " + error);
            }));

            if (string.IsNullOrEmpty(responseJson))
                yield break;

            ZkLoginResponse initResponse = JsonUtility.FromJson<ZkLoginResponse>(responseJson);
            if (initResponse == null || string.IsNullOrEmpty(initResponse.sessionId))
            {
                onError?.Invoke("Invalid response from backend");
                yield break;
            }

            string sessionId = initResponse.sessionId;
            string authUrl = initResponse.authUrl;

            Debug.Log("[ZkLogin] Session ID: " + sessionId);
            Debug.Log("[ZkLogin] Auth URL: " + authUrl);

            // Step 2: Open browser for OAuth (in WebGL, use Application.OpenURL)
            // In Editor, we'll simulate by polling
            if (!string.IsNullOrEmpty(authUrl))
            {
                Application.OpenURL(authUrl);
                Debug.Log("[ZkLogin] Opened browser for OAuth. Please complete login in browser.");
            }

            // Step 3: Poll for completion
            // Note: Netlify Functions uses file names, not nested paths
            // So zklogin-status.ts maps to /zklogin-status endpoint
            string statusUrl = backendBaseUrl.TrimEnd('/') + "/zklogin-status?sessionId=" + UnityWebRequest.EscapeURL(sessionId);
            int maxAttempts = 60; // Poll for up to 60 seconds
            int attempts = 0;
            bool completed = false;

            while (!completed && attempts < maxAttempts)
            {
                yield return new WaitForSeconds(1f); // Wait 1 second between polls
                attempts++;

                string statusJson = "";
                yield return StartCoroutine(GetJson(statusUrl, (response) =>
                {
                    statusJson = response;
                }, (error) =>
                {
                    Debug.LogWarning("[ZkLogin] Status check failed: " + error);
                }));

                if (!string.IsNullOrEmpty(statusJson))
                {
                    ZkLoginStatusResponse statusResponse = JsonUtility.FromJson<ZkLoginStatusResponse>(statusJson);
                    if (statusResponse != null)
                    {
                        if (statusResponse.status == "success" && !string.IsNullOrEmpty(statusResponse.walletAddress))
                        {
                            // Success!
                            Debug.Log("[ZkLogin] Login completed! Wallet: " + statusResponse.walletAddress);
                            onSuccess?.Invoke(statusResponse.walletAddress);
                            completed = true;
                        }
                        else if (statusResponse.status == "error")
                        {
                            onError?.Invoke(statusResponse.error ?? "Login failed");
                            completed = true;
                        }
                        // else status == "pending", continue polling
                    }
                }
            }

            if (!completed)
            {
                onError?.Invoke("Login timeout. Please try again.");
            }
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
                    onError?.Invoke("POST " + url + " failed: " + www.error);
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

