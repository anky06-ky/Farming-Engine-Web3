import type { Handler } from "@netlify/functions";
import { getSuiClient, getKeypair, getPackageId, isValidSuiAddress } from "../lib/suiClient";
import { TransactionBlock } from "@mysten/sui.js/transactions";

/**
 * POST /.netlify/functions/mint
 *
 * Expected JSON body (matches Web3BackendClient.MintRequest):
 * {
 *   "walletAddress": "0x...",
 *   "itemId": "legendary_hoe_01"
 * }
 *
 * Response JSON (matches Web3BackendClient.MintResponse):
 * {
 *   "objectId": "0xSuiObjectId..."
 * }
 */
export const handler: Handler = async (event) => {
  if (event.httpMethod !== "POST") {
    return {
      statusCode: 405,
      body: "Method Not Allowed",
    };
  }

  try {
    const body = event.body ? JSON.parse(event.body) : {};
    const { walletAddress, itemId } = body;

    if (!walletAddress || !itemId) {
      return {
        statusCode: 400,
        body: JSON.stringify({ error: "Missing walletAddress or itemId" }),
      };
    }

    // Validate wallet address format
    if (!isValidSuiAddress(walletAddress)) {
      return {
        statusCode: 400,
        body: JSON.stringify({ error: "Invalid wallet address format" }),
      };
    }

    // Check if Sui is configured (at least need PACKAGE_ID for query, PRIVATE_KEY optional for minting)
    const useSui = !!process.env.SUI_PACKAGE_ID;
    const canMint = useSui && !!process.env.SUI_PRIVATE_KEY;

    if (useSui && canMint) {
      // Use real Sui blockchain to mint
      try {
        const client = getSuiClient();
        const keypair = getKeypair();
        const packageId = getPackageId();

        // Create transaction
        const tx = new TransactionBlock();
        tx.moveCall({
          target: `${packageId}::nft::mint`,
          arguments: [
            tx.pure.string(itemId),
            tx.pure.address(walletAddress),
          ],
        });

        // Sign and execute
        const result = await client.signAndExecuteTransactionBlock({
          signer: keypair,
          transactionBlock: tx,
          options: {
            showEffects: true,
            showObjectChanges: true,
          },
        });

        // Extract object ID from transaction result
        // The minted NFT object will be in the objectChanges
        const nftObject = result.objectChanges?.find(
          (change) => change.type === "created" && "objectType" in change && change.objectType?.includes("nft::NFT")
        );

        const objectId = nftObject && "objectId" in nftObject 
          ? nftObject.objectId 
          : result.digest; // Fallback to transaction digest

        return {
          statusCode: 200,
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            objectId,
            transactionDigest: result.digest,
          }),
        };
      } catch (suiError: any) {
        console.error("Sui mint error:", suiError);
        return {
          statusCode: 500,
          body: JSON.stringify({ 
            error: "Sui mint failed", 
            details: suiError.message 
          }),
        };
      }
    } else {
      // Fallback to fake data (when Sui not configured or no private key)
      if (useSui && !canMint) {
        console.log("SUI_PRIVATE_KEY not set, falling back to fake mint for testing");
      } else {
        console.log("Sui not configured, using fake mint");
      }
      
      const fakeObjectId = `0xFAKE_${itemId}_${Date.now().toString(16)}`;

      return {
        statusCode: 200,
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          objectId: fakeObjectId,
        }),
      };
    }
  } catch (err: any) {
    console.error("Mint handler error:", err);
    return {
      statusCode: 500,
      body: JSON.stringify({ error: err.message ?? "Unknown error" }),
    };
  }
};


