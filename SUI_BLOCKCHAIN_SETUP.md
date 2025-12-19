# 🚀 Tích hợp Sui Blockchain Thật vào Game

## ✅ Trạng thái hiện tại

- ✅ Backend code đã sẵn sàng (tự động detect Sui nếu có env vars)
- ✅ Smart contract Move đã có (`sui-contract/sources/nft.move`)
- ✅ Unity integration đã hoàn chỉnh
- ⏳ **Cần:** Deploy contract và setup environment variables

---

## 📋 Các bước để dùng Sui thật

### Bước 1: Deploy Smart Contract (5 phút)

```bash
# 1. Vào thư mục contract
cd web3-backend/sui-contract

# 2. Build contract
sui move build

# 3. Publish contract (cần wallet có SUI để pay gas)
sui client publish --gas-budget 100000000

# 4. Copy Package ID từ output (dạng 0x...)
```

**Lưu lại Package ID!**

---

### Bước 2: Setup Environment Variables

#### Nếu dùng Render:
1. Vào Render Dashboard → Chọn service `web3-backend` → Tab **"Environment"**
2. Click "Add Environment Variable" và thêm:
   - `SUI_NETWORK` = `testnet`
   - `SUI_PACKAGE_ID` = `<package-id-từ-bước-1>`
   - `SUI_PRIVATE_KEY` = `<private-key-của-wallet>` (base64)

#### Nếu dùng Hostinger (Express server):
1. Tạo file `.env` trong `web3-backend/`
2. Thêm:
   ```
   SUI_NETWORK=testnet
   SUI_PACKAGE_ID=0x...
   SUI_PRIVATE_KEY=...
   ```

---

### Bước 3: Test từ Unity

1. **Set wallet address trong Unity:**
   - Mở game
   - Nhấn F9 để mở Web3DebugPanel
   - Set wallet address = address của Sui wallet

2. **Test mint NFT:**
   - Craft "Legendary Hoe" (hoặc NFT item khác)
   - Check Unity Console: Phải thấy `[Web3] NFT minted successfully! ObjectId: 0x...`
   - Check Sui Explorer: https://suivision.xyz/object/0x...

3. **Test sync NFT inventory:**
   - Click "Sync NFT Inventory" trong Web3DebugPanel
   - Check inventory: NFT phải xuất hiện

---

## 🔍 Kiểm tra Sui Explorer

Sau khi mint thành công, check:
- **Suivision:** https://suivision.xyz/object/{objectId}
- **Suiscan:** https://suiscan.xyz/object/{objectId}

---

## ⚠️ Lưu ý

1. **Private Key Security:**
   - ⚠️ KHÔNG commit private key vào code
   - Chỉ dùng trong Environment Variables
   - Consider dùng service account wallet riêng

2. **Network:**
   - Dùng `testnet` để test
   - Chuyển sang `mainnet` khi production

3. **Gas Fees:**
   - Cần SUI trong wallet để pay gas
   - Testnet: Lấy từ faucet (Discord #testnet-faucet)

---

## 🎯 Quick Commands

```bash
# Check Sui CLI
sui --version

# Check wallet
sui client active-address

# Check balance
sui client gas

# Deploy contract
cd web3-backend/sui-contract
sui move build
sui client publish --gas-budget 100000000
```

---

## ✅ Checklist

- [ ] Sui CLI đã cài
- [ ] Wallet đã tạo và fund SUI
- [ ] Contract đã deploy
- [ ] Package ID đã copy
- [ ] Environment variables đã setup
- [ ] Backend đã deploy lại
- [ ] Test mint từ Unity
- [ ] Check Sui Explorer

---

## 📚 Tài liệu tham khảo

- Quick Start: `web3-backend/QUICK_START_SUI.md`
- Chi tiết: `web3-backend/DEPLOY_SUI.md`
- Integration: `web3-backend/SUI_INTEGRATION.md`

---

**Sau khi setup xong, game sẽ dùng Sui blockchain thật!** 🎉

