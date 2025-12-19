# Web3 Backend - Render Express Server

Backend cho Unity FarmingEngine game, chạy trên Render với Express.js server.

## 🚀 Deploy lên Render

### Bước 1: Chuẩn bị

1. **Push code lên GitHub** (nếu chưa có)
2. **Đảm bảo có file `render.yaml`** trong `web3-backend/` folder

### Bước 2: Tạo Web Service trên Render

1. **Vào Render Dashboard:**
   - https://dashboard.render.com
   - Click "New" → "Web Service"

2. **Connect GitHub repository:**
   - Chọn repository của bạn
   - Click "Connect"

3. **Cấu hình Service:**
   - **Name**: `web3-backend`
   - **Root Directory**: `web3-backend`
   - **Environment**: `Node`
   - **Build Command**: `npm install && npm run build`
   - **Start Command**: `node server.js`
   - **Plan**: Free (hoặc Paid nếu muốn)

4. **Click "Create Web Service"**

### Bước 3: Setup Environment Variables

Sau khi service được tạo, vào tab **"Environment"** và thêm:

- `NODE_ENV` = `production`
- `PORT` = `10000` (Render tự set, nhưng có thể set để chắc chắn)
- `SUI_NETWORK` = `testnet` (nếu dùng Sui)
- `SUI_PACKAGE_ID` = `0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3` (nếu dùng Sui)
- `SUI_PRIVATE_KEY` = `<your-private-key-base64>` (nếu cần mint NFT)

### Bước 4: Deploy

Render sẽ tự động deploy sau khi bạn tạo service. Đợi 2-3 phút để build và start.

**URL của bạn sẽ là:** `https://web3-backend-xxx.onrender.com`

## 📡 API Endpoints

Sau khi deploy, các endpoints sẽ có dạng:

- **Health**: `https://web3-backend-xxx.onrender.com/api/health`
- **Mint**: `POST https://web3-backend-xxx.onrender.com/api/mint`
- **NFTs**: `GET https://web3-backend-xxx.onrender.com/api/nfts?wallet=0x1234...`
- **zkLogin Init**: `POST https://web3-backend-xxx.onrender.com/api/zklogin-init`
- **zkLogin Status**: `GET https://web3-backend-xxx.onrender.com/api/zklogin-status?sessionId=...`

## 🎮 Cấu hình Unity

Trong Unity, tìm GameObject có component `Web3BackendClient` và set:

**backendBaseUrl** = `https://web3-backend-xxx.onrender.com/api`

## 🔗 Sui Blockchain Integration

✅ **Backend đã sẵn sàng tích hợp Sui!**

- Code đã được implement với Sui SDK
- Auto fallback về fake data nếu Sui chưa setup
- Xem `DEPLOY_SUI.md` để biết cách deploy contract và setup
- Quick start: Xem `QUICK_START_SUI.md` (5 phút)

### Environment Variables (cho Sui):

- `SUI_NETWORK`: `testnet` hoặc `mainnet`
- `SUI_PACKAGE_ID`: Package ID của smart contract
- `SUI_PRIVATE_KEY`: Private key để mint (base64 encoded)

## 💻 Local Development

```bash
# Install dependencies
npm install

# Build TypeScript
npm run build

# Run server
npm run dev  # hoặc npm start
```

Server sẽ chạy tại: `http://localhost:3000`

Test endpoints:
- `http://localhost:3000/api/health`
- `http://localhost:3000/api/nfts?wallet=0x123`

## 📁 Cấu trúc Project

```
web3-backend/
├── src/
│   ├── lib/
│   │   ├── suiClient.ts        # Sui client helpers
│   │   └── zkloginSessions.ts  # zkLogin session management
│   └── functions/              # TypeScript functions (compiled to dist/)
│       ├── health.ts
│       ├── mint.ts
│       ├── nfts.ts
│       ├── zklogin-init.ts
│       └── zklogin-status.ts
├── dist/                       # Compiled JavaScript (từ npm run build)
├── server.js                   # Express server entry point
├── render.yaml                 # Render deployment config
├── package.json
└── tsconfig.json
```

## ⚠️ Lưu ý

1. **Render Free Tier:**
   - Service sẽ sleep sau 15 phút không dùng
   - Lần đầu request có thể mất 30-60 giây để wake up

2. **Environment Variables:**
   - KHÔNG commit private keys vào Git
   - Chỉ set trong Render Dashboard → Environment tab

3. **Build Process:**
   - Render sẽ tự động chạy `npm install && npm run build`
   - TypeScript sẽ được compile vào `dist/` folder
   - Server.js sẽ require các module từ `dist/`

## 🐛 Troubleshooting

### Service không start

- Check Render logs để xem lỗi
- Đảm bảo `npm run build` thành công
- Check `server.js` có đúng không

### Endpoints trả về 404

- Đảm bảo URL có `/api` prefix
- Check `server.js` routes đã đúng chưa

### Sui mint trả về `0xFAKE_...`

- Check environment variables đã set chưa
- Check `SUI_PRIVATE_KEY` format đúng chưa (base64)
- Xem Render logs để debug

## 📚 Documentation

- `DEPLOY_SUI.md` - Hướng dẫn deploy Sui contract
- `QUICK_START_SUI.md` - Quick start 5 phút
- `SETUP_ENV_VARS_RENDER.md` - Setup environment variables
- `CHECK_BACKEND_STATUS.md` - Debug guide
