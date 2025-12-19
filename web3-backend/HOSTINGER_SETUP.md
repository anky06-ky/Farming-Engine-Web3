# Deploy lên Hostinger (Shared Hosting)

## 🎯 Tình huống
Bạn đã có Hostinger hosting và domain sẵn. Cần deploy Node.js backend lên đó.

## ⚠️ Lưu ý
Hostinger shared hosting **KHÔNG hỗ trợ serverless functions** như Netlify. Cần chuyển sang **Express.js server**.

## 📋 Prerequisites

1. ✅ Hostinger hosting (Single plan)
2. ✅ Domain đã setup
3. ✅ SSH access (có thể cần request từ Hostinger support)
4. ✅ Node.js enabled trong cPanel

---

## 🚀 Bước 1: Enable Node.js trong cPanel

1. **Login vào cPanel:**
   - Vào `https://your-domain.com/cpanel`
   - Hoặc `https://hpanel.hostinger.com`

2. **Tìm "Node.js Selector" hoặc "Node.js App":**
   - Trong cPanel, search "Node.js"
   - Click vào "Node.js Selector" hoặc "Node.js App"

3. **Create Node.js App:**
   - Click "Create Application"
   - **Node.js Version:** Chọn 18.x hoặc 20.x
   - **Application Mode:** Production
   - **Application Root:** `public_html/backend` (hoặc `backend`)
   - **Application URL:** `your-domain.com/api` (hoặc subdomain)
   - **Application Startup File:** `server.js`
   - Click "Create"

4. **Lưu lại thông tin:**
   - Application URL (ví dụ: `https://your-domain.com/api`)
   - SSH command để connect

---

## 🔧 Bước 2: Convert Functions sang Express Server

Cần tạo Express server để wrap các functions.

### Tạo `server.js`:

```javascript
const express = require('express');
const cors = require('cors');
const { getSuiClient, getKeypair, getPackageId, isValidSuiAddress } = require('./dist/lib/suiClient');
const { createSession, getSession, updateSession } = require('./dist/lib/zkloginSessions');
const crypto = require('crypto');
const { TransactionBlock } = require('@mysten/sui.js/transactions');

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware
app.use(cors());
app.use(express.json());

// Health check
app.get('/api/health', (req, res) => {
  res.json({
    ok: true,
    service: 'mini-hackathon-backend',
    message: 'Backend is running on Hostinger'
  });
});

// Mint NFT
app.post('/api/mint', async (req, res) => {
  try {
    const { walletAddress, itemId } = req.body;

    if (!walletAddress || !itemId) {
      return res.status(400).json({ error: 'Missing walletAddress or itemId' });
    }

    if (!isValidSuiAddress(walletAddress)) {
      return res.status(400).json({ error: 'Invalid wallet address format' });
    }

    const useSui = !!process.env.SUI_PACKAGE_ID;
    const canMint = useSui && !!process.env.SUI_PRIVATE_KEY;

    if (useSui && canMint) {
      try {
        const client = getSuiClient();
        const keypair = getKeypair();
        const packageId = getPackageId();

        const tx = new TransactionBlock();
        tx.moveCall({
          target: `${packageId}::nft::mint`,
          arguments: [tx.pure.string(itemId), tx.pure.address(walletAddress)],
        });

        const result = await client.signAndExecuteTransactionBlock({
          signer: keypair,
          transactionBlock: tx,
          options: { showEffects: true, showObjectChanges: true },
        });

        const nftObject = result.objectChanges?.find(
          (change) => change.type === 'created' && 'objectType' in change && change.objectType?.includes('nft::NFT')
        );
        const objectId = nftObject && 'objectId' in nftObject ? nftObject.objectId : result.digest;

        return res.json({ objectId, transactionDigest: result.digest });
      } catch (suiError) {
        console.error('Sui mint error:', suiError);
      }
    }

    // Fallback to fake data
    const fakeObjectId = `0xFAKE_${itemId}_${Date.now().toString(16)}`;
    return res.json({ objectId: fakeObjectId });
  } catch (err) {
    console.error('Mint handler error:', err);
    return res.status(500).json({ error: err.message ?? 'Unknown error' });
  }
});

// Get Owned NFTs
app.get('/api/nfts', async (req, res) => {
  try {
    const wallet = req.query.wallet;
    if (!wallet) {
      return res.status(400).json({ error: 'Missing wallet query parameter' });
    }

    const useSui = !!process.env.SUI_PACKAGE_ID;

    if (useSui && isValidSuiAddress(wallet)) {
      try {
        const client = getSuiClient();
        const packageId = getPackageId();
        const nftType = `${packageId}::nft::NFT`;

        const objects = await client.getOwnedObjects({
          owner: wallet,
          filter: { StructType: nftType },
          options: { showContent: true, showType: true },
        });

        const items = [];
        for (const obj of objects.data) {
          if (obj.data?.content && 'fields' in obj.data.content) {
            const fields = obj.data.content.fields;
            items.push({
              objectId: obj.data.objectId,
              itemId: fields.item_id || '',
              nftType: fields.nft_type || 'ITEM',
              collection: fields.collection || 'FarmingEngineDemo',
            });
          }
        }
        return res.json({ items });
      } catch (suiError) {
        console.error('Sui query error:', suiError);
      }
    }

    // Fallback to fake data
    const fakeList = {
      items: [
        {
          objectId: '0xFAKE_OBJECT_1',
          itemId: 'legendary_hoe_01',
          nftType: 'ITEM',
          collection: 'FarmingEngineDemo',
        },
      ],
    };
    return res.json(fakeList);
  } catch (err) {
    console.error('NFTs handler error:', err);
    return res.status(500).json({ error: err.message ?? 'Unknown error' });
  }
});

// zkLogin Init
app.post('/api/zklogin-init', (req, res) => {
  try {
    const { provider } = req.body;

    if (!provider || !['google', 'facebook', 'twitch'].includes(provider)) {
      return res.status(400).json({ error: 'Invalid provider. Must be \'google\', \'facebook\', or \'twitch\'' });
    }

    const sessionId = crypto.randomBytes(16).toString('hex');
    const hash = crypto.createHash('sha256').update(provider + sessionId).digest('hex');
    const walletAddress = '0x' + hash.substring(0, 64);

    createSession(sessionId, provider);

    setTimeout(() => {
      updateSession(sessionId, {
        status: 'success',
        walletAddress: walletAddress,
      });
    }, 2000);

    res.json({
      sessionId,
      authUrl: process.env.ZKLOGIN_ENABLED === 'true' ? undefined : undefined,
    });
  } catch (err) {
    console.error('zkLogin init error:', err);
    return res.status(500).json({ error: err.message ?? 'Unknown error' });
  }
});

// zkLogin Status
app.get('/api/zklogin-status', (req, res) => {
  try {
    const sessionId = req.query.sessionId;

    if (!sessionId) {
      return res.status(400).json({ error: 'Missing sessionId query parameter' });
    }

    const session = getSession(sessionId);

    if (!session) {
      return res.status(404).json({ error: 'Session not found or expired' });
    }

    if (session.status === 'success' && session.walletAddress) {
      return res.json({
        status: 'success',
        walletAddress: session.walletAddress,
      });
    }

    if (session.status === 'error') {
      return res.json({
        status: 'error',
        error: 'Login failed',
      });
    }

    return res.json({ status: 'pending' });
  } catch (err) {
    console.error('zkLogin status error:', err);
    return res.status(500).json({ error: err.message ?? 'Unknown error' });
  }
});

// Start server
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
```

---

## 📦 Bước 3: Update package.json

Thêm dependencies cần thiết:

```json
{
  "name": "mini-hackathon-web3-backend",
  "version": "0.1.0",
  "private": true,
  "scripts": {
    "build": "tsc",
    "start": "node server.js",
    "dev": "node server.js"
  },
  "dependencies": {
    "@mysten/sui.js": "^0.53.0",
    "express": "^4.18.2",
    "cors": "^2.8.5"
  },
  "devDependencies": {
    "@types/node": "^20.0.0",
    "rimraf": "^5.0.5",
    "typescript": "^5.7.0"
  }
}
```

---

## 📤 Bước 4: Upload Files lên Hostinger

### Cách 1: Dùng File Manager (cPanel)

1. **Login vào cPanel**
2. **File Manager:**
   - Vào "File Manager"
   - Navigate đến `public_html/backend` (hoặc folder bạn đã set trong Node.js App)
3. **Upload files:**
   - Upload tất cả files từ `web3-backend/` folder
   - Hoặc dùng "Compress" → Upload ZIP → Extract

### Cách 2: Dùng Git (nếu có SSH)

```bash
# SSH vào server
ssh username@your-domain.com

# Navigate đến app folder
cd ~/public_html/backend

# Clone repo
git clone https://github.com/YOUR-REPO.git .

# Install dependencies
npm install

# Build TypeScript
npm run build
```

---

## 🔐 Bước 5: Setup Environment Variables

1. **Trong cPanel → Node.js App:**
   - Click vào app bạn đã tạo
   - Tìm "Environment Variables" hoặc ".env"
   - Add variables:
     ```
     SUI_NETWORK=testnet
     SUI_PACKAGE_ID=0x...
     SUI_PRIVATE_KEY=...
     NODE_ENV=production
     PORT=3000
     ```

2. **Hoặc tạo `.env` file:**
   - Tạo file `.env` trong app folder
   - Add variables như trên

---

## 🚀 Bước 6: Start Application

1. **Trong cPanel → Node.js App:**
   - Click vào app
   - Click "Start" hoặc "Restart"
   - Check logs để đảm bảo không có lỗi

2. **Test endpoints:**
   ```
   https://your-domain.com/api/health
   ```

---

## 🔄 Bước 7: Update Unity Backend URL

Trong Unity, update `backendBaseUrl`:

```csharp
public string backendBaseUrl = "https://your-domain.com/api";
```

**Thay đổi:**
- `/.netlify/functions/health` → `/api/health`
- `/.netlify/functions/mint` → `/api/mint`
- `/.netlify/functions/nfts` → `/api/nfts`
- `/.netlify/functions/zklogin-init` → `/api/zklogin-init`
- `/.netlify/functions/zklogin-status` → `/api/zklogin-status`

---

## ✅ Checklist

- [ ] Node.js enabled trong cPanel
- [ ] Node.js App created
- [ ] Express server (`server.js`) created
- [ ] `package.json` updated với Express
- [ ] Files uploaded lên server
- [ ] `npm install` chạy thành công
- [ ] `npm run build` chạy thành công
- [ ] Environment variables setup
- [ ] App started trong cPanel
- [ ] Test `/api/health` endpoint
- [ ] Unity backend URL updated
- [ ] Test từ Unity game

---

## 🐛 Troubleshooting

### Issue 1: "Cannot find module 'express'"
**Fix:** Chạy `npm install` trong app folder

### Issue 2: "Port already in use"
**Fix:** Check PORT trong environment variables, hoặc dùng port khác

### Issue 3: "Module not found" errors
**Fix:** Đảm bảo đã chạy `npm run build` để compile TypeScript

### Issue 4: App không start
**Fix:** Check logs trong cPanel → Node.js App → Logs

---

## 📞 Support

Nếu gặp vấn đề:
1. Check Hostinger documentation
2. Contact Hostinger support
3. Check Node.js logs trong cPanel

---

## 🎯 Tóm tắt

1. **Enable Node.js** trong cPanel
2. **Create Express server** (`server.js`)
3. **Upload files** lên Hostinger
4. **Install dependencies** (`npm install`)
5. **Build TypeScript** (`npm run build`)
6. **Setup env variables**
7. **Start app** trong cPanel
8. **Update Unity URL**

**URL mới:** `https://your-domain.com/api`

