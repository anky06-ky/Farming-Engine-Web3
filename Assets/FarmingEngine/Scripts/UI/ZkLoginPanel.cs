using UnityEngine;
using UnityEngine.UI;
using System;

namespace FarmingEngine
{
    /// <summary>
    /// UI Panel for zkLogin authentication.
    /// Allows users to login with Google, Facebook, or Twitch OAuth providers.
    /// Each user account will get a unique Sui wallet address.
    /// </summary>
    public class ZkLoginPanel : MonoBehaviour
    {
        [Header("UI References")]
        public GameObject panelRoot;
        public Button googleLoginButton;
        public Button facebookLoginButton;
        public Button twitchLoginButton;
        public Button closeButton;
        public Text statusText;
        public Text walletAddressText;

        [Header("Config")]
        public string backendBaseUrl = "https://web3-backend-hdsw.onrender.com/api";

        private ZkLoginBackendClient backendClient;
        private bool isLoggingIn = false;

        private void Start()
        {
            // Initialize backend client (will auto-create if not exists)
            backendClient = ZkLoginBackendClient.Get();
            if (backendClient != null)
            {
                backendClient.backendBaseUrl = backendBaseUrl;
                Debug.Log("[ZkLogin] Backend client initialized: " + backendClient.backendBaseUrl);
            }
            else
            {
                Debug.LogError("[ZkLogin] Failed to initialize backend client!");
            }

            // Setup button listeners
            if (googleLoginButton != null)
                googleLoginButton.onClick.AddListener(() => OnLoginClick("google"));
            
            if (facebookLoginButton != null)
                facebookLoginButton.onClick.AddListener(() => OnLoginClick("facebook"));
            
            if (twitchLoginButton != null)
                twitchLoginButton.onClick.AddListener(() => OnLoginClick("twitch"));

            if (closeButton != null)
                closeButton.onClick.AddListener(OnCloseClick);

            // Check if already logged in
            UpdateUI();
        }

        private void UpdateUI()
        {
            PlayerData pdata = PlayerData.Get();
            string wallet = pdata != null ? pdata.GetWalletAddress() : "";

            if (!string.IsNullOrEmpty(wallet))
            {
                // Already logged in
                if (statusText != null)
                    statusText.text = "Logged in";
                
                if (walletAddressText != null)
                    walletAddressText.text = "Wallet: " + wallet.Substring(0, Mathf.Min(20, wallet.Length)) + "...";

                // Disable login buttons
                SetButtonsEnabled(false);
            }
            else
            {
                // Not logged in
                if (statusText != null)
                    statusText.text = "Choose a login method:";
                
                if (walletAddressText != null)
                    walletAddressText.text = "Not logged in";

                // Enable login buttons
                SetButtonsEnabled(true);
            }
        }

        private void SetButtonsEnabled(bool enabled)
        {
            if (googleLoginButton != null)
                googleLoginButton.interactable = enabled && !isLoggingIn;
            
            if (facebookLoginButton != null)
                facebookLoginButton.interactable = enabled && !isLoggingIn;
            
            if (twitchLoginButton != null)
                twitchLoginButton.interactable = enabled && !isLoggingIn;
        }

        private void OnLoginClick(string provider)
        {
            if (isLoggingIn)
            {
                Debug.LogWarning("[ZkLogin] Already logging in, please wait...");
                return;
            }

            Debug.Log("[ZkLogin] Starting login with provider: " + provider);
            isLoggingIn = true;
            SetButtonsEnabled(false);

            if (statusText != null)
                statusText.text = "Logging in with " + provider + "...";

            // Start zkLogin flow
            if (backendClient != null)
            {
                backendClient.StartZkLogin(provider, 
                    (walletAddress) =>
                    {
                        // Success - save wallet to PlayerData
                        Debug.Log("[ZkLogin] Login successful! Wallet: " + walletAddress);
                        
                        PlayerData pdata = PlayerData.Get();
                        if (pdata != null)
                        {
                            pdata.SetWalletAddress(walletAddress);
                            TheGame.Get()?.Save(); // Save game data
                            
                            // Reset sync status since wallet changed
                            Web3WalletValidator.ResetSyncStatus();
                        }

                        isLoggingIn = false;
                        UpdateUI();

                        // Show success message
                        if (statusText != null)
                            statusText.text = "Login successful!";
                        
                        // Auto-sync NFT inventory after login
                        if (TheGame.Get() != null)
                        {
                            TheGame.Get().SyncNFTInventory();
                        }
                    },
                    (error) =>
                    {
                        // Error
                        Debug.LogError("[ZkLogin] Login failed: " + error);
                        isLoggingIn = false;
                        UpdateUI();

                        if (statusText != null)
                            statusText.text = "Login failed: " + error;
                    });
            }
            else
            {
                Debug.LogError("[ZkLogin] Backend client not found!");
                isLoggingIn = false;
                UpdateUI();
            }
        }

        private void OnCloseClick()
        {
            if (panelRoot != null)
                panelRoot.SetActive(false);
            else
                gameObject.SetActive(false);
        }

        public void Show()
        {
            if (panelRoot != null)
                panelRoot.SetActive(true);
            else
                gameObject.SetActive(true);
            
            UpdateUI();
        }

        public void Hide()
        {
            if (panelRoot != null)
                panelRoot.SetActive(false);
            else
                gameObject.SetActive(false);
        }
    }
}

