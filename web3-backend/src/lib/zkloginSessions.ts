// Shared session store for zkLogin
// In production, use Redis or a database instead of in-memory storage

interface ZkLoginSession {
  provider: string;
  createdAt: number;
  status: "pending" | "success" | "error";
  walletAddress?: string;
  error?: string;
}

const sessions: Map<string, ZkLoginSession> = new Map();

// Clean up old sessions (older than 10 minutes)
setInterval(() => {
  const now = Date.now();
  for (const [sessionId, session] of sessions.entries()) {
    if (now - session.createdAt > 10 * 60 * 1000) {
      sessions.delete(sessionId);
    }
  }
}, 60000); // Clean up every minute

export function createSession(sessionId: string, provider: string): void {
  sessions.set(sessionId, {
    provider,
    createdAt: Date.now(),
    status: "pending",
  });
}

export function getSession(sessionId: string): ZkLoginSession | undefined {
  return sessions.get(sessionId);
}

export function updateSession(sessionId: string, updates: Partial<ZkLoginSession>): void {
  const session = sessions.get(sessionId);
  if (session) {
    Object.assign(session, updates);
  }
}

export function deleteSession(sessionId: string): void {
  sessions.delete(sessionId);
}

