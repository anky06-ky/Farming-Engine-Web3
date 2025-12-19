import type { Handler } from "@netlify/functions";
import { getSession } from "../lib/zkloginSessions";

/**
 * GET /.netlify/functions/zklogin-status?sessionId=...
 * 
 * Note: Netlify Functions doesn't support nested paths like /zklogin/status
 * So we use zklogin-status as the function name
 */

export const handler: Handler = async (event) => {
  if (event.httpMethod !== "GET") {
    return { statusCode: 405, body: "Method Not Allowed" };
  }

  try {
    const sessionId = event.queryStringParameters?.sessionId;

    if (!sessionId) {
      return {
        statusCode: 400,
        body: JSON.stringify({ error: "Missing sessionId query parameter" }),
      };
    }

    const session = getSession(sessionId);

    if (!session) {
      return {
        statusCode: 404,
        body: JSON.stringify({ error: "Session not found or expired" }),
      };
    }

    if (session.status === "success" && session.walletAddress) {
      return {
        statusCode: 200,
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          status: "success",
          walletAddress: session.walletAddress,
        }),
      };
    }

    if (session.status === "error") {
      return {
        statusCode: 200,
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          status: "error",
          error: "Login failed",
        }),
      };
    }

    // Still pending
    return {
      statusCode: 200,
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        status: "pending",
      }),
    };
  } catch (err: any) {
    console.error("zkLogin status error:", err);
    return { statusCode: 500, body: JSON.stringify({ error: err.message ?? "Unknown error" }) };
  }
};

