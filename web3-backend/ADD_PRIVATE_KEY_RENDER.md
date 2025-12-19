# ✅ Thêm Private Key vào Render

## 🔑 Private Key của bạn

**Address:** `0xcc47188441e52eea4f98bac2161ad0559c1297adfc70f18115d771fe9a`

**Private Key (hex):** `cc47188441e52eea4f98bac2161ad0559c1297adfc70f93d75f18115d771fe9a`

**Private Key (base64):** `xEcYhEHlLupPmLrCFhrQVZwSlr38cPk9dfGBFddx/po=`

---

## 📝 Cách thêm vào Render

### Bước 1: Vào Render Dashboard

1. **Vào:** https://dashboard.render.com
2. **Chọn service:** `web3-backend`
3. **Click tab "Environment"**

### Bước 2: Thêm SUI_PRIVATE_KEY

1. **Click "Add Environment Variable"**
2. **Key:** `SUI_PRIVATE_KEY`
3. **Value:** Chọn một trong hai:

   **Option A: Dùng hex (không có 0x):**
   ```
   cc47188441e52eea4f98bac2161ad0559c1297adfc70f93d75f18115d771fe9a
   ```

   **Option B: Dùng base64 (khuyến nghị):**
   ```
   xEcYhEHlLupPmLrCFhrQVZwSlr38cPk9dfGBFddx/po=
   ```

4. **Click "Save Changes"**

### Bước 3: Đợi deploy

- Render sẽ tự động restart service
- Đợi **1-2 phút** để deploy xong

---

## ✅ Verify

### Check Render Logs

Vào Render Dashboard → Logs tab, phải thấy:
```
[Mint] Config check: {
  hasPackageId: true,
  hasPrivateKey: true,  ← Phải là true
  canMint: true        ← Phải là true
}
```

**Nếu vẫn thấy:**
```
hasPrivateKey: false
canMint: false
```
→ Check lại env var đã được save chưa, hoặc restart service.

---

## 🧪 Test Mint NFT

### Từ Unity:

1. **Mở game trong Unity**
2. **Nhấn F9** → Web3DebugPanel
3. **Set wallet address:** `0xcc47188441e52eea4f98bac2161ad0559c1297adfc70f18115d771fe9a`
4. **Craft NFT item** (ví dụ: "Legendary Hoe")
5. **Check Unity Console:**
   - ✅ **Success:** `ObjectId: 0x...` (không phải `0xFAKE_...`)
   - ❌ **Fail:** `ObjectId: 0xFAKE_...` → Check Render logs

---

## 🔍 Troubleshooting

### Nếu vẫn trả về `0xFAKE_...`:

1. **Check Render Logs:**
   - Xem có error gì không
   - Check `hasPrivateKey` có phải `true` không

2. **Verify env var:**
   - Vào Render Dashboard → Environment tab
   - Check `SUI_PRIVATE_KEY` có giá trị đúng không
   - **Lưu ý:** Không có spaces, không có `0x` prefix (nếu dùng hex)

3. **Restart service:**
   - Render Dashboard → Manual Deploy
   - Đợi deploy xong

4. **Test từ terminal:**
   ```bash
   curl -X POST https://web3-backend-hdsw.onrender.com/api/mint \
     -H "Content-Type: application/json" \
     -d '{
       "walletAddress": "0xcc47188441e52eea4f98bac2161ad0559c1297adfc70f18115d771fe9a",
       "itemId": "test_item_01"
     }'
   ```
   
   **Success:** `{"objectId":"0x...","transactionDigest":"..."}`
   **Fail:** `{"objectId":"0xFAKE_..."}` → Check logs

---

## ⚠️ Lưu ý bảo mật

- ✅ **Đã thêm vào Render env vars** (an toàn, không commit vào Git)
- ❌ **KHÔNG commit private key vào code**
- ❌ **KHÔNG share private key** với ai
- ⚠️ **Chỉ dùng testnet key** cho development

---

**Sau khi thêm SUI_PRIVATE_KEY, test mint NFT từ Unity!** 🚀

