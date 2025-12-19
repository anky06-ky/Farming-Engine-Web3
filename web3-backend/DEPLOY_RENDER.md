# 🚀 Deploy Backend lên Render

## ✅ Render là gì?

- ✅ **FREE tier** tốt (750 hours/month)
- ✅ Hỗ trợ Node.js/Express
- ✅ Auto-deploy từ GitHub
- ✅ HTTPS tự động
- ✅ Environment variables dễ setup

---

## 📋 Bước 1: Tạo Account Render

1. **Vào:** https://render.com
2. **Sign up** với GitHub account (khuyến nghị)
3. **Verify email**

---

## 📋 Bước 2: Tạo Web Service

1. **Vào Dashboard:** https://dashboard.render.com
2. **Click "New +"** → **"Web Service"**
3. **Connect GitHub repository:**
   - Chọn repo: `NguyenPhong2912/Farming-Engine-Web3`
   - Click "Connect"

---

## 📋 Bước 3: Configure Service

### Basic Settings:

- **Name:** `web3-backend` (hoặc tên bạn muốn)
- **Region:** Chọn gần nhất (Singapore, US, etc.)
- **Branch:** `main`
- **Root Directory:** `web3-backend` ⚠️ **QUAN TRỌNG!**
- **Runtime:** `Node`
- **Build Command:** `npm install && npm run build`
- **Start Command:** `node server.js`

### Environment Variables:

Click "Add Environment Variable" và thêm:

1. **NODE_ENV:**
   - Key: `NODE_ENV`
   - Value: `production`

2. **PORT:**
   - Key: `PORT`
   - Value: `10000` (Render tự động set PORT, nhưng set để chắc chắn)

3. **SUI_NETWORK:**
   - Key: `SUI_NETWORK`
   - Value: `testnet`

4. **SUI_PACKAGE_ID:**
   - Key: `SUI_PACKAGE_ID`
   - Value: `0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3`

5. **SUI_PRIVATE_KEY:** (nếu cần mint)
   - Key: `SUI_PRIVATE_KEY`
   - Value: `<your-private-key-base64>`

---

## 📋 Bước 4: Deploy

1. **Click "Create Web Service"**
2. **Render sẽ tự động:**
   - Clone repo
   - Install dependencies
   - Build TypeScript
   - Start server
3. **Đợi 2-3 phút** để deploy xong
4. **Check logs** để đảm bảo không có lỗi

---

## 📋 Bước 5: Get URL

Sau khi deploy xong, Render sẽ cung cấp URL:
```
https://web3-backend-xxxx.onrender.com
```

**Lưu lại URL này!**

---

## 📋 Bước 6: Update Unity Backend URL

Trong Unity, update `backendBaseUrl`:

### Trong `Web3BackendClient.cs`:
```csharp
public string backendBaseUrl = "https://web3-backend-xxxx.onrender.com/api";
```

### Hoặc trong Inspector:
- Tìm GameObject có `Web3BackendClient` component
- Set `backendBaseUrl` = `https://web3-backend-xxxx.onrender.com/api`

---

## ✅ Test Endpoints

Sau khi deploy, test:

1. **Health check:**
   ```
   https://web3-backend-xxxx.onrender.com/api/health
   ```
   Phải trả về: `{"ok":true,"service":"mini-hackathon-backend",...}`

2. **Mint NFT:**
   ```bash
   curl -X POST https://web3-backend-xxxx.onrender.com/api/mint \
     -H "Content-Type: application/json" \
     -d '{"walletAddress":"0x...","itemId":"legendary_hoe_01"}'
   ```

3. **Query NFTs:**
   ```
   https://web3-backend-xxxx.onrender.com/api/nfts?wallet=0x...
   ```

---

## 🔧 Troubleshooting

### Issue 1: Build failed
**Fix:** Check logs trong Render Dashboard → Logs tab

### Issue 2: "Cannot find module"
**Fix:** Đảm bảo `package.json` có đầy đủ dependencies

### Issue 3: Port error
**Fix:** Render tự động set PORT, không cần hardcode

### Issue 4: Service sleeps after 15 minutes
**Fix:** 
- Free tier sẽ sleep sau 15 phút không dùng
- Lần đầu request sẽ mất 30-60 giây để wake up
- Upgrade lên paid plan để không sleep

---

## 💰 Pricing

- **FREE:** 750 hours/month, có thể sleep
- **Starter:** $7/month - Không sleep, 512MB RAM
- **Standard:** $25/month - 2GB RAM, better performance

---

## 🎯 Quick Setup Checklist

- [ ] Render account created
- [ ] GitHub repo connected
- [ ] Web Service created
- [ ] Root Directory = `web3-backend` ✅
- [ ] Build Command = `npm install && npm run build`
- [ ] Start Command = `node server.js`
- [ ] Environment variables added
- [ ] Deploy successful
- [ ] URL copied
- [ ] Unity backend URL updated
- [ ] Test endpoints working

---

## 📝 Notes

1. **Root Directory quan trọng:**
   - Phải set = `web3-backend`
   - Nếu không, Render sẽ không tìm thấy `package.json`

2. **Auto-deploy:**
   - Render tự động deploy khi push code lên GitHub
   - Có thể disable trong Settings

3. **Custom Domain:**
   - Có thể point domain của bạn đến Render
   - Vào Settings → Custom Domains

---

**Sau khi deploy xong, backend sẽ chạy trên Render và game sẽ dùng Sui blockchain thật!** 🎉

