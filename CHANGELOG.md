# Changelog

## [1.0.0] - 2025-12-18

### ✅ Hoàn thành

#### Backend (Render Express Server)
- ✅ Deploy backend lên Render thành công
- ✅ Health check endpoint
- ✅ Mint NFT endpoint (`POST /mint`)
- ✅ Query owned NFTs endpoint (`GET /nfts`)
- ✅ Fake data cho demo (sẵn sàng tích hợp Sui thật)

#### Unity Integration
- ✅ `Web3BackendClient` - HTTP client cho backend
- ✅ `SyncNFTInventory` - Tự động sync NFT vào inventory
- ✅ Auto-mint NFT khi craft item đặc biệt
- ✅ NFT notification khi mint thành công/thất bại
- ✅ Web3DebugPanel để test và debug
- ✅ Fallback logic: accept cả ItemData và NFTItemData

#### NFT Items
- ✅ `LegendaryHoe` - NFTItemData example
  - Id: `legendary_hoe_01`
  - Craftable với auto-mint
  - Stats tốt hơn item thường

#### Documentation
- ✅ README.md - Tổng quan project
- ✅ SUI_INTEGRATION.md - Hướng dẫn tích hợp Sui
- ✅ DEPLOY_GAME.md - Hướng dẫn deploy game
- ✅ TEST.md - Testing guide
- ✅ CHANGELOG.md - File này

### 🔄 Đã cải thiện

- Backend: Thêm comments và structure rõ ràng
- Unity: Thêm debug logs chi tiết
- UI: Thêm notification khi mint NFT
- Code: Clean up và organize tốt hơn

### 📝 Notes

- Backend hiện dùng fake data, sẵn sàng tích hợp Sui thật
- Tất cả code đã được test và hoạt động
- Documentation đầy đủ cho developers khác

### 🚀 Next Steps (Optional)

- [x] Tích hợp Sui blockchain thật ✅
- [ ] Thêm nhiều NFT items hơn
- [ ] Deploy game lên hosting
- [ ] Thêm wallet connection UI
- [ ] Thêm NFT marketplace

---

## [1.1.0] - 2025-12-18 - Sui Integration

### ✅ Sui Blockchain Integration

#### Smart Contract
- ✅ Sui Move contract (`sui-contract/sources/nft.move`)
- ✅ Contract deployed lên Sui testnet
- ✅ Package ID: `0xbe165ea0eb818414af3a5ec2642169dc7fd87dd662e660420512569e0c48fc3e`

#### Backend Updates
- ✅ `src/lib/suiClient.ts` - Sui client helper
- ✅ `src/functions/mint.ts` - Mint với Sui SDK (có fallback)
- ✅ `src/functions/nfts.ts` - Query NFTs từ Sui (có fallback)
- ✅ Auto-detect: dùng Sui nếu có `SUI_PACKAGE_ID`, fallback nếu không

#### Documentation
- ✅ `DEPLOY_SUI.md` - Chi tiết deployment guide
- ✅ `QUICK_START_SUI.md` - Quick start 5 phút
- ✅ `SETUP_ENV_VARS_RENDER.md` - Environment variables setup
- ✅ `SETUP_COMPLETE.md` - Complete setup summary

### 📝 Notes

- Contract đã deploy thành công lên Sui testnet
- Backend code sẵn sàng dùng Sui thật
- Chỉ cần setup Render env vars là xong
- Tự động fallback về fake data nếu Sui chưa setup

