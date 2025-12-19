# 🔍 Check Backend Status - Tại sao vẫn trả về 0xFAKE_...

## ❌ Vấn đề: NFT vẫn trả về `0xFAKE_...`

Mặc dù wallet address đã đúng (66 ký tự), nhưng backend vẫn trả về fake data.

---

## ✅ Checklist Debug

### 1. Check Render Environment Variables

**Vào Render Dashboard:**
1. https://dashboard.render.com
2. Chọn service: `web3-backend`
3. Tab "Environment"

**Phải có các biến sau:**
- ✅ `SUI_NETWORK` = `testnet`
- ✅ `SUI_PACKAGE_ID` = `0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3`
- ✅ `SUI_PRIVATE_KEY` = `<base64-encoded-key>` ← **QUAN TRỌNG NHẤT!**

**Nếu thiếu `SUI_PRIVATE_KEY`:**
- Backend sẽ fallback về fake data
- Check guide: `web3-backend/ADD_PRIVATE_KEY_RENDER.md`

---

### 2. Check Render Logs

**Vào Render Dashboard → Logs tab:**

**Phải thấy:**
```
[Mint] Config check: {
  hasPackageId: true,
  hasPrivateKey: true,  ← Phải là true
  canMint: true         ← Phải là true
}
```

**Nếu thấy:**
```
hasPrivateKey: false
canMint: false
```
→ `SUI_PRIVATE_KEY` chưa được set hoặc không đúng format.

---

### 3. Check Request Logs

Khi mint NFT từ Unity, Render logs phải hiển thị:

```
[Mint] Received request: { body: { walletAddress: '0x...', itemId: '...' } }
[Mint] Validating wallet address: 0x...
[Mint] Address length: 66
[Mint] Address after 0x: 64 chars
[Mint] Attempting real Sui mint...
[Mint] Package ID: 0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3
[Mint] ✅ Success! ObjectId: 0x...
```

**Nếu thấy:**
```
[Mint] ⚠️ Using FAKE data. Reason: { canMint: false, ... }
```
→ Check env vars và logs để tìm nguyên nhân.

---

### 4. Test Backend Directly

**Test từ terminal/Postman:**

```bash
curl -X POST https://web3-backend-hdsw.onrender.com/api/mint \
  -H "Content-Type: application/json" \
  -d '{
    "walletAddress": "0x8e2c64a60b96346cd780d95e9b9600630af06c9ee198dc0b59256d1f982df532",
    "itemId": "test_item_01"
  }'
```

**Success response:**
```json
{
  "objectId": "0x...",
  "transactionDigest": "0x..."
}
```

**Fail response (fake):**
```json
{
  "objectId": "0xFAKE_test_item_01_..."
}
```

---

## 🔧 Các Nguyên Nhân Thường Gặp

### 1. SUI_PRIVATE_KEY chưa được set

**Giải pháp:**
- Vào Render Dashboard → Environment
- Add `SUI_PRIVATE_KEY` = `<base64-encoded-key>`
- Save và đợi deploy xong

### 2. SUI_PRIVATE_KEY sai format

**Giải pháp:**
- Phải là base64 hoặc hex (không có `0x` prefix)
- Check guide: `web3-backend/GET_PRIVATE_KEY.md`

### 3. Build failed - Sui SDK không load

**Giải pháp:**
- Check Render Logs → Build logs
- Phải thấy: `✅ Build completed successfully`
- Nếu có lỗi TypeScript → Fix và redeploy

### 4. Service đang sleep (Render free tier)

**Giải pháp:**
- Lần đầu request mất 30-60 giây để wake up
- Hoặc click "Manual Deploy" để wake up

---

## 🎯 Quick Fix Steps

1. **Check Render Environment Variables:**
   - `SUI_PRIVATE_KEY` phải được set
   - Format phải đúng (base64 hoặc hex)

2. **Check Render Logs:**
   - Xem `hasPrivateKey` và `canMint` có phải `true` không
   - Xem có error gì không

3. **Test từ terminal:**
   - Dùng curl để test backend trực tiếp
   - Xem response có phải `0xFAKE_...` không

4. **Nếu vẫn lỗi:**
   - Copy logs từ Render Dashboard
   - Gửi để debug tiếp

---

**Sau khi check các bước trên, test lại mint NFT từ Unity!** 🚀

