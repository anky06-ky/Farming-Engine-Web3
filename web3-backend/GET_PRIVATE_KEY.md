# 🔑 Lấy Sui Private Key

## ✅ Tình trạng hiện tại

Từ Render logs:
- ✅ `SUI_PACKAGE_ID`: SET
- ✅ `SUI_NETWORK`: testnet
- ❌ `SUI_PRIVATE_KEY`: MISSING ← **Cần fix ngay!**

---

## 🎯 Cách 1: Lấy từ Sui Wallet Extension (Dễ nhất)

### Bước 1: Mở Sui Wallet

1. **Mở browser extension:** Sui Wallet
2. **Click vào wallet address** (top-right)
3. **Settings** → **Export Private Key**
4. **Nhập password** của wallet
5. **Copy private key** (hex format, ví dụ: `a1b2c3d4...`)

### Bước 2: Convert sang Base64

**Option A: Dùng online tool:**
- Vào: https://base64.guru/converter/encode/hex
- Paste hex key → Click "Encode" → Copy base64 result

**Option B: Dùng terminal (Mac/Linux):**
```bash
echo "YOUR_HEX_KEY" | xxd -r -p | base64
```

**Option C: Dùng Node.js:**
```bash
node -e "console.log(Buffer.from('YOUR_HEX_KEY', 'hex').toString('base64'))"
```

### Bước 3: Thêm vào Render

1. **Vào Render Dashboard:** https://dashboard.render.com
2. **Chọn service:** `web3-backend`
3. **Tab "Environment"**
4. **Add Environment Variable:**
   - **Key:** `SUI_PRIVATE_KEY`
   - **Value:** `<paste-base64-key-here>`
5. **Save Changes**

---

## 🎯 Cách 2: Tạo wallet mới bằng Sui CLI

### Bước 1: Install Sui CLI (nếu chưa có)

```bash
cargo install --locked --git https://github.com/MystenLabs/sui.git --branch devnet sui
```

Hoặc dùng binary:
```bash
curl -fsSL https://get.sui.io | sh
```

### Bước 2: Tạo wallet mới

```bash
sui client new-address ed25519
```

Output sẽ có dạng:
```
Created new keypair for address: 0x...
```

### Bước 3: Export private key

```bash
# List all addresses
sui client addresses

# Export private key (replace <address> với address vừa tạo)
sui keytool export <address> ed25519
```

Output sẽ là hex key.

### Bước 4: Convert sang Base64 và thêm vào Render

Làm giống Cách 1, Bước 2-3.

---

## 🎯 Cách 3: Dùng wallet hiện có (nếu đã có)

Nếu bạn đã có wallet và biết private key:

1. **Copy private key** (hex format)
2. **Convert sang base64** (dùng tool ở trên)
3. **Thêm vào Render** như Cách 1, Bước 3

---

## ⚠️ Lưu ý bảo mật

- **KHÔNG commit private key vào Git**
- **KHÔNG share private key** với ai
- **Chỉ dùng testnet key** cho development
- **Mainnet key phải được bảo vệ cẩn thận**

---

## ✅ Sau khi thêm SUI_PRIVATE_KEY

1. **Render sẽ tự động restart** service
2. **Đợi 1-2 phút** để deploy xong
3. **Check logs** trong Render Dashboard:
   ```
   [Mint] Config check: {
     hasPrivateKey: true,  ← Phải là true
     canMint: true         ← Phải là true
   }
   ```
4. **Test mint NFT từ Unity:**
   - Craft NFT item
   - Check Unity Console
   - Phải thấy: `ObjectId: 0x...` (KHÔNG phải `0xFAKE_...`)

---

## 🧪 Test nhanh

Sau khi thêm env var, test từ terminal:

```bash
curl -X POST https://web3-backend-hdsw.onrender.com/api/mint \
  -H "Content-Type: application/json" \
  -d '{
    "walletAddress": "0x8e2c64a60b96346cd780d95e9b9600630af06c9ee198dc0b59256d1f982df532",
    "itemId": "test_item_01"
  }'
```

**Success:** Trả về `{"objectId":"0x...","transactionDigest":"..."}`
**Fail:** Trả về `{"objectId":"0xFAKE_..."}` → Check env vars

---

**Sau khi thêm SUI_PRIVATE_KEY, test lại mint NFT từ Unity!** 🚀

