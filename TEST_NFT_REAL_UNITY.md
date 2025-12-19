# 🧪 Test NFT Thật trong Unity

## ✅ Prerequisites

Trước khi test, đảm bảo:

1. ✅ **Backend đã deploy lên Render:** `https://web3-backend-hdsw.onrender.com`
2. ✅ **Environment variables đã setup trong Render:**
   - `SUI_NETWORK` = `testnet`
   - `SUI_PACKAGE_ID` = `0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3`
   - `SUI_PRIVATE_KEY` = `<your-private-key-base64>` (cần để mint)
3. ✅ **Unity backend URL đã update:** `https://web3-backend-hdsw.onrender.com/api`
4. ✅ **Có Sui wallet với SUI để pay gas**

---

## 📋 Bước 1: Setup Wallet trong Unity

1. **Mở game trong Unity Editor**
2. **Nhấn F9** để mở Web3DebugPanel
3. **Set wallet address:**
   - Gõ địa chỉ Sui wallet của bạn (ví dụ: `0x8e2c64a60b96346cd780d95e9b9600630af06c9ee198dc0b59256d1f982df532`)
   - Click "Apply Wallet"
4. **Click "Sync NFT Inventory"** để sync NFTs hiện có

---

## 📋 Bước 2: Test Mint NFT (Craft Item)

### Cách 1: Craft Legendary Hoe

1. **Tìm crafting recipe cho "Legendary Hoe"** trong game
2. **Craft item đó**
3. **Check Unity Console:**
   - Phải thấy: `[Web3] NFT minted successfully! ObjectId: 0x...`
   - **KHÔNG phải:** `0xFAKE_...` (nếu thấy `0xFAKE_` nghĩa là vẫn dùng fake data)

4. **Check Sui Explorer:**
   - Copy `ObjectId` từ Console
   - Vào: https://suivision.xyz/object/{objectId}
   - Phải thấy NFT trên blockchain!

### Cách 2: Test từ Web3DebugPanel (nếu có button)

- Nếu có button "Test Mint" trong Web3DebugPanel
- Click và check Console

---

## 📋 Bước 3: Test Sync NFT Inventory

1. **Mint NFT trước** (từ bước 2 hoặc từ Sui wallet khác)
2. **Trong Unity:**
   - Nhấn F9 → Web3DebugPanel
   - Click "Sync NFT Inventory"
3. **Check Unity Console:**
   - Phải thấy: `[Web3] FetchOwnedNFTs success: X items`
   - Phải thấy: `[Web3] Item added to inventory at slot: X`
4. **Check Inventory trong game:**
   - NFT phải xuất hiện trong inventory
   - Item phải là "Legendary Hoe" (hoặc NFT item khác)

---

## 📋 Bước 4: Verify trên Sui Explorer

Sau khi mint thành công:

1. **Copy ObjectId từ Unity Console**
2. **Vào Sui Explorer:**
   - **Suivision:** https://suivision.xyz/object/{objectId}
   - **Suiscan:** https://suiscan.xyz/object/{objectId}
3. **Check thông tin NFT:**
   - Owner: Phải là wallet address của bạn
   - Type: Phải có `nft::NFT`
   - Fields: `item_id`, `collection`, `nft_type`

---

## 🔍 Debug Checklist

### Nếu mint fail:

1. **Check Unity Console logs:**
   - Tìm logs bắt đầu với `[Web3]`
   - Xem có lỗi gì không

2. **Check Render logs:**
   - Vào Render Dashboard → Logs
   - Xem có lỗi Sui không

3. **Check Environment Variables:**
   - `SUI_PACKAGE_ID` đã set chưa?
   - `SUI_PRIVATE_KEY` đã set chưa?
   - `SUI_NETWORK` = `testnet`?

4. **Check Wallet:**
   - Wallet có SUI để pay gas không?
   - Wallet address đúng format không? (64 hex chars sau 0x)

### Nếu sync fail:

1. **Check wallet address:**
   - Phải là Sui address thật (64 hex chars)
   - Không phải test address như `0xabc123`

2. **Check backend response:**
   - Test: `https://web3-backend-hdsw.onrender.com/api/nfts?wallet=YOUR_WALLET`
   - Phải trả về JSON với `items` array

3. **Check NFT exists:**
   - Đảm bảo đã mint NFT trước
   - Check Sui Explorer xem NFT có tồn tại không

---

## ✅ Success Indicators

### Mint thành công:
- ✅ Console: `[Web3] NFT minted successfully! ObjectId: 0x...` (không phải `0xFAKE_...`)
- ✅ Sui Explorer: NFT hiển thị với ObjectId đó
- ✅ Owner = wallet address của bạn

### Sync thành công:
- ✅ Console: `[Web3] FetchOwnedNFTs success: X items`
- ✅ Console: `[Web3] Item added to inventory at slot: X`
- ✅ Inventory trong game: NFT xuất hiện

---

## 🎯 Quick Test Commands

### Test từ Browser:

1. **Health:**
   ```
   https://web3-backend-hdsw.onrender.com/api/health
   ```

2. **Query NFTs:**
   ```
   https://web3-backend-hdsw.onrender.com/api/nfts?wallet=YOUR_WALLET_ADDRESS
   ```

3. **Mint (POST):**
   ```bash
   curl -X POST https://web3-backend-hdsw.onrender.com/api/mint \
     -H "Content-Type: application/json" \
     -d '{"walletAddress":"YOUR_WALLET","itemId":"legendary_hoe_01"}'
   ```

---

## 📝 Notes

1. **First request có thể chậm:**
   - Render free tier có thể sleep sau 15 phút
   - Lần đầu request sẽ mất 30-60 giây để wake up

2. **Gas fees:**
   - Cần SUI trong wallet để pay gas
   - Testnet: Lấy từ faucet (Discord #testnet-faucet)

3. **ObjectId format:**
   - Sui ObjectId: `0x` + 64 hex characters
   - Nếu thấy `0xFAKE_...` → Vẫn dùng fake data (check env vars)

---

**Sau khi test thành công, game sẽ dùng Sui blockchain thật!** 🎉

