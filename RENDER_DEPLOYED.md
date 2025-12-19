# ✅ Backend đã deploy lên Render!

## 🌐 URL của bạn:
```
https://web3-backend-hdsw.onrender.com
```

---

## 📋 Bước tiếp theo: Setup Environment Variables

### Vào Render Dashboard:

1. **Vào:** https://dashboard.render.com
2. **Chọn service:** `web3-backend`
3. **Vào tab:** "Environment"
4. **Click "Add Environment Variable"**

### Thêm các biến sau:

1. **NODE_ENV:**
   - Key: `NODE_ENV`
   - Value: `production`

2. **PORT:**
   - Key: `PORT`
   - Value: `10000` (Render tự động set, nhưng set để chắc chắn)

3. **SUI_NETWORK:**
   - Key: `SUI_NETWORK`
   - Value: `testnet`

4. **SUI_PACKAGE_ID:**
   - Key: `SUI_PACKAGE_ID`
   - Value: `0x5980397d5e926553837ce087fa7a6a13d4dfd054f6f764903482e8b5af990ed3`

5. **SUI_PRIVATE_KEY:** (nếu cần mint NFT)
   - Key: `SUI_PRIVATE_KEY`
   - Value: `<your-private-key-base64>`

### Sau khi thêm env vars:

- Render sẽ tự động restart service
- Đợi 1-2 phút để restart xong

---

## 🧪 Test Endpoints

### 1. Health Check:
```
https://web3-backend-hdsw.onrender.com/api/health
```

Expected: `{"ok":true,"service":"mini-hackathon-backend","message":"Backend is running on Render"}`

### 2. Query NFTs:
```
https://web3-backend-hdsw.onrender.com/api/nfts?wallet=0x8e2c64a60b96346cd780d95e9b9600630af06c9ee198dc0b59256d1f982df532
```

### 3. Mint NFT (POST):
```bash
curl -X POST https://web3-backend-hdsw.onrender.com/api/mint \
  -H "Content-Type: application/json" \
  -d '{"walletAddress":"0x8e2c64a60b96346cd780d95e9b9600630af06c9ee198dc0b59256d1f982df532","itemId":"legendary_hoe_01"}'
```

---

## 🎮 Update Unity Backend URL

### Trong Unity:

1. **Tìm GameObject có `Web3BackendClient` component**
2. **Set `backendBaseUrl`:**
   ```
   https://web3-backend-hdsw.onrender.com/api
   ```

3. **Tìm GameObject có `ZkLoginBackendClient` component** (nếu có)
4. **Set `backendBaseUrl`:**
   ```
   https://web3-backend-hdsw.onrender.com/api
   ```

### Hoặc trong code:

**Web3BackendClient.cs:**
```csharp
public string backendBaseUrl = "https://web3-backend-hdsw.onrender.com/api";
```

**ZkLoginBackendClient.cs:**
```csharp
public string backendBaseUrl = "https://web3-backend-hdsw.onrender.com/api";
```

---

## ✅ Checklist

- [x] Backend deployed lên Render
- [x] URL: `https://web3-backend-hdsw.onrender.com`
- [ ] Environment variables đã setup
- [ ] Test `/api/health` endpoint
- [ ] Unity backend URL đã update
- [ ] Test từ Unity game

---

## 🎯 Next Steps

1. **Setup environment variables** trong Render Dashboard
2. **Test endpoints** từ browser/curl
3. **Update Unity** backend URL
4. **Test game** - Craft NFT và sync inventory

---

**Backend đã sẵn sàng! Chỉ cần setup env vars và test!** 🚀

