# Quick Start: Tích hợp Sui trong 5 phút

## Bước 1: Cài Sui CLI (2 phút)

```bash
# macOS
brew install sui

# Hoặc Linux/macOS
cargo install --locked --git https://github.com/MystenLabs/sui.git --branch main sui
```

## Bước 2: Tạo Wallet và Fund (1 phút)

```bash
# Tạo wallet mới
sui client new-address ed25519

# Copy address và fund từ faucet
# Vào https://discord.com/invite/sui → #testnet-faucet
# Gửi: !faucet <your-address>
```

## Bước 3: Deploy Contract (1 phút)

```bash
cd web3-backend/sui-contract
sui move build
sui client publish --gas-budget 100000000
```

Copy **Package ID** từ output (dạng `0x...`)

## Bước 4: Setup Render Env Vars (1 phút)

1. Vào Render Dashboard → Chọn service `web3-backend` → Tab **"Environment"**
2. Click "Add Environment Variable" và thêm:
   - `SUI_NETWORK` = `testnet`
   - `SUI_PACKAGE_ID` = `<package-id-từ-bước-3>`
   - `SUI_PRIVATE_KEY` = `<private-key-từ-bước-2>` (base64 encoded)

## Bước 5: Deploy và Test!

1. Render sẽ tự động restart service sau khi thêm env vars
2. Đợi 1-2 phút để deploy xong
3. Test từ Unity game
4. Check Sui Explorer: https://suivision.xyz/ hoặc https://suiscan.xyz/

✅ **Xong!** Backend giờ dùng Sui blockchain thật!

---

**Chi tiết hơn?** Xem `DEPLOY_SUI.md` hoặc `SETUP_ENV_VARS_RENDER.md`

