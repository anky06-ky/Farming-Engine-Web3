using UnityEngine;

namespace FarmingEngine
{
    /// <summary>
    /// Instance component for Web3 NFT objects, can be combined with UniqueID.
    /// </summary>
    public class NFTInstance : MonoBehaviour
    {
        /// <summary>
        /// On-chain objectId on Sui for this instance.
        /// </summary>
        public string nftObjectId; // objectId trên Sui
    }
}


