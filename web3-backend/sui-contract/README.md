# Sui Smart Contract - Farming Engine NFT

Smart contract để mint và quản lý NFTs cho Farming Engine game.

## 📋 Prerequisites

1. **Sui CLI installed**: 
   ```bash
   brew install sui
   # hoặc
   cargo install --locked --git https://github.com/MystenLabs/sui.git --branch main sui
   ```

2. **Wallet created và funded**:
   - Đã tạo wallet: ✅
   - Address: `0x8e2c64a60b96346cd780d95e9b9600630af06c9ee198dc0b59256d1f982df532`
   - Cần fund với testnet SUI (từ faucet)

## 🚀 Deploy Contract

### Cách 1: Dùng script (Khuyến nghị)

```bash
cd web3-backend/sui-contract
chmod +x deploy.sh
./deploy.sh
```

### Cách 2: Manual

```bash
cd web3-backend/sui-contract

# 1. Check active address
sui client active-address

# 2. Check gas balance (cần có SUI)
sui client gas

# 3. Build contract
sui move build

# 4. Publish contract
sui client publish --gas-budget 100000000
```

## 📝 Sau khi deploy

1. **Copy Package ID** từ output (dạng `0x...`)

2. **Thêm vào Netlify Environment Variables**:
   - `SUI_NETWORK` = `testnet`
   - `SUI_PACKAGE_ID` = `<package-id>`
   - `SUI_PRIVATE_KEY` = `<private-key-base64>` (optional, chỉ cần nếu auto-mint)

3. **Deploy lại backend** trên Netlify

## 🔍 Verify

Sau khi deploy, verify trên Sui Explorer:
- https://suivision.xyz/ hoặc https://suiscan.xyz/
- Search bằng Package ID hoặc wallet address

## 📚 Contract Functions

- `mint(item_id, recipient)` - Mint NFT cho game item
- `item_id(nft)` - Get item ID của NFT
- `collection(nft)` - Get collection name
- `nft_type(nft)` - Get NFT type

## 🐛 Troubleshooting

### "Insufficient gas"
→ Fund wallet với SUI từ faucet

### "Build failed"
→ Check Sui CLI version: `sui --version`
→ Update nếu cần: `brew upgrade sui`

### "Address not found"
→ Set active address: `sui client switch --address <address>`

