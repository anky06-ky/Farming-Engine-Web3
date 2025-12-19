# Hướng dẫn Deploy Sui Smart Contract và Setup

## Bước 1: Cài đặt Sui CLI

### macOS/Linux:
```bash
cargo install --locked --git https://github.com/MystenLabs/sui.git --branch main sui
```

### Hoặc dùng Homebrew (macOS):
```bash
brew install sui
```

### Verify installation:
```bash
sui --version
```

## Bước 2: Setup Sui Wallet

1. Tạo wallet mới:
```bash
sui client new-address ed25519
```

2. Lưu lại:
- **Address**: Địa chỉ wallet (dùng để nhận NFT)
- **Private Key**: Lưu cẩn thận (cần để mint)

3. Fund wallet với testnet SUI:
- Vào https://discord.com/invite/sui
- Vào channel `#testnet-faucet`
- Gửi: `!faucet <your-address>`

## Bước 3: Deploy Smart Contract

1. Vào thư mục contract:
```bash
cd web3-backend/sui-contract
```

2. Build contract:
```bash
sui move build
```

3. Publish contract:
```bash
sui client publish --gas-budget 100000000
```

4. Copy **Package ID** từ output (dạng `0x...`)

## Bước 4: Setup Environment Variables trong Render

1. Vào Render Dashboard → Chọn service `web3-backend` → Tab **"Environment"**

2. Click "Add Environment Variable" và thêm các biến sau:

### Required:
- **SUI_NETWORK**: `testnet` (hoặc `mainnet` khi sẵn sàng)
- **SUI_PACKAGE_ID**: Package ID từ bước 3 (ví dụ: `0x1234...`)

### For Minting (Optional - chỉ cần nếu muốn auto-mint):
- **SUI_PRIVATE_KEY**: Private key của wallet (base64 hoặc hex encoded)

⚠️ **Security**: 
- KHÔNG commit private key vào code
- Chỉ dùng trong Render Environment Variables
- Consider dùng service account wallet riêng

## Bước 5: Encode Private Key

### Option 1: Base64 (Recommended)
```bash
# Từ hex string
echo "YOUR_PRIVATE_KEY_HEX" | xxd -r -p | base64

# Hoặc từ file
cat private_key.hex | xxd -r -p | base64
```

### Option 2: Hex (cũng được)
Chỉ cần copy hex string trực tiếp

## Bước 6: Test

1. Render sẽ tự động restart service sau khi thêm env vars (đợi 1-2 phút)

2. Test từ Unity:
   - Set wallet address = address từ Bước 2
   - Craft Legendary Hoe
   - Check mint có thành công không

3. Verify trên Sui Explorer:
   - Vào https://suivision.xyz/ hoặc https://suiscan.xyz/
   - Search bằng wallet address hoặc object ID
   - Xem NFT đã được mint chưa

## Bước 7: Verify trong Code

Backend sẽ tự động:
- ✅ Dùng Sui nếu `SUI_PACKAGE_ID` được set
- ✅ Fallback về fake data nếu chưa set (để dễ test)

Check logs trong Render Dashboard → Logs tab để xem:
- "Sui not configured, using fake mint" → Chưa setup
- "Sui mint error" → Có lỗi, check details
- Transaction digest → Thành công!

## Troubleshooting

### "SUI_PACKAGE_ID not set"
→ Thêm env var trong Render Dashboard → Environment tab

### "Invalid wallet address format"
→ Sui addresses phải là 64 hex chars với `0x` prefix

### "Sui mint failed"
→ Check:
- Private key đúng chưa?
- Wallet có đủ SUI để pay gas?
- Package ID đúng chưa?
- Network (testnet/mainnet) đúng chưa?

### Contract không build được
→ Check:
- Sui CLI version mới nhất
- Move.toml config đúng
- Dependencies đúng

## Next Steps

Sau khi setup xong:
1. Test mint từ Unity game
2. Check Sui Explorer để verify
3. Có thể thêm nhiều NFT items
4. Consider upgrade lên mainnet khi sẵn sàng

## Resources

- [Sui Documentation](https://docs.sui.io/)
- [Suivision](https://suivision.xyz/) | [Suiscan](https://suiscan.xyz/)
- [Sui Discord](https://discord.com/invite/sui) - Hỗ trợ và testnet faucet

