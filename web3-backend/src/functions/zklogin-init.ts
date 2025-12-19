import type { Handler } from "@netlify/functions";
import crypto from "crypto";
import { createSession, updateSession } from "../lib/zkloginSessions";

/**
 * POST /.netlify/functions/zklogin-init
 * 
 * Note: Netlify Functions doesn't support nested paths like /zklogin/init
 * So we use zklogin-init as the function name
 */

export const handler: Handler = async (event) => {
  if (event.httpMethod !== "POST") {
    return { statusCode: 405, body: "Method Not Allowed" };
  }

  try {
    const body = event.body ? JSON.parse(event.body) : {};
    const { provider } = body;

    if (!provider || !["google", "facebook", "twitch"].includes(provider)) {
      return {
        statusCode: 400,
        body: JSON.stringify({ error: "Invalid provider. Must be 'google', 'facebook', or 'twitch'" }),
      };
    }

    // Generate session ID
    const sessionId = crypto.randomBytes(16).toString("hex");

    // For demo: Generate a deterministic Sui address from provider + sessionId
    // In production, this would:
    // 1. Redirect to OAuth provider
    // 2. Get JWT token
    // 3. Generate ZK proof
    // 4. Derive Sui address from proof
    
    // Simplified: Generate address from hash
    const hash = crypto.createHash("sha256").update(provider + sessionId).digest("hex");
    const walletAddress = "0x" + hash.substring(0, 64); // Sui addresses are 66 chars (0x + 64 hex)

    // Store session
    createSession(sessionId, provider);

    // For demo: Simulate OAuth completion after 2 seconds
    // In production, this would be handled by OAuth callback
    setTimeout(() => {
      updateSession(sessionId, {
        status: "success",
        walletAddress: walletAddress,
      });
    }, 2000);

    // OAuth URL (in production, this would be the real OAuth URL)
    const authUrl = `https://accounts.google.com/o/oauth2/v2/auth?client_id=YOUR_CLIENT_ID&redirect_uri=${encodeURIComponent(
      process.env.ZKLOGIN_REDIRECT_URI || "https://web3-backend-xxx.onrender.com/api/zklogin-callback"
    )}&response_type=code&scope=openid%20email&state=${sessionId}`;

    return {
      statusCode: 200,
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        sessionId,
        authUrl: process.env.ZKLOGIN_ENABLED === "true" ? authUrl : undefined, // Only return if OAuth is configured
        // For demo, we'll auto-complete after 2 seconds
      }),
    };
  } catch (err: any) {
    console.error("zkLogin init error:", err);
    return { statusCode: 500, body: JSON.stringify({ error: err.message ?? "Unknown error" }) };
  }
};

