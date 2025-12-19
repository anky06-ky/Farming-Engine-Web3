# zkLogin Setup Guide

## Overview

zkLogin cho phép user đăng nhập bằng OAuth providers (Google, Facebook, Twitch) và tự động tạo Sui wallet address cho mỗi user.

## Architecture

### Flow:
1. User chọn provider (Google/Facebook/Twitch) trong Unity
2. Unity gọi `/zklogin/init` để bắt đầu flow
3. Backend trả về OAuth URL và session ID
4. Unity mở browser để user login
5. Unity poll `/zklogin/status` để check completion
6. Backend trả về Sui wallet address
7. Unity lưu wallet address vào PlayerData

## Backend Endpoints

### POST `/zklogin/init`
Initiate zkLogin flow.

**Request:**
```json
{
  "provider": "google" // or "facebook", "twitch"
}
```

**Response:**
```json
{
  "sessionId": "abc123...",
  "authUrl": "https://accounts.google.com/oauth2/..."
}
```

### GET `/zklogin/status?sessionId=...`
Check login status.

**Response (pending):**
```json
{
  "status": "pending"
}
```

**Response (success):**
```json
{
  "status": "success",
  "walletAddress": "0x..."
}
```

**Response (error):**
```json
{
  "status": "error",
  "error": "Login failed"
}
```

## Current Implementation (Demo)

Hiện tại implementation là **simplified version** cho demo:
- Tự động generate Sui address từ hash (không cần OAuth thật)
- Auto-complete sau 2 giây
- Không cần ZK proof generation

## Production Setup

Để dùng zkLogin thật, cần:

1. **OAuth Setup:**
   - Register app với Google/Facebook/Twitch
   - Get Client ID và Client Secret
   - Set redirect URI

2. **ZK Proving Service:**
   - Setup proving service để generate ZK proofs
   - Hoặc dùng public proving service

3. **Salt Server:**
   - Setup salt server để lưu salt cho mỗi user
   - Hoặc dùng deterministic salt

4. **Environment Variables (trong Render Dashboard):**
   ```
   ZKLOGIN_ENABLED=true
   ZKLOGIN_REDIRECT_URI=https://web3-backend-xxx.onrender.com/api/zklogin-callback
   GOOGLE_CLIENT_ID=...
   GOOGLE_CLIENT_SECRET=...
   FACEBOOK_CLIENT_ID=...
   FACEBOOK_CLIENT_SECRET=...
   TWITCH_CLIENT_ID=...
   TWITCH_CLIENT_SECRET=...
   ```

## Testing

1. Deploy backend lên Render (xem `README.md`)
2. Test từ Unity:
   - Open ZkLoginPanel
   - Click "Login with Google"
   - Wait 2 seconds (demo auto-completes)
   - Check wallet address được lưu vào PlayerData

## References

- [Sui zkLogin Docs](https://docs.sui.io/guides/developer/cryptography/zklogin)
- [Sui zkLogin Example](https://docs.sui.io/guides/developer/cryptography/zklogin-integration/zklogin-example)

