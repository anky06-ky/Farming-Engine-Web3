# 🚀 Hostinger Quick Start - Chạy Script Tự Động

## ✅ Đã tạo scripts tự động cho bạn!

Sau khi deploy từ Git, chỉ cần chạy:

## 📋 Bước 1: SSH vào server

```bash
ssh u210601428@uk-fast-web1349.main-hosting.eu
```

## 📋 Bước 2: Chạy setup script

```bash
cd ~/public_html/backend/web3-backend
bash setup-hostinger.sh
```

Script sẽ tự động:
- ✅ Check Node.js
- ✅ Install dependencies (`npm install`)
- ✅ Build TypeScript (`npm run build`)
- ✅ Tạo `.env` file

## 📋 Bước 3: Edit .env file (nếu cần)

```bash
nano .env
```

Add environment variables nếu cần:
```
SUI_NETWORK=testnet
SUI_PACKAGE_ID=0x...
SUI_PRIVATE_KEY=...
```

## 📋 Bước 4: Start server

### Option 1: Dùng PM2 (Khuyến nghị - chạy background)

```bash
# Install PM2
npm install -g pm2

# Start server
bash start-server.sh
```

### Option 2: Chạy trực tiếp

```bash
node server.js
```

## ✅ Test API

Sau khi start server, test:

```bash
curl https://thanhphong.fun/api/health
```

Phải trả về: `{"ok":true,"service":"mini-hackathon-backend",...}`

---

## 🔧 Nếu gặp lỗi

### Lỗi: "Node.js not found"
→ Upgrade lên Business plan hoặc dùng VPS khác

### Lỗi: "Cannot find web3-backend folder"
→ Check path: `cd ~/public_html/backend/web3-backend`

### Lỗi: "npm install failed"
→ Check internet connection hoặc disk space

---

## 📝 Tóm tắt Commands

```bash
# SSH vào server
ssh u210601428@uk-fast-web1349.main-hosting.eu

# Setup
cd ~/public_html/backend/web3-backend
bash setup-hostinger.sh

# Start server
bash start-server.sh
```

**Xong!** 🎉

