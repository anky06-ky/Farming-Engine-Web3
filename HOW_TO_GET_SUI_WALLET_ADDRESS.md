# 🔑 Cách Lấy Đầy Đủ Sui Wallet Address

## ✅ Sui Wallet Address phải có:
- **Độ dài:** 66 ký tự
- **Format:** `0x` + 64 hex characters
- **Ví dụ:** `0xcc47188441e52eea4f98bac2161ad0559c1297adfc70f93d75f18115d771fe9a`

---

## 🎯 Cách 1: Lấy từ Sui Wallet Extension (Dễ nhất)

### Bước 1: Mở Sui Wallet Extension

1. **Mở browser** (Chrome/Edge)
2. **Click vào Sui Wallet icon** (top-right)
3. **Hoặc:** Click vào extension icon → Sui Wallet

### Bước 2: Copy Wallet Address

1. **Click vào wallet address** (hiển thị ở top của popup)
2. **Address sẽ được copy tự động** vào clipboard
3. **Hoặc:** Click vào "Copy" button bên cạnh address

### Bước 3: Verify Address

- **Phải có:** `0x` ở đầu
- **Độ dài:** 66 ký tự (bao gồm `0x`)
- **Format:** Chỉ có hex characters (0-9, a-f, A-F)

---

## 🎯 Cách 2: Lấy từ Sui CLI

### Bước 1: Check Sui CLI đã cài chưa

```bash
sui --version
```

Nếu chưa có, cài đặt:
```bash
cargo install --locked --git https://github.com/MystenLabs/sui.git --branch devnet sui
```

Hoặc dùng binary:
```bash
curl -fsSL https://get.sui.io | sh
```

### Bước 2: List tất cả addresses

```bash
sui client addresses
```

Output sẽ có dạng:
```
0xcc47188441e52eea4f98bac2161ad0559c1297adfc70f18115d771fe9a
0x8e2c64a60b96346cd780d95e9b9600630af06c9ee198dc0b59256d1f982df532
```

### Bước 3: Copy address đầy đủ

- **Copy toàn bộ address** (66 ký tự)
- **Verify:** Phải bắt đầu với `0x` và có 64 hex characters sau đó

---

## 🎯 Cách 3: Lấy từ Sui Explorer

### Bước 1: Vào Sui Explorer

- **Testnet:** https://suiscan.xyz/testnet
- **Mainnet:** https://suiscan.xyz/

### Bước 2: Connect Wallet

1. **Click "Connect Wallet"**
2. **Chọn Sui Wallet**
3. **Approve connection**

### Bước 3: Copy Address

1. **Click vào wallet address** (top-right)
2. **Copy address** từ popup hoặc profile page

---

## 🎯 Cách 4: Tạo Wallet Mới (Nếu chưa có)

### Bước 1: Tạo address mới

```bash
sui client new-address ed25519
```

Output:
```
Created new keypair for address: 0x...
```

### Bước 2: Copy address đầy đủ

- **Copy toàn bộ address** từ output
- **Verify:** Phải có 66 ký tự

---

## ⚠️ Lưu ý Quan Trọng

### ❌ Không đúng:
- `0xcc47188441e52eea4f98bac2161ad055` (chỉ 42 ký tự - bị cắt)
- `cc47188441e52eea4f98bac2161ad0559c1297adfc70f93d75f18115d771fe9a` (thiếu `0x`)
- `0xCC47188441E52EEA4F98BAC2161AD0559C1297ADFC70F93D75F18115D771FE9A` (uppercase - OK nhưng thường dùng lowercase)

### ✅ Đúng:
- `0xcc47188441e52eea4f98bac2161ad0559c1297adfc70f93d75f18115d771fe9a` (66 ký tự, đầy đủ)

---

## 🧪 Verify Address trong Unity

### Bước 1: Mở Web3DebugPanel

1. **Nhấn F9** trong Unity
2. **Web3DebugPanel** sẽ hiện ra

### Bước 2: Paste Address

1. **Copy address đầy đủ** từ Sui Wallet/CLI
2. **Paste vào input field** "Set wallet (test):"
3. **Check "Length: 66 (expected: 66)"** phải hiển thị
4. **Nếu thấy warning màu đỏ** → Address chưa đầy đủ, copy lại

### Bước 3: Apply Wallet

1. **Click "Apply Wallet"**
2. **Check Unity Console:**
   ```
   [Web3] Wallet address set in PlayerData: 0x... (length: 66)
   ```

---

## 🔍 Troubleshooting

### Vấn đề: Address bị cắt trong input field

**Giải pháp:**
1. **Copy address đầy đủ** từ Sui Wallet (không phải từ Unity)
2. **Paste vào input field** (Ctrl+V / Cmd+V)
3. **Check length** phải là 66

### Vấn đề: Không biết address nào đang dùng

**Giải pháp:**
1. **Check Sui Wallet Extension** → Copy address từ đó
2. **Hoặc:** `sui client active-address` (nếu dùng CLI)

### Vấn đề: Address không match với private key

**Giải pháp:**
- **Address và private key phải match**
- Nếu không match → Dùng address tương ứng với private key đã set trong Render

---

## 📝 Checklist

- [ ] Address có `0x` ở đầu
- [ ] Address có đúng 66 ký tự
- [ ] Address chỉ có hex characters (0-9, a-f, A-F)
- [ ] Address match với private key trong Render
- [ ] Unity Console hiển thị "length: 66" sau khi apply

---

**Sau khi có address đầy đủ, paste vào Web3DebugPanel và test lại mint NFT!** 🚀

