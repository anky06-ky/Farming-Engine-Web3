# ✅ Sui Package ID Setup

## Package ID của bạn:
```
0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3
```

---

## 🚀 Bước tiếp theo: Setup Environment Variables

### Option 1: Render (Đang dùng)

1. **Vào Render Dashboard:**
   - https://dashboard.render.com
   - Chọn service `web3-backend`

2. **Vào Environment Variables:**
   - Click tab **"Environment"**
   - Click "Add Environment Variable"

3. **Thêm các biến:**
   - `SUI_NETWORK` = `testnet`
   - `SUI_PACKAGE_ID` = `0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3`
   - `SUI_PRIVATE_KEY` = `<private-key-của-wallet>` (base64 encoded)

4. **Save Changes:**
   - Render sẽ tự động restart service sau khi save
   - Đợi 1-2 phút để deploy xong

---

### Option 2: Hostinger (Nếu dùng Express server)

1. **Tạo file `.env` trong `web3-backend/`:**
   ```bash
   cd ~/public_html/backend/web3-backend
   nano .env
   ```

2. **Thêm vào file:**
   ```
   NODE_ENV=production
   PORT=3000
   SUI_NETWORK=testnet
   SUI_PACKAGE_ID=0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3
   SUI_PRIVATE_KEY=<your-private-key-base64>
   ```

3. **Restart server:**
   ```bash
   pm2 restart web3-backend
   # hoặc
   node server.js
   ```

---

## 🔍 Verify Setup

### Test từ Unity:

1. **Mở game trong Unity**
2. **Nhấn F9** để mở Web3DebugPanel
3. **Set wallet address** = address của Sui wallet bạn dùng
4. **Craft NFT item** (ví dụ: Legendary Hoe)
5. **Check Unity Console:**
   - Phải thấy: `[Web3] NFT minted successfully! ObjectId: 0x...`
   - Không còn: `0xFAKE_...` (nếu thấy `0xFAKE_` nghĩa là vẫn dùng fake data)

### Test từ Browser/curl:

```bash
# Test health (thay YOUR_RENDER_URL bằng URL thật của bạn)
curl https://web3-backend-xxx.onrender.com/api/health

# Test query NFTs (với wallet address thật)
curl "https://web3-backend-xxx.onrender.com/api/nfts?wallet=0x8e2c64a60b96346cd780d95e9b9600630af06c9ee198dc0b59256d1f982df532"
```

---

## 📝 Checklist

- [x] Contract đã deploy
- [x] Package ID: `0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3`
- [ ] Environment variables đã setup
- [ ] Backend đã deploy lại
- [ ] Test mint từ Unity
- [ ] Check Sui Explorer

---

## 🔗 Sui Explorer

Sau khi mint NFT, check trên:
- **Suivision:** https://suivision.xyz/object/{objectId}
- **Suiscan:** https://suiscan.xyz/object/{objectId}

Hoặc search Package ID:
- https://suivision.xyz/object/0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3

---

## ⚠️ Lưu ý

1. **SUI_PRIVATE_KEY:**
   - Cần để mint NFT
   - Phải là private key của wallet có SUI để pay gas
   - Encode base64 trước khi thêm vào env vars

2. **Wallet Address:**
   - Dùng wallet address trong Unity để test
   - Phải là address của wallet bạn đã fund SUI

3. **Network:**
   - Hiện tại dùng `testnet`
   - Khi production, đổi sang `mainnet`

---

**Sau khi setup xong, game sẽ dùng Sui blockchain thật!** 🎉

