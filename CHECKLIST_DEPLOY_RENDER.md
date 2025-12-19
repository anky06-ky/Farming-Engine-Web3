# ✅ Checklist: Deploy Backend lên Render - Từng Bước

## 🎯 Mục tiêu
Deploy backend lên Render và cập nhật Unity để dùng Render URL thay vì Netlify.

---

## 📋 BƯỚC 1: Chuẩn bị Code (5 phút)

### ✅ 1.1. Kiểm tra file `render.yaml`
- [ ] File `web3-backend/render.yaml` đã có chưa?
- [ ] Nếu chưa có, file đã được tạo sẵn trong project

### ✅ 1.2. Kiểm tra `server.js`
- [ ] File `web3-backend/server.js` đã có
- [ ] File này là Express server entry point

### ✅ 1.3. Push code lên GitHub
- [ ] Code đã commit và push lên GitHub
- [ ] Repository đã public hoặc đã connect với Render

**Lệnh:**
```bash
git add .
git commit -m "Update to use Render instead of Netlify"
git push origin main
```

---

## 📋 BƯỚC 2: Tạo Web Service trên Render (10 phút)

### ✅ 2.1. Đăng nhập Render
- [ ] Vào https://dashboard.render.com
- [ ] Đăng nhập hoặc đăng ký (free tier OK)

### ✅ 2.2. Tạo Web Service
- [ ] Click **"New"** → **"Web Service"**
- [ ] Connect GitHub repository của bạn
- [ ] Chọn branch (thường là `main`)

### ✅ 2.3. Cấu hình Service
Điền các thông tin sau:

- [ ] **Name**: `web3-backend`
- [ ] **Root Directory**: `web3-backend` ⚠️ **QUAN TRỌNG!**
- [ ] **Environment**: `Node`
- [ ] **Build Command**: `npm install && npm run build`
- [ ] **Start Command**: `node server.js`
- [ ] **Plan**: Free (hoặc Paid)

### ✅ 2.4. Deploy
- [ ] Click **"Create Web Service"**
- [ ] Đợi 2-3 phút để build và deploy
- [ ] Check logs để đảm bảo không có lỗi

**URL của bạn sẽ là:** `https://web3-backend-xxx.onrender.com`
(Copy URL này để dùng sau!)

---

## 📋 BƯỚC 3: Setup Environment Variables (5 phút)

### ✅ 3.1. Vào Environment Tab
- [ ] Trong Render Dashboard, chọn service `web3-backend`
- [ ] Click tab **"Environment"**

### ✅ 3.2. Thêm Environment Variables

Click **"Add Environment Variable"** và thêm từng biến:

**Required:**
- [ ] `NODE_ENV` = `production`
- [ ] `PORT` = `10000`

**Cho Sui Integration (nếu dùng):**
- [ ] `SUI_NETWORK` = `testnet`
- [ ] `SUI_PACKAGE_ID` = `0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3`
- [ ] `SUI_PRIVATE_KEY` = `<your-private-key-base64>` (nếu cần mint NFT)

### ✅ 3.3. Save
- [ ] Click **"Save Changes"**
- [ ] Render sẽ tự động restart service
- [ ] Đợi 1-2 phút để restart xong

---

## 📋 BƯỚC 4: Verify Deployment (5 phút)

### ✅ 4.1. Test Health Endpoint
Mở browser hoặc dùng curl:

```
https://web3-backend-xxx.onrender.com/api/health
```

- [ ] Phải trả về: `{"ok":true,...}`
- [ ] Nếu lỗi 404 → Check URL có `/api` chưa
- [ ] Nếu lỗi 500 → Check Render logs

### ✅ 4.2. Test từ Terminal (Optional)
```bash
# Test health
curl https://web3-backend-xxx.onrender.com/api/health

# Test query NFTs (thay YOUR_WALLET bằng wallet address thật)
curl "https://web3-backend-xxx.onrender.com/api/nfts?wallet=YOUR_WALLET"
```

- [ ] Health endpoint trả về OK
- [ ] NFTs endpoint trả về JSON

---

## 📋 BƯỚC 5: Update Unity (10 phút)

### ✅ 5.1. Mở Unity Project
- [ ] Mở Unity Editor
- [ ] Load project

### ✅ 5.2. Update Backend URL trong Unity

Có 2 cách:

#### Cách 1: Update trong Inspector (Khuyến nghị)

1. **Tìm GameObject có `Web3BackendClient` component:**
   - Search trong Hierarchy: `Web3BackendClient` hoặc `TheGame`
   - Hoặc tìm trong scene

2. **Select GameObject đó**

3. **Trong Inspector, tìm `Web3BackendClient` component**

4. **Update field `Backend Base Url`:**
   - Cũ: `https://web3farming.netlify.app/.netlify/functions`
   - Mới: `https://web3-backend-xxx.onrender.com/api`
   - ⚠️ Thay `xxx` bằng URL thật của bạn từ Render!

5. **Nếu có `ZkLoginBackendClient` component:**
   - Update `backendBaseUrl` tương tự

6. **Save scene** (Ctrl+S)

#### Cách 2: Update trong Code (Nếu muốn)

- [ ] Mở file `Assets/FarmingEngine/Scripts/Web3BackendClient.cs`
- [ ] Tìm dòng: `public string backendBaseUrl = "..."`
- [ ] Update thành Render URL
- [ ] Save file

**Lưu ý:** Code đã có default URL là Render rồi, nhưng nên check lại!

### ✅ 5.3. Check các Scene khác
- [ ] Check scene `Farm.unity`
- [ ] Check scene `ZkLogin.unity` (nếu có)
- [ ] Update backend URL trong tất cả scenes

### ✅ 5.4. Test trong Unity
- [ ] Play game trong Unity
- [ ] Nhấn F9 để mở Web3DebugPanel
- [ ] Click "Sync NFT Inventory"
- [ ] Check Unity Console:
  - ✅ Phải thấy: `[Web3] FetchOwnedNFTs calling: https://web3-backend-xxx.onrender.com/api/nfts...`
  - ❌ KHÔNG còn lỗi 404

---

## 📋 BƯỚC 6: Final Testing (10 phút)

### ✅ 6.1. Test Sync NFT Inventory
- [ ] Mở game trong Unity
- [ ] Set wallet address trong Web3DebugPanel
- [ ] Click "Sync NFT Inventory"
- [ ] Check Console: Phải thấy success message

### ✅ 6.2. Test Mint NFT
- [ ] Craft NFT item (ví dụ: Legendary Hoe)
- [ ] Check Console:
  - ✅ Success: `ObjectId: 0x...` (không phải `0xFAKE_...`)
  - ❌ Fail: `ObjectId: 0xFAKE_...` → Check Render env vars

### ✅ 6.3. Test zkLogin (nếu dùng)
- [ ] Mở ZkLoginPanel
- [ ] Click "Login with Google"
- [ ] Check Console: Phải thấy success

---

## 🐛 Troubleshooting

### ❌ Service không start
**Giải pháp:**
1. Check Render logs → Logs tab
2. Xem có lỗi gì không
3. Đảm bảo `npm run build` thành công

### ❌ Endpoints trả về 404
**Giải pháp:**
1. Check URL có `/api` prefix chưa
2. Check `server.js` routes đã đúng chưa
3. Check Render logs

### ❌ Sui mint trả về `0xFAKE_...`
**Giải pháp:**
1. Check Render Environment Variables:
   - `SUI_PACKAGE_ID` đã set chưa?
   - `SUI_PRIVATE_KEY` đã set chưa?
2. Check Render logs để xem lỗi
3. Xem `CHECK_BACKEND_STATUS.md` để debug chi tiết

### ❌ Unity không kết nối được
**Giải pháp:**
1. Check backend URL trong Unity Inspector
2. Check URL có đúng format không: `https://web3-backend-xxx.onrender.com/api`
3. Test endpoint từ browser trước
4. Check Unity Console logs

---

## ✅ Checklist Tổng Kết

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

---

## 🎉 Hoàn thành!

Sau khi hoàn thành tất cả các bước trên, backend của bạn sẽ:
- ✅ Chạy trên Render
- ✅ Sẵn sàng nhận requests từ Unity
- ✅ Tích hợp với Sui blockchain (nếu đã setup env vars)

**URL mẫu:** `https://web3-backend-xxx.onrender.com/api`

---

## 📚 Tài liệu tham khảo

- `DEPLOY_RENDER.md` - Hướng dẫn chi tiết deploy
- `CHECK_BACKEND_STATUS.md` - Debug guide
- `SETUP_ENV_VARS_RENDER.md` - Setup env vars chi tiết
- `README.md` - Tổng quan project

---

**Cần help?** Check logs trong Render Dashboard → Logs tab!

