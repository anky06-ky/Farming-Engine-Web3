using UnityEngine;

namespace FarmingEngine
{
    /// <summary>
    /// Validates wallet requirement for Web3 features.
    /// Blocks all game actions if wallet is not set OR NFT inventory has not been synced.
    /// </summary>
    public static class Web3WalletValidator
    {
        private static bool walletWarningShown = false;
        private static bool nftSynced = false; // Track if NFT inventory has been synced at least once

        /// <summary>
        /// Check if wallet is set AND NFT inventory has been synced. Returns true only if both conditions are met.
        /// </summary>
        public static bool IsWalletValid()
        {
            PlayerData pdata = PlayerData.Get();
            if (pdata == null)
                return false;

            string wallet = pdata.GetWalletAddress();
            bool hasWallet = !string.IsNullOrEmpty(wallet);

            // Block if no wallet
            if (!hasWallet)
            {
                if (!walletWarningShown)
                {
                    ShowWalletWarning("Wallet Required", 
                        "You need to connect a wallet to play this game.\n\n" +
                        "Please set your wallet address in the Web3 Debug Panel (Press F9).\n\n" +
                        "All game actions are disabled until wallet is connected.");
                    walletWarningShown = true;
                }
                return false;
            }

            // Block if wallet exists but NFT inventory not synced yet
            if (!nftSynced)
            {
                if (!walletWarningShown)
                {
                    ShowWalletWarning("Sync Required", 
                        "Wallet connected, but NFT inventory needs to be synced.\n\n" +
                        "Please click 'Sync NFT Inventory' in the Web3 Debug Panel (Press F9).\n\n" +
                        "All game actions are disabled until sync is complete.");
                    walletWarningShown = true;
                }
                return false;
            }

            // Both wallet and sync are done
            return true;
        }

        /// <summary>
        /// Mark NFT inventory as synced (call after successful sync)
        /// </summary>
        public static void MarkNFTInventorySynced()
        {
            nftSynced = true;
            walletWarningShown = false; // Reset warning so it can show again if wallet changes
            Debug.Log("[Web3] NFT inventory synced. Game actions enabled.");
        }

        /// <summary>
        /// Reset sync status (call when wallet changes)
        /// </summary>
        public static void ResetSyncStatus()
        {
            nftSynced = false;
            walletWarningShown = false;
        }

        /// <summary>
        /// Show warning to player about wallet requirement
        /// </summary>
        private static void ShowWalletWarning(string title, string message)
        {
            ReadPanel panel = ReadPanel.Get(0);
            if (panel != null)
            {
                panel.ShowPanel(title, message);
            }
            Debug.LogWarning("[Web3] " + title + ": " + message);
        }
    }
}

