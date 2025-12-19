const express = require('express');
const cors = require('cors');

// Import compiled modules
let getSuiClient, getKeypair, getPackageId, isValidSuiAddress;
let createSession, getSession, updateSession;
let TransactionBlock;

try {
  const suiClient = require('./dist/lib/suiClient');
  getSuiClient = suiClient.getSuiClient;
  getKeypair = suiClient.getKeypair;
  getPackageId = suiClient.getPackageId;
  isValidSuiAddress = suiClient.isValidSuiAddress;
} catch (e) {
  console.warn('Sui client not available:', e.message);
}

try {
  const zkloginSessions = require('./dist/lib/zkloginSessions');
  createSession = zkloginSessions.createSession;
  getSession = zkloginSessions.getSession;
  updateSession = zkloginSessions.updateSession;
} catch (e) {
  console.warn('zkLogin sessions not available:', e.message);
}

try {
  TransactionBlock = require('@mysten/sui.js/transactions').TransactionBlock;
} catch (e) {
  console.warn('Sui TransactionBlock not available:', e.message);
}

const crypto = require('crypto');
const app = express();
// Render automatically sets PORT, but default to 3000 for local dev
const PORT = process.env.PORT || 3000;

// Middleware
app.use(cors());
app.use(express.json());

// Root endpoint - API info
app.get('/', (req, res) => {
  res.json({
    service: 'mini-hackathon-web3-backend',
    version: '1.0.0',
    status: 'running',
    endpoints: {
      health: '/api/health',
      mint: 'POST /api/mint',
      nfts: 'GET /api/nfts?wallet=0x...',
      zkloginInit: 'POST /api/zklogin-init',
      zkloginStatus: 'GET /api/zklogin-status?sessionId=...'
    },
    message: 'Backend is running on Render. Use /api/* endpoints.'
  });
});

// /api endpoint - API info
app.get('/api', (req, res) => {
  res.json({
    service: 'mini-hackathon-web3-backend',
    version: '1.0.0',
    status: 'running',
    endpoints: {
      health: '/api/health',
      mint: 'POST /api/mint',
      nfts: 'GET /api/nfts?wallet=0x...',
      zkloginInit: 'POST /api/zklogin-init',
      zkloginStatus: 'GET /api/zklogin-status?sessionId=...'
    },
    message: 'Backend is running on Render. Available endpoints listed above.'
  });
});

// Health check
app.get('/api/health', (req, res) => {
  res.json({
    ok: true,
    service: 'mini-hackathon-backend',
    message: 'Backend is running on Render'
  });
});

// Mint NFT
app.post('/api/mint', async (req, res) => {
  try {
    console.log('[Mint] Received request:', {
      body: req.body,
      headers: req.headers['content-type']
    });

    const { walletAddress, itemId } = req.body;

    if (!walletAddress || !itemId) {
      console.error('[Mint] Missing parameters:', {
        hasWalletAddress: !!walletAddress,
        hasItemId: !!itemId,
        walletAddress,
        itemId
      });
      return res.status(400).json({ 
        error: 'Missing walletAddress or itemId',
        received: { walletAddress: !!walletAddress, itemId: !!itemId }
      });
    }

    console.log('[Mint] Validating wallet address:', walletAddress);
    console.log('[Mint] Address length:', walletAddress.length);
    console.log('[Mint] Address after 0x:', walletAddress.substring(2).length, 'chars');
    console.log('[Mint] isValidSuiAddress function available:', !!isValidSuiAddress);
    
    // Manual validation if function not available
    let isValid = false;
    if (isValidSuiAddress) {
      isValid = isValidSuiAddress(walletAddress);
      console.log('[Mint] Validation result (function):', isValid);
    } else {
      // Fallback: manual regex check
      const hexPart = walletAddress.substring(2);
      isValid = /^0x[a-fA-F0-9]{64}$/.test(walletAddress) && hexPart.length === 64;
      console.log('[Mint] Validation result (manual):', isValid);
    }
    
    if (!isValid) {
      console.error('[Mint] Invalid wallet address format:', walletAddress);
      console.error('[Mint] Address details:', {
        length: walletAddress.length,
        hexPartLength: walletAddress.substring(2).length,
        startsWith0x: walletAddress.startsWith('0x'),
        hexCharsOnly: /^0x[a-fA-F0-9]+$/.test(walletAddress)
      });
      return res.status(400).json({ 
        error: 'Invalid wallet address format',
        received: walletAddress,
        receivedLength: walletAddress.length,
        hexPartLength: walletAddress.substring(2).length,
        expectedFormat: '0x followed by exactly 64 hex characters'
      });
    }

    const useSui = !!process.env.SUI_PACKAGE_ID;
    const canMint = useSui && !!process.env.SUI_PRIVATE_KEY;

    // Debug logging
    console.log('[Mint] Config check:', {
      hasPackageId: !!process.env.SUI_PACKAGE_ID,
      hasPrivateKey: !!process.env.SUI_PRIVATE_KEY,
      hasSuiClient: !!getSuiClient,
      hasKeypair: !!getKeypair,
      hasPackageIdFn: !!getPackageId,
      hasTransactionBlock: !!TransactionBlock,
      useSui,
      canMint
    });

    if (useSui && canMint && getSuiClient && getKeypair && getPackageId && TransactionBlock) {
      try {
        console.log('[Mint] Attempting real Sui mint...');
        const client = getSuiClient();
        const keypair = getKeypair();
        const packageId = getPackageId();

        console.log('[Mint] Package ID:', packageId);
        console.log('[Mint] Wallet:', walletAddress);
        console.log('[Mint] Item ID:', itemId);

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

        console.log('[Mint] ✅ Success! ObjectId:', objectId);
        return res.json({ objectId, transactionDigest: result.digest });
      } catch (suiError) {
        console.error('[Mint] ❌ Sui mint error:', suiError);
        console.error('[Mint] Error details:', {
          message: suiError.message,
          stack: suiError.stack
        });
        // Don't fallback silently - return error
        return res.status(500).json({ 
          error: 'Sui mint failed', 
          details: suiError.message,
          fallback: 'Using fake data'
        });
      }
    }

    // Fallback to fake data
    console.warn('[Mint] ⚠️ Using FAKE data. Reason:', {
      useSui,
      canMint,
      hasSuiClient: !!getSuiClient,
      hasKeypair: !!getKeypair,
      hasPackageIdFn: !!getPackageId,
      hasTransactionBlock: !!TransactionBlock,
      envVars: {
        SUI_PACKAGE_ID: process.env.SUI_PACKAGE_ID ? 'SET' : 'MISSING',
        SUI_PRIVATE_KEY: process.env.SUI_PRIVATE_KEY ? 'SET' : 'MISSING',
        SUI_NETWORK: process.env.SUI_NETWORK || 'NOT SET'
      }
    });
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

    if (useSui && isValidSuiAddress && isValidSuiAddress(wallet) && getSuiClient && getPackageId) {
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

    if (createSession) {
      createSession(sessionId, provider);

      setTimeout(() => {
        if (updateSession) {
          updateSession(sessionId, {
            status: 'success',
            walletAddress: walletAddress,
          });
        }
      }, 2000);
    }

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

    if (!getSession) {
      return res.status(500).json({ error: 'Session management not available' });
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
  console.log(`Health check: http://localhost:${PORT}/api/health`);
});

