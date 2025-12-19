# ✅ Next Steps - Backend đã hoạt động!

## 🎯 Checklist

### 1. Setup Environment Variables trong Render ⚠️ QUAN TRỌNG

1. **Vào Render Dashboard:**
   - https://dashboard.render.com
   - Chọn service: `web3-backend`

2. **Vào tab "Environment"**

3. **Thêm các biến:**
   - `NODE_ENV` = `production`
   - `PORT` = `10000`
   - `SUI_NETWORK` = `testnet`
   - `SUI_PACKAGE_ID` = `0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3`
   - `SUI_PRIVATE_KEY` = `<your-private-key-base64>` (nếu cần mint)

4. **Render sẽ tự động restart** sau khi thêm env vars

---

### 2. Test Endpoints từ Browser

Sau khi setup env vars, test:

1. **Health:**
   ```
   https://web3-backend-hdsw.onrender.com/api/health
   ```
   Phải trả về: `{"ok":true,...}`

2. **Query NFTs:**
   ```
   https://web3-backend-hdsw.onrender.com/api/nfts?wallet=0x8e2c64a60b96346cd780d95e9b9600630af06c9ee198dc0b59256d1f982df532
   ```
   Phải trả về JSON với `items` array

---

### 3. Test Mint NFT từ Unity

1. **Mở game trong Unity**
2. **Nhấn F9** → Web3DebugPanel
3. **Set wallet address:**
   - Gõ: `0x8e2c64a60b96346cd780d95e9b9600630af06c9ee198dc0b59256d1f982df532`
   - Click "Apply Wallet"
4. **Click "Sync NFT Inventory"**
5. **Craft "Legendary Hoe"** (hoặc NFT item khác)
6. **Check Unity Console:**
   - Phải thấy: `[Web3] NFT minted successfully! ObjectId: 0x...`
   - **KHÔNG phải:** `0xFAKE_...` (nếu thấy `0xFAKE_` → check env vars)

---

### 4. Verify trên Sui Explorer

1. **Copy ObjectId từ Unity Console**
2. **Vào Sui Explorer:**
   - https://suivision.xyz/object/{objectId}
   - Hoặc: https://suiscan.xyz/object/{objectId}
3. **Check:**
   - Owner = wallet address của bạn
   - Type = có `nft::NFT`
   - Fields = `item_id`, `collection`

---

### 5. Test Sync NFT Inventory

1. **Mint NFT trước** (từ bước 3)
2. **Trong Unity:**
   - Nhấn F9 → Web3DebugPanel
   - Click "Sync NFT Inventory"
3. **Check:**
   - Console: `[Web3] FetchOwnedNFTs success: X items`
   - Inventory: NFT xuất hiện trong game

---

## ✅ Success Indicators

### Backend hoạt động:
- ✅ `/api/health` trả về `{"ok":true}`
- ✅ `/api/nfts` trả về JSON
- ✅ Không còn lỗi 404

### Sui Integration hoạt động:
- ✅ Mint NFT trả về `ObjectId: 0x...` (không phải `0xFAKE_...`)
- ✅ Sui Explorer hiển thị NFT
- ✅ Sync inventory thành công

---

## 🎮 Game Flow

1. **User mở game**
2. **Nhấn F9** → Set wallet address
3. **Click "Sync NFT Inventory"** → NFTs từ blockchain sync vào game
4. **Craft NFT items** → Tự động mint lên blockchain
5. **Check Sui Explorer** → Verify NFT trên blockchain

---

## 📝 Tóm tắt

- [x] Backend deployed lên Render
- [x] Endpoints hoạt động
- [ ] **Environment variables setup** ← Làm ngay!
- [ ] Test mint NFT từ Unity
- [ ] Verify trên Sui Explorer
- [ ] Test sync inventory

---

**Bước tiếp theo: Setup environment variables trong Render!** 🚀

