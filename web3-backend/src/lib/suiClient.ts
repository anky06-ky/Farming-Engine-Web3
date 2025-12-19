import { SuiClient, getFullnodeUrl } from "@mysten/sui.js/client";
import { Ed25519Keypair } from "@mysten/sui.js/keypairs/ed25519";
import { TransactionBlock } from "@mysten/sui.js/transactions";

/**
 * Get Sui client instance
 * Uses environment variable SUI_NETWORK (default: testnet)
 */
export function getSuiClient(): SuiClient {
  const network = process.env.SUI_NETWORK || "testnet";
  return new SuiClient({
    url: getFullnodeUrl(network as "testnet" | "mainnet" | "devnet"),
  });
}

/**
 * Get keypair from environment variable
 * SUI_PRIVATE_KEY should be base64 encoded
 */
export function getKeypair(): Ed25519Keypair {
  const privateKey = process.env.SUI_PRIVATE_KEY;
  if (!privateKey) {
    throw new Error("SUI_PRIVATE_KEY environment variable is not set");
  }

  try {
    // Try base64 decode first
    const keyBytes = Buffer.from(privateKey, "base64");
    return Ed25519Keypair.fromSecretKey(keyBytes);
  } catch {
    // If base64 fails, try hex
    const keyBytes = Buffer.from(privateKey, "hex");
    return Ed25519Keypair.fromSecretKey(keyBytes);
  }
}

/**
 * Get package ID from environment variable
 */
export function getPackageId(): string {
  const packageId = process.env.SUI_PACKAGE_ID;
  if (!packageId) {
    throw new Error("SUI_PACKAGE_ID environment variable is not set");
  }
  return packageId;
}

/**
 * Validate Sui address format
 */
export function isValidSuiAddress(address: string): boolean {
  if (!address || typeof address !== 'string') {
    return false;
  }
  // Sui addresses are 32 bytes (64 hex chars) with 0x prefix
  // Trim whitespace and check
  const trimmed = address.trim();
  const regex = /^0x[a-fA-F0-9]{64}$/;
  const isValid = regex.test(trimmed);
  console.log('[isValidSuiAddress]', {
    address,
    trimmed,
    length: trimmed.length,
    hexPartLength: trimmed.substring(2).length,
    isValid
  });
  return isValid;
}

