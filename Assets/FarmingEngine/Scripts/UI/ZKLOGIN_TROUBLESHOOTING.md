# ZkLogin Troubleshooting

## Không thấy component ZkLoginBackendClient trong Unity?

### Giải pháp 1: Refresh Unity
1. Trong Unity Editor: `Assets` → `Refresh` (hoặc `Ctrl+R` / `Cmd+R`)
2. Đợi Unity compile lại scripts
3. Tìm lại component trong "Add Component" menu

### Giải pháp 2: Kiểm tra file location
Component phải ở: `Assets/FarmingEngine/Scripts/Web3/ZkLoginBackendClient.cs`

Nếu không thấy:
1. Kiểm tra file có tồn tại không
2. Kiểm tra file có extension `.cs` không
3. Kiểm tra namespace: `namespace FarmingEngine`

### Giải pháp 3: Component tự động tạo
**Không cần add component thủ công!**

`ZkLoginBackendClient` sẽ tự động được tạo khi:
- `ZkLoginPanel` gọi `ZkLoginBackendClient.Get()`
- Component sẽ được tạo trong scene tự động

**Cách test:**
1. Add `ZkLoginPanel` component vào GameObject
2. Play scene
3. Component `ZkLoginBackendClient` sẽ tự động xuất hiện trong Hierarchy

### Giải pháp 4: Tìm component trong menu
1. Click "Add Component" button
2. Search: `ZkLoginBackendClient`
3. Hoặc search: `ZkLogin` (sẽ hiện cả ZkLoginPanel và ZkLoginBackendClient)

### Giải pháp 5: Kiểm tra lỗi compile
1. Mở Unity Console (`Window` → `General` → `Console`)
2. Kiểm tra có lỗi compile không
3. Nếu có lỗi, fix lỗi trước
4. Unity sẽ không hiện component nếu có lỗi compile

### Giải pháp 6: Reimport script
1. Right-click file `ZkLoginBackendClient.cs` trong Project window
2. Chọn `Reimport`
3. Đợi Unity compile lại

### Giải pháp 7: Kiểm tra Assembly Definition
Nếu project dùng Assembly Definition Files (.asmdef):
- Đảm bảo `ZkLoginBackendClient.cs` nằm trong assembly đúng
- Hoặc thêm reference nếu cần

---

## Component không hoạt động?

### Kiểm tra:
1. **Backend URL đúng chưa?**
   - Mặc định: `https://web3farming.netlify.app/.netlify/functions`
   - Có thể set trong Inspector hoặc code

2. **Backend đã deploy chưa?**
   - Test: `https://web3farming.netlify.app/.netlify/functions/health`
   - Phải trả về `{"ok": true}`

3. **Check Unity Console logs:**
   - Tìm logs bắt đầu với `[ZkLogin]`
   - Xem có lỗi gì không

---

## Vẫn không được?

1. **Restart Unity Editor**
2. **Xóa Library folder và reimport** (cẩn thận!)
3. **Kiểm tra Unity version:** Cần Unity 2019.4+ (vì dùng `UnityWebRequest`)

