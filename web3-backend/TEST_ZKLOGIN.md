# Test zkLogin Endpoints

## ✅ Endpoints đã hoạt động!

### 1. Test zklogin-init (POST)

**Cách 1: Dùng curl**
```bash
# Thay YOUR_RENDER_URL bằng URL thật của bạn
curl -X POST https://web3-backend-xxx.onrender.com/api/zklogin-init \
  -H "Content-Type: application/json" \
  -d '{"provider":"google"}'
```

**Expected Response:**
```json
{
  "sessionId": "abc123...",
  "authUrl": null
}
```

**Cách 2: Dùng Postman/Insomnia**
- Method: `POST`
- URL: `https://web3-backend-xxx.onrender.com/api/zklogin-init` (thay YOUR_RENDER_URL)
- Headers: `Content-Type: application/json`
- Body (JSON):
  ```json
  {
    "provider": "google"
  }
  ```

### 2. Test zklogin-status (GET)

**Lưu ý:** Cần `sessionId` từ bước 1!

**Cách 1: Dùng curl**
```bash
# Thay YOUR_RENDER_URL bằng URL thật của bạn
curl "https://web3-backend-xxx.onrender.com/api/zklogin-status?sessionId=YOUR_SESSION_ID"
```

**Expected Response (pending):**
```json
{
  "status": "pending"
}
```

**Expected Response (success - sau 2 giây):**
```json
{
  "status": "success",
  "walletAddress": "0x..."
}
```

**Cách 2: Dùng Browser**
- Mở browser
- URL: `https://web3-backend-xxx.onrender.com/api/zklogin-status?sessionId=YOUR_SESSION_ID` (thay YOUR_RENDER_URL)
- Phải có `sessionId` trong URL!

### 3. Test từ Unity

1. **Mở ZkLoginPanel trong Unity**
2. **Click "Login with Google"**
3. **Check Unity Console:**
   - `[ZkLogin] Initiating login with provider: google`
   - `[ZkLogin] Session ID: ...`
   - `[ZkLogin] Login completed! Wallet: ...`

## ❌ Common Errors

### Error: "Method Not Allowed" cho zklogin-init
**Nguyên nhân:** Đang gọi GET thay vì POST
**Fix:** Dùng POST method

### Error: "Missing sessionId query parameter" cho zklogin-status
**Nguyên nhân:** Thiếu `sessionId` trong URL
**Fix:** Thêm `?sessionId=YOUR_SESSION_ID` vào URL

### Error: "Session not found or expired"
**Nguyên nhân:** 
- Session ID sai
- Session đã hết hạn (>10 phút)
- Chưa gọi `/zklogin-init` trước
**Fix:** Gọi `/zklogin-init` trước để tạo session

## 🔍 Debug

### Check logs trong Render:
1. Vào Render Dashboard
2. Chọn service `web3-backend`
3. Vào "Logs" tab
4. Xem logs để debug

### Check Unity Console:
- Tất cả logs bắt đầu với `[ZkLogin]`
- Xem có lỗi gì không

## ✅ Flow hoàn chỉnh

1. **Unity gọi POST `/zklogin-init`** với `{"provider":"google"}`
2. **Backend trả về** `{"sessionId":"...","authUrl":null}`
3. **Unity poll GET `/zklogin-status?sessionId=...`** mỗi 1 giây
4. **Sau 2 giây**, backend auto-complete và trả về `{"status":"success","walletAddress":"0x..."}`
5. **Unity lưu wallet address** vào PlayerData
6. **Auto-sync NFT inventory**

