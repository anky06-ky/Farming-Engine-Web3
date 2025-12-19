# 🔧 Setup Environment Variables trong Render

## ❌ Vấn đề: NFT trả về `0xFAKE_...`

**Nguyên nhân:** Backend chưa có environment variables để kết nối với Sui blockchain.

---

## ✅ Giải pháp: Setup Environment Variables

### Bước 1: Vào Render Dashboard

1. **Vào:** https://dashboard.render.com
2. **Chọn service:** `web3-backend`
3. **Click tab "Environment"**

---

### Bước 2: Thêm Environment Variables

Thêm các biến sau (click "Add Environment Variable" cho mỗi biến):

#### 1. `SUI_NETWORK`
- **Key:** `SUI_NETWORK`
- **Value:** `testnet`
- **Description:** Sui network (testnet/mainnet/devnet)

#### 2. `SUI_PACKAGE_ID`
- **Key:** `SUI_PACKAGE_ID`
- **Value:** `0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3`
- **Description:** Package ID của Sui contract đã deploy

#### 3. `SUI_PRIVATE_KEY` ⚠️ QUAN TRỌNG
- **Key:** `SUI_PRIVATE_KEY`
- **Value:** `<your-private-key-base64>`
- **Description:** Private key để sign transactions (base64 encoded)

**Lưu ý:** 
- Private key phải là base64 encoded
- Nếu bạn có hex key, convert sang base64:
  ```bash
  echo "YOUR_HEX_KEY" | xxd -r -p | base64
  ```

#### 4. `NODE_ENV` (optional)
- **Key:** `NODE_ENV`
- **Value:** `production`

#### 5. `PORT` (optional - Render tự set)
- **Key:** `PORT`
- **Value:** `10000`

---

### Bước 3: Save và Deploy

1. **Click "Save Changes"**
2. **Render sẽ tự động restart service**
3. **Đợi 1-2 phút** để deploy xong

---

## 🧪 Test sau khi setup

### 1. Check Logs

Vào Render Dashboard → Logs tab, phải thấy:
```
✅ Sui client initialized
✅ Package ID: 0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3
```

**Nếu thấy warning:**
```
⚠️ Sui client not available: ...
```
→ Có nghĩa là env vars chưa được set đúng hoặc build failed.

---

### 2. Test Mint từ Unity

1. **Mở game trong Unity**
2. **Nhấn F9** → Web3DebugPanel
3. **Set wallet address**
4. **Craft NFT item**
5. **Check Unity Console:**
   - ✅ **Success:** `ObjectId: 0x...` (không phải `0xFAKE_...`)
   - ❌ **Fail:** `ObjectId: 0xFAKE_...` → Check env vars và logs

---

## 🔑 Lấy Private Key

### Nếu bạn đã có wallet:

1. **Export private key từ Sui wallet:**
   - Sui Wallet extension → Settings → Export Private Key
   - Copy private key (hex format)

2. **Convert sang base64:**
   ```bash
   echo "YOUR_HEX_KEY" | xxd -r -p | base64
   ```

3. **Paste vào Render env var:** `SUI_PRIVATE_KEY`

---

### Nếu chưa có wallet:

1. **Tạo wallet mới:**
   ```bash
   sui client new-address ed25519
   ```

2. **Export private key:**
   ```bash
   sui client active-address
   sui keytool export <address> ed25519
   ```

3. **Convert sang base64 và paste vào Render**

---

## ⚠️ Lưu ý bảo mật

- **KHÔNG commit private key vào Git**
- **KHÔNG share private key**
- **Chỉ dùng testnet key** cho development
- **Mainnet key phải được bảo vệ cẩn thận**

---

## ✅ Checklist

- [ ] `SUI_NETWORK` = `testnet`
- [ ] `SUI_PACKAGE_ID` = `0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3`
- [ ] `SUI_PRIVATE_KEY` = `<base64-encoded-key>`
- [ ] Service đã restart
- [ ] Logs không có error
- [ ] Test mint NFT từ Unity → ObjectId không phải `0xFAKE_...`

---

**Sau khi setup xong, test mint NFT từ Unity!** 🚀

