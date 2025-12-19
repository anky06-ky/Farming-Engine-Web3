# Enable SSH trong Hostinger hPanel

## 📍 Cách tìm SSH Access trong Hostinger

### Cách 1: Tìm trong "Nâng cao" (Advanced)

1. **Vào hPanel:** https://hpanel.hostinger.com
2. **Chọn website:** `thanhphong.fun`
3. **Trong sidebar bên trái:**
   - Scroll xuống tìm **"Trang web" (Website)**
   - Click vào **"Nâng cao" (Advanced)** để expand
   - Tìm **"SSH Access"** hoặc **"SSH"** trong sub-menu
   - Click vào đó

### Cách 2: Dùng Search

1. **Click vào search bar** ở trên cùng (⌘ K)
2. Gõ **"SSH"** hoặc **"SSH Access"**
3. Click vào kết quả

### Cách 3: Tìm trong "Bảo mật" (Security)

1. **Trong sidebar**, tìm **"Bảo mật" (Security)**
2. Click vào
3. Tìm **"SSH Access"** trong đó

---

## 🔧 Enable SSH

Sau khi vào SSH Access page:

1. **Tìm button "Enable SSH"** hoặc toggle switch
2. **Click "Enable"** hoặc toggle ON
3. **Đợi vài phút** để Hostinger activate SSH
4. **Lưu lại thông tin:**
   - SSH Host (có thể khác `uk-fast-web1349.main-hosting.eu`)
   - SSH Port (có thể không phải 22)
   - Username (thường là `u210601428`)

---

## ⚠️ Lưu ý

### Single Plan có thể không có SSH

- **Single plan** thường KHÔNG có SSH access
- **Business plan** trở lên mới có SSH
- Nếu không thấy SSH Access, có thể cần **upgrade plan**

### Nếu không có SSH

Có thể dùng:
1. **File Manager** - Upload/edit files trực tiếp
2. **Terminal trong hPanel** (nếu có)
3. **VPS FREE** (Railway, Render) - Point subdomain đến đó

---

## ✅ Sau khi Enable SSH

Thử lại SSH với thông tin mới:

```bash
ssh u210601428@SSH_HOST -p SSH_PORT
```

Hoặc nếu port là 22:
```bash
ssh u210601428@SSH_HOST
```

---

## 🎯 Next Steps

1. Enable SSH trong hPanel
2. Lưu SSH Host và Port
3. Thử SSH lại với thông tin mới
4. Nếu thành công → Chạy setup script

Bạn có thấy "SSH Access" trong hPanel không?

