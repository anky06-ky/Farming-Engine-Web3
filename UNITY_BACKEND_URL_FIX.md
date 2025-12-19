# 🔧 Fix Backend URL trong Unity

## ❌ Lỗi hiện tại:
```
GET https://web3-backend-hdsw.onrender.com/nfts?wallet=... failed: HTTP/1.1 404 Not Found
```

**Vấn đề:** URL thiếu `/api` → Phải là `/api/nfts`

---

## ✅ Cách sửa trong Unity

### Cách 1: Update trong Inspector (Khuyến nghị)

1. **Mở scene trong Unity**
2. **Tìm GameObject có `Web3BackendClient` component:**
   - Có thể tên là "Web3BackendClient" hoặc "TheGame"
   - Hoặc search trong Hierarchy: `Web3BackendClient`
3. **Select GameObject đó**
4. **Trong Inspector, tìm `Web3BackendClient` component**
5. **Check field `Backend Base Url`:**
   - Phải là: `https://web3-backend-hdsw.onrender.com/api`
   - **KHÔNG phải:** `https://web3-backend-hdsw.onrender.com` (thiếu `/api`)
6. **Nếu sai, sửa thành:** `https://web3-backend-hdsw.onrender.com/api`
7. **Save scene**

### Cách 2: Check tất cả scenes

Có thể có nhiều scenes, cần check tất cả:

1. **Scenes cần check:**
   - `Assets/FarmingEngine/Scenes/Farm.unity`
   - `Assets/FarmingEngine/Scenes/House.unity`
   - `Assets/FarmingEngine/Scenes/Mine.unity`
   - `Assets/FarmingEngine/Scenes/Blank.unity`
   - `Assets/FarmingEngine/Scenes/Test.unity`
   - `Assets/FarmingEngine/Scenes/ZkLogin.unity` (nếu có)

2. **Mở từng scene và check `Web3BackendClient` component**

### Cách 3: Check Prefab (nếu dùng Prefab)

1. **Tìm Prefab có `Web3BackendClient`**
2. **Mở Prefab**
3. **Update `backendBaseUrl`**
4. **Apply changes**

---

## ✅ URL đúng

### Web3BackendClient:
```
https://web3-backend-hdsw.onrender.com/api
```

### Endpoints sẽ là:
- Health: `https://web3-backend-hdsw.onrender.com/api/health`
- NFTs: `https://web3-backend-hdsw.onrender.com/api/nfts?wallet=...`
- Mint: `https://web3-backend-hdsw.onrender.com/api/mint`

---

## 🔍 Verify

Sau khi sửa:

1. **Play game trong Unity**
2. **Nhấn F9** → Web3DebugPanel
3. **Click "Sync NFT Inventory"**
4. **Check Unity Console:**
   - Phải thấy: `[Web3] FetchOwnedNFTs calling: https://web3-backend-hdsw.onrender.com/api/nfts?wallet=...`
   - **KHÔNG còn lỗi 404**

---

## ⚠️ Lưu ý

- Code đã đúng: `backendBaseUrl = "https://web3-backend-hdsw.onrender.com/api"`
- Nhưng GameObject trong scene có thể vẫn dùng giá trị cũ
- Cần update trong Inspector hoặc Prefab

---

**Sau khi sửa, test lại!** ✅

