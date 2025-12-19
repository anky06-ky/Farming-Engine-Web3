module farming_engine::nft {
    use sui::object::{Self, UID};
    use sui::tx_context::TxContext;
    use sui::transfer;
    use std::string::{Self, String};

    /// NFT struct representing a game item
    struct NFT has key, store {
        id: UID,
        item_id: String,
        collection: String,
        nft_type: String,
    }

    /// Mint a new NFT for a game item
    /// @param item_id: The unique identifier of the game item (e.g., "legendary_hoe_01")
    /// @param recipient: The address that will own this NFT
    public fun mint(
        item_id: vector<u8>,
        recipient: address,
        ctx: &mut TxContext
    ) {
        let nft = NFT {
            id: object::new(ctx),
            item_id: string::utf8(item_id),
            collection: string::utf8(b"FarmingEngineDemo"),
            nft_type: string::utf8(b"ITEM"),
        };
        transfer::transfer(nft, recipient);
    }

    /// Get the item_id of an NFT
    public fun item_id(nft: &NFT): String {
        nft.item_id
    }

    /// Get the collection name
    public fun collection(nft: &NFT): String {
        nft.collection
    }

    /// Get the NFT type
    public fun nft_type(nft: &NFT): String {
        nft.nft_type
    }
}

