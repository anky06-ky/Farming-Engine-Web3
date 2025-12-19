# Sui Blockchain Integration Guide

✅ **Đã được tích hợp!** Backend code đã sẵn sàng dùng Sui blockchain thật.

## Prerequisites

1. **Sui SDK**: ✅ Đã có trong `package.json` (`@mysten/sui.js`)
2. **Sui CLI**: Cần cài để deploy contract (xem `DEPLOY_SUI.md`)
3. **Sui Network**: Chọn testnet hoặc mainnet

## ✅ Đã hoàn thành

- ✅ Sui client helper (`src/lib/suiClient.ts`)
- ✅ Mint function với Sui integration (`src/functions/mint.ts`)
- ✅ Query NFTs từ Sui (`src/functions/nfts.ts`)
- ✅ Smart contract Move (`sui-contract/sources/nft.move`)
- ✅ Auto fallback về fake data nếu Sui chưa setup

## Cách hoạt động

Backend tự động detect:
- **Nếu có `SUI_PACKAGE_ID`**: Dùng Sui blockchain thật
- **Nếu không có**: Fallback về fake data (để dễ test)

Không cần thay đổi code, chỉ cần setup environment variables!

## Bước 2: Environment Variables

Thêm vào Render Environment Variables:

1. Vào Render Dashboard → Chọn service `web3-backend` → Tab **"Environment"**
2. Click "Add Environment Variable" và thêm:
   - `SUI_NETWORK`: `testnet` hoặc `mainnet`
   - `SUI_PACKAGE_ID`: Package ID của smart contract
   - `SUI_PRIVATE_KEY`: Private key của wallet để mint (base64 encoded)

## Bước 3: Implement Mint Function

Thay thế fake mint logic trong `src/functions/mint.ts`:

```typescript
// Parse private key from env
const keypair = Ed25519Keypair.fromSecretKey(
  Buffer.from(process.env.SUI_PRIVATE_KEY!, "base64")
);

// Create transaction
const tx = new TransactionBlock();
tx.moveCall({
  target: `${process.env.SUI_PACKAGE_ID}::nft::mint`,
  arguments: [
    tx.pure.string(itemId),
    tx.pure.address(walletAddress),
  ],
});

// Sign and execute
const result = await client.signAndExecuteTransactionBlock({
  signer: keypair,
  transactionBlock: tx,
});

return {
  statusCode: 200,
  body: JSON.stringify({
    objectId: result.digest,
  }),
};
```

## Bước 4: Implement Query NFTs

Thay thế fake query trong `src/functions/nfts.ts`:

```typescript
// Query owned objects
const objects = await client.getOwnedObjects({
  owner: wallet,
  filter: {
    StructType: `${process.env.SUI_PACKAGE_ID}::nft::NFT`,
  },
});

// Parse objects to NFTInfo format
const items = objects.data.map((obj) => ({
  objectId: obj.data?.objectId || "",
  itemId: obj.data?.content?.fields?.itemId || "",
  nftType: "ITEM",
  collection: "FarmingEngineDemo",
}));

return {
  statusCode: 200,
  body: JSON.stringify({ items }),
};
```

## Bước 5: Smart Contract (Move)

Tạo smart contract trên Sui:

```move
module farming_engine::nft {
    use sui::object::{Self, UID};
    use sui::tx_context::{Self, TxContext};
    use sui::transfer;
    use std::string::{Self, String};

    struct NFT has key, store {
        id: UID,
        item_id: String,
        collection: String,
    }

    public fun mint(
        item_id: vector<u8>,
        recipient: address,
        ctx: &mut TxContext
    ): NFT {
        let nft = NFT {
            id: object::new(ctx),
            item_id: string::utf8(item_id),
            collection: b"FarmingEngineDemo",
        };
        transfer::transfer(nft, recipient);
        nft
    }
}
```

## Bước 6: Deploy Contract

1. Install Sui CLI: `cargo install --locked --git https://github.com/MystenLabs/sui.git --branch devnet sui`
2. Build: `sui move build`
3. Publish: `sui client publish --gas-budget 100000000`
4. Copy Package ID và thêm vào Render env vars

## Bước 7: Test

1. Deploy backend với Sui integration
2. Test mint từ Unity game
3. Check Sui Explorer: `https://suivision.xyz/object/{objectId}` hoặc `https://suiscan.xyz/object/{objectId}`

## Security Notes

- ⚠️ **KHÔNG** commit private keys vào code
- Dùng Render Environment Variables
- Consider using a service account wallet
- Add rate limiting cho mint endpoint
- Validate wallet addresses

## Resources

- [Sui Documentation](https://docs.sui.io/)
- [Sui TypeScript SDK](https://github.com/MystenLabs/sui/tree/main/sdk/typescript)
- [Suivision](https://suivision.xyz/) | [Suiscan](https://suiscan.xyz/)

