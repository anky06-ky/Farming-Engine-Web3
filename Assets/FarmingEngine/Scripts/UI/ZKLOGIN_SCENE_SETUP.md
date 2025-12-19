# ZkLogin Scene Setup Guide

## Tạo Scene Mới

1. **Tạo Scene:**
   - Unity: `File` → `New Scene`
   - Save as: `Assets/FarmingEngine/Scenes/ZkLogin.unity`

2. **Setup Scene:**
   - Add `ZkLoginBackendClient` GameObject:
     - Create Empty GameObject: `ZkLoginBackendClient`
     - **Tìm component:** Trong Inspector, click "Add Component" → Search "ZkLoginBackendClient"
     - **Hoặc:** Component sẽ tự động được tạo khi `ZkLoginPanel` gọi `ZkLoginBackendClient.Get()`
     - Set `backendBaseUrl` nếu cần (mặc định đã có)

3. **Tạo UI:**
   - Create Canvas (UI → Canvas)
   - Create Panel: `ZkLoginPanel` (UI → Panel)
   - Add Component: `ZkLoginPanel` to panel

4. **Setup UI Elements:**
   - **Google Login Button:**
     - Create Button: `GoogleLoginButton`
     - Assign to `ZkLoginPanel.googleLoginButton`
   
   - **Facebook Login Button:**
     - Create Button: `FacebookLoginButton`
     - Assign to `ZkLoginPanel.facebookLoginButton`
   
   - **Twitch Login Button:**
     - Create Button: `TwitchLoginButton`
     - Assign to `ZkLoginPanel.twitchLoginButton`
   
   - **Close Button:**
     - Create Button: `CloseButton`
     - Assign to `ZkLoginPanel.closeButton`
   
   - **Status Text:**
     - Create Text: `StatusText`
     - Assign to `ZkLoginPanel.statusText`
   
   - **Wallet Address Text:**
     - Create Text: `WalletAddressText`
     - Assign to `ZkLoginPanel.walletAddressText`

5. **Setup Panel Root:**
   - Assign panel GameObject to `ZkLoginPanel.panelRoot` (hoặc để null để dùng gameObject)

## Cách Sử Dụng

1. **Load Scene:**
   ```csharp
   UnityEngine.SceneManagement.SceneManager.LoadScene("ZkLogin");
   ```

2. **Show Panel từ code:**
   ```csharp
   ZkLoginPanel panel = FindObjectOfType<ZkLoginPanel>();
   if (panel != null)
       panel.Show();
   ```

3. **User Flow:**
   - User mở scene
   - Chọn provider (Google/Facebook/Twitch)
   - Click login button
   - Wait for completion (2 seconds trong demo)
   - Wallet address được lưu vào PlayerData
   - Auto-sync NFT inventory

## Integration với Game

Sau khi login thành công:
- Wallet address được lưu vào `PlayerData.wallet_address`
- `Web3WalletValidator` sẽ detect wallet
- User cần sync NFT inventory để chơi game

## Notes

- Scene này có thể được dùng như login screen khi game start
- Hoặc có thể integrate vào main menu
- Trong WebGL build, `Application.OpenURL()` sẽ mở browser cho OAuth

