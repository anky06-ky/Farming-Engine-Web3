# 📚 Documentation Structure

## ✅ Files được giữ lại (Quan trọng)

### Root Level
- `README.md` - Main project documentation
- `CHANGELOG.md` - Project changelog

### Backend (`web3-backend/`)
- `README.md` - Backend overview và quick start
- `HOSTINGER_SETUP.md` - **Hướng dẫn deploy lên Hostinger (đang dùng)**
- `QUICK_START_SUI.md` - Quick start cho Sui integration
- `SUI_INTEGRATION.md` - Chi tiết Sui integration
- `DEPLOY_SUI.md` - Hướng dẫn deploy Sui contract
- `ZKLOGIN_SETUP.md` - zkLogin setup guide
- `TEST_ZKLOGIN.md` - Testing guide cho zkLogin

### Unity (`Assets/FarmingEngine/Scripts/UI/`)
- `ZKLOGIN_SCENE_SETUP.md` - Hướng dẫn setup zkLogin scene
- `ZKLOGIN_TROUBLESHOOTING.md` - Troubleshooting cho zkLogin

### Sui Contract (`web3-backend/sui-contract/`)
- `README.md` - Sui contract documentation
- `DEPLOY_COMMANDS.md` - Commands để deploy contract
- `NEXT_STEPS.md` - Next steps sau khi deploy

---

## 🗑️ Files đã xóa (Cũ/Trùng lặp)

- `INTEGRATION_COMPLETE.md` - Cũ, không cần nữa
- `SETUP_COMPLETE.md` - Cũ, không cần nữa
- `SUCCESS_SUMMARY.md` - Cũ, không cần nữa
- `PROJECT_STATUS.md` - Có thể merge vào README
- `DEPLOY_GAME.md` - Không cần thiết
- `TEST_MINT_NFT.md` - Cũ, không cần nữa
- `web3-backend/DEPLOY_ZKLOGIN.md` - Trùng với ZKLOGIN_SETUP.md
- `web3-backend/URGENT_DEPLOY.md` - Tạm thời, không cần nữa
- `web3-backend/TEST.md` - Cũ, không cần nữa
- `web3-backend/HOW_TO_ADD_ENV_VARS.md` - Trùng với SETUP_ENV_VARS.md
- `web3-backend/NETLIFY_ENV_SETUP.md` - Trùng với SETUP_ENV_VARS.md
- `web3-backend/SETUP_ENV_VARS.md` - Đã merge vào HOSTINGER_SETUP.md
- `web3-backend/HOSTING_ALTERNATIVES.md` - Đã chọn Hostinger rồi
- `web3-backend/sui-contract/DEPLOYED.md` - Cũ, không cần nữa

---

## 📖 Cách sử dụng Documentation

### Cho người mới:
1. Đọc `README.md` (root) để hiểu tổng quan
2. Đọc `web3-backend/README.md` để setup backend
3. Đọc `HOSTINGER_SETUP.md` để deploy

### Cho Sui Integration:
1. Đọc `QUICK_START_SUI.md` (5 phút)
2. Đọc `DEPLOY_SUI.md` (chi tiết)
3. Đọc `SUI_INTEGRATION.md` (advanced)

### Cho zkLogin:
1. Đọc `ZKLOGIN_SETUP.md`
2. Đọc `ZKLOGIN_SCENE_SETUP.md` (Unity)
3. Đọc `TEST_ZKLOGIN.md` (testing)

---

## 🎯 Tổng kết

**Trước:** 20+ documentation files (nhiều trùng lặp)  
**Sau:** 12 files (tổ chức rõ ràng, không trùng lặp)

**Cấu trúc mới:**
- Root: Overview và changelog
- Backend: Setup, deployment, integration guides
- Unity: Scene setup và troubleshooting
- Sui Contract: Contract-specific docs

