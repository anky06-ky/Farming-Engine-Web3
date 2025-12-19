# 🚀 Hướng dẫn Deploy Backend lên Render

## ✅ Prerequisites

1. ✅ Code đã push lên GitHub
2. ✅ Có tài khoản Render (free tier OK)
3. ✅ File `render.yaml` đã có trong `web3-backend/` folder

## 📋 Bước 1: Tạo Web Service trên Render

1. **Vào Render Dashboard:**
   - https://dashboard.render.com
   - Đăng nhập hoặc đăng ký (free)

2. **Click "New" → "Web Service"**

3. **Connect GitHub:**
   - Chọn repository của bạn
   - Click "Connect"
   - Chọn branch (thường là `main` hoặc `master`)

4. **Cấu hình Service:**
   - **Name**: `web3-backend`
   - **Root Directory**: `web3-backend` (quan trọng!)
   - **Environment**: `Node`
   - **Build Command**: `npm install && npm run build`
   - **Start Command**: `node server.js`
   - **Plan**: Free (hoặc Paid nếu muốn không sleep)

5. **Click "Create Web Service"**

Render sẽ bắt đầu build và deploy. Đợi 2-3 phút.

## 📋 Bước 2: Setup Environment Variables

Sau khi service được tạo, vào tab **"Environment"**:

1. **Click "Add Environment Variable"**

2. **Thêm các biến sau:**

   **Required:**
   - `NODE_ENV` = `production`
   - `PORT` = `10000` (Render tự set, nhưng set để chắc chắn)

   **Cho Sui Integration (nếu dùng):**
   - `SUI_NETWORK` = `testnet`
   - `SUI_PACKAGE_ID` = `0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3`
   - `SUI_PRIVATE_KEY` = `<your-private-key-base64>`

3. **Click "Save Changes"**

Render sẽ tự động restart service sau khi save.

## 📋 Bước 3: Verify Deployment

1. **Check URL của bạn:**
   - Render sẽ cho URL dạng: `https://web3-backend-xxx.onrender.com`
   - Copy URL này

2. **Test Health Endpoint:**
   ```
   https://web3-backend-xxx.onrender.com/api/health
   ```
   Phải trả về: `{"ok":true,...}`

3. **Test từ Unity:**
   - Set `backendBaseUrl` = `https://web3-backend-xxx.onrender.com/api`
   - Test sync NFT inventory

## 🎯 API Endpoints

Sau khi deploy, các endpoints sẽ là:

- **Health**: `GET /api/health`
- **Mint NFT**: `POST /api/mint`
- **Query NFTs**: `GET /api/nfts?wallet=0x...`
- **zkLogin Init**: `POST /api/zklogin-init`
- **zkLogin Status**: `GET /api/zklogin-status?sessionId=...`

## ⚠️ Lưu ý Render Free Tier

1. **Service Sleep:**
   - Free tier sẽ sleep sau 15 phút không dùng
   - Lần đầu request có thể mất 30-60 giây để wake up
   - Sau đó sẽ nhanh bình thường

2. **Build Time:**
   - Build có thể mất 2-5 phút
   - Check logs để xem progress

3. **Logs:**
   - Vào Render Dashboard → Logs tab để xem logs
   - Logs sẽ hiển thị real-time

## 🔧 Troubleshooting

### Service không start

- **Check logs:** Render Dashboard → Logs tab
- **Check build:** Đảm bảo `npm run build` thành công
- **Check server.js:** File phải có trong `web3-backend/` folder

### Endpoints trả về 404

- **Check URL:** Phải có `/api` prefix
- **Check routes:** Xem `server.js` có đúng routes không

### Environment variables không work

- **Check format:** Đảm bảo không có spaces
- **Check save:** Click "Save Changes" sau khi thêm
- **Restart:** Render sẽ tự restart, nhưng có thể manual restart

### Sui mint trả về `0xFAKE_...`

- **Check env vars:** Đảm bảo `SUI_PACKAGE_ID` và `SUI_PRIVATE_KEY` đã set
- **Check logs:** Xem Render logs để debug
- **Xem:** `CHECK_BACKEND_STATUS.md` để debug chi tiết

## 📝 Checklist

- [ ] Code đã push lên GitHub
- [ ] Render service đã tạo
- [ ] Root Directory = `web3-backend`
- [ ] Build Command = `npm install && npm run build`
- [ ] Start Command = `node server.js`
- [ ] Environment variables đã setup
- [ ] Service đã deploy thành công
- [ ] Health endpoint trả về OK
- [ ] Unity backend URL đã update
- [ ] Test từ Unity thành công

## 🎉 Done!

Sau khi hoàn thành, backend của bạn sẽ chạy trên Render và sẵn sàng nhận requests từ Unity game!

**URL mẫu:** `https://web3-backend-xxx.onrender.com/api`

---

**Cần help?** Xem:
- `README.md` - Tổng quan
- `CHECK_BACKEND_STATUS.md` - Debug guide
- `SETUP_ENV_VARS_RENDER.md` - Setup env vars chi tiết

