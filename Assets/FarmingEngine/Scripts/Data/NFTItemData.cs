using UnityEngine;

namespace FarmingEngine
{
    [CreateAssetMenu(menuName = "Web3/NFT Item Data")]
    public class NFTItemData : ItemData
    {
        [Header("Web3 / NFT")]
        public string nftType;      // land, tool, pet
        public string collection;   // collection address
        public bool autoMintOnCraft = true; // Only mint when crafted if this is true
    }
}


