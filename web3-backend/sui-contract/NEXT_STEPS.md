# Bước tiếp theo - Deploy Contract

## ✅ Bạn đã có:
- Wallet đã được fund: `0x8e2c64a60b96346cd780d95e9b9600630af06c9ee198dc0b59256d1f982df532`
- Active address hiện tại: `0x0d7371b03d1d28bae7f4a6e244a4b1cad982b1d6286f4abe2f49c6864387e05e` (chưa có gas)

## 🔄 Switch sang address đã được fund:

```bash
sui client switch --address 0x8e2c64a60b96346cd780d95e9b9600630af06c9ee198dc0b59256d1f982df532
```

## ✅ Verify:

```bash
# Check active address (phải là address đã fund)
sui client active-address

# Check gas (phải có SUI)
sui client gas
```

## 🚀 Deploy Contract:

```bash
# Đảm bảo đang ở đúng thư mục
cd /Users/phongnguyen/MiniHackathon/web3-backend/sui-contract

# Build contract
sui move build

# Publish contract
sui client publish --gas-budget 100000000
```

## 📝 Sau khi deploy:

1. **Copy Package ID** từ output (dạng `0x...`)

2. **Thêm vào Netlify Environment Variables**:
   - `SUI_NETWORK` = `testnet`
   - `SUI_PACKAGE_ID` = `<package-id>`
   - `SUI_PRIVATE_KEY` = `<private-key>` (xem cách lấy bên dưới)

3. **Lấy Private Key** (nếu cần):
```bash
sui client export stoic-sphene
# Hoặc
sui keytool export --key-identity 0x8e2c64a60b96346cd780d95e9b9600630af06c9ee198dc0b59256d1f982df532
```

4. **Deploy lại backend** trên Netlify

## ✅ Test:

- Mint NFT từ Unity game
- Check Sui Explorer: https://suivision.xyz/ hoặc https://suiscan.xyz/

