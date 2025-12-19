# Farming Engine Web3 - Unity Game với Sui Blockchain

Farming game tích hợp Web3, cho phép người chơi sở hữu và trade NFT items trên Sui blockchain.

## 🎮 Tính năng

- ✅ **NFT Inventory Sync**: Tự động sync NFT từ blockchain vào game inventory
- ✅ **Auto Mint on Craft**: Tự động mint NFT khi craft item đặc biệt
- ✅ **Backend API**: Render Express server backend để tương tác với Sui
- ✅ **Unity Integration**: Tích hợp hoàn chỉnh với FarmingEngine

## 📁 Cấu trúc Project

```
MiniHackathon/
├── Assets/
│   └── FarmingEngine/          # Unity game engine
│       ├── Scripts/
│       │   ├── Web3BackendClient.cs    # HTTP client cho backend
│       │   ├── TheGame.cs              # NFT sync logic
│       │   └── PlayerCharacterCraft.cs # Mint NFT khi craft
│       └── Resources/
│           └── Items/
│               └── Equipment/
│                   └── LegendaryHoe.asset  # NFTItemData example
│
└── web3-backend/                # Render Express server backend
    ├── src/
    │   ├── lib/                 # Sui client helpers
    │   └── functions/           # TypeScript functions (compiled to dist/)
    ├── server.js                # Express server entry point
    ├── render.yaml              # Render deployment config
    └── package.json
```

## 🚀 Quick Start

### 1. Backend Setup

```bash
cd web3-backend
npm install
npm run build
```

### 2. Deploy Backend lên Render

1. Push code lên GitHub
2. Vào [Render Dashboard](https://dashboard.render.com) → New → Web Service
3. Connect GitHub repository
4. Set:
   - **Name**: `web3-backend`
   - **Root Directory**: `web3-backend`
   - **Environment**: `Node`
   - **Build Command**: `npm install && npm run build`
   - **Start Command**: `node server.js`
5. Deploy!

Backend sẽ có URL: `https://web3-backend-xxx.onrender.com`

### 3. Unity Setup

1. Mở project trong Unity 2022.3.38f1
2. Tìm GameObject có `Web3BackendClient` component
3. Set **backendBaseUrl** = `https://web3-backend-xxx.onrender.com/api`
4. Set wallet address trong `PlayerData` (hoặc dùng Web3DebugPanel)

### 4. Test

1. Chạy game trong Unity
2. Nhấn phím toggle để mở Web3DebugPanel
3. Set wallet address: `0xabc123` (hoặc bất kỳ)
4. Click "Sync NFT Inventory"
5. Kiểm tra inventory - item "Legendary Hoe" sẽ xuất hiện!

## 🎯 Cách sử dụng

### Sync NFT vào Inventory

Game tự động sync NFT khi:
- Game start
- Manual sync từ Web3DebugPanel

### Mint NFT khi Craft

1. Tạo NFTItemData trong Unity với:
   - `craftable = true`
   - `autoMintOnCraft = true`
2. Craft item đó trong game
3. NFT sẽ tự động được mint và hiển thị notification

### Tạo NFTItemData mới

1. Trong Unity: `Assets` → `Create` → `Web3` → `NFT Item Data`
2. Set properties:
   - **Id**: `unique_item_id` (phải match với backend)
   - **Title**: Tên hiển thị
   - **autoMintOnCraft**: true nếu muốn auto-mint
3. Đặt file trong `Resources/Items/` để được load

## 🔧 API Endpoints

### Health Check
```
GET /api/health
```

### Get Owned NFTs
```
GET /api/nfts?wallet=0x...
Response: { "items": [...] }
```

### Mint NFT
```
POST /api/mint
Body: { "walletAddress": "0x...", "itemId": "legendary_hoe_01" }
Response: { "objectId": "0x..." }
```

## 🔗 Tích hợp Sui Blockchain

Xem file `web3-backend/SUI_INTEGRATION.md` để biết cách tích hợp Sui blockchain thật.

Hiện tại backend đang dùng fake data cho demo. Để dùng Sui thật:
1. Deploy smart contract lên Sui
2. Update backend code theo `SUI_INTEGRATION.md`
3. Set environment variables trong Render Dashboard

## 📝 Development

### Local Development

```bash
cd web3-backend
npm install
npm run build
npm run dev  # Chạy Express server local
```

Test endpoints:
- `http://localhost:3000/api/health`
- `http://localhost:3000/api/nfts?wallet=0x123`

### Unity Development

- Debug logs: Xem Console trong Unity
- Web3DebugPanel: Nhấn phím toggle (mặc định có thể là một phím nào đó)
- Test wallet: Dùng bất kỳ address nào (ví dụ: `0xabc123`)

## 🎨 Customization

### Thêm NFT Items

1. Tạo NFTItemData trong Unity
2. Update `web3-backend/src/functions/nfts.ts` để trả về item mới
3. Deploy backend

### Thay đổi Backend URL

Trong Unity, set `Web3BackendClient.backendBaseUrl` hoặc dùng environment variable.

## 📚 Documentation

- `web3-backend/README.md` - Backend setup
- `web3-backend/SUI_INTEGRATION.md` - Sui integration guide
- `web3-backend/TEST.md` - Testing guide

## 🐛 Troubleshooting

### NFT không xuất hiện trong inventory

- Kiểm tra item ID có match giữa backend và Unity không
- Kiểm tra file NFTItemData có trong `Resources/` không
- Xem Console logs để debug

### Backend không hoạt động

- Kiểm tra Render deploy logs
- Test endpoints bằng curl hoặc Postman
- Đảm bảo `render.yaml` config đúng
- Check environment variables trong Render Dashboard

### Mint NFT failed

- Kiểm tra wallet address đã set chưa
- Xem Console logs để biết lỗi cụ thể
- Kiểm tra backend logs trên Render Dashboard

## 📄 License

FarmingEngine asset có license riêng. Xem `Assets/FarmingEngine/Licence.txt`

## 🙏 Credits

- **FarmingEngine**: Game engine template
- **Sui**: Blockchain platform
- **Render**: Hosting platform

## 📞 Support

Nếu có vấn đề, xem:
- Unity Console logs
- Render service logs
- Backend README files

---

**Happy Farming! 🌾**

