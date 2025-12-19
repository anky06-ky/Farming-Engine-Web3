# 🔍 Kiểm tra Render Service

## ❌ Lỗi: "Cannot GET /api"

Có thể do:
1. **Service đang sleep** (Render free tier sleep sau 15 phút)
2. **Service chưa start**
3. **Build/Deploy failed**

---

## ✅ Cách kiểm tra

### 1. Check Render Dashboard

1. **Vào:** https://dashboard.render.com
2. **Chọn service:** `web3-backend`
3. **Check "Logs" tab:**
   - Xem có lỗi gì không
   - Xem service có start không
   - Phải thấy: `Server running on port 10000`

4. **Check "Events" tab:**
   - Xem deploy có thành công không
   - Phải thấy: "Deploy succeeded" (màu xanh)

### 2. Check Service Status

Trong Render Dashboard:
- **Status** phải là: "Live" (màu xanh)
- **Nếu là "Sleep"**: Click "Manual Deploy" để wake up

---

## 🧪 Test Endpoints

### Test từ Browser:

1. **Root:**
   ```
   https://web3-backend-hdsw.onrender.com/
   ```
   Phải trả về JSON với API info

2. **/api:**
   ```
   https://web3-backend-hdsw.onrender.com/api
   ```
   Phải trả về JSON với API info

3. **/api/health:**
   ```
   https://web3-backend-hdsw.onrender.com/api/health
   ```
   Phải trả về: `{"ok":true,...}`

4. **/api/nfts:**
   ```
   https://web3-backend-hdsw.onrender.com/api/nfts?wallet=0x8e2c64a60b96346cd780d95e9b9600630af06c9ee198dc0b59256d1f982df532
   ```
   Phải trả về JSON với `items` array

---

## ⚠️ Render Free Tier Sleep

**Render free tier sẽ sleep sau 15 phút không dùng.**

### Dấu hiệu:
- Lần đầu request mất **30-60 giây** để wake up
- Sau đó sẽ nhanh bình thường

### Giải pháp:
1. **Đợi 30-60 giây** cho lần đầu request
2. **Hoặc upgrade** lên paid plan để không sleep

---

## 🔧 Nếu vẫn lỗi

### Check 1: Service có start không?

Trong Render Logs, phải thấy:
```
Server running on port 10000
Health check: http://localhost:10000/api/health
```

### Check 2: Build có thành công không?

Trong Render Logs, phải thấy:
```
> tsc
✅ Build completed successfully
```

### Check 3: Environment Variables

Đảm bảo đã set:
- `PORT` = `10000`
- `NODE_ENV` = `production`

---

## 🎯 Quick Fix

1. **Vào Render Dashboard**
2. **Click "Manual Deploy"** để force restart
3. **Đợi 2-3 phút** để deploy xong
4. **Test lại endpoints**

---

**Sau khi service wake up, tất cả endpoints sẽ hoạt động!** ✅

