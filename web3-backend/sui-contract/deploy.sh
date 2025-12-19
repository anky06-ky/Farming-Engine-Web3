#!/bin/bash

# Script để deploy Sui smart contract
# Usage: ./deploy.sh

echo "🚀 Deploying Sui NFT Contract..."
echo ""

# Check if sui is installed
if ! command -v sui &> /dev/null; then
    echo "❌ Sui CLI not found. Please install first:"
    echo "   brew install sui"
    echo "   or"
    echo "   cargo install --locked --git https://github.com/MystenLabs/sui.git --branch main sui"
    exit 1
fi

# Check active address
echo "📋 Active address:"
sui client active-address
echo ""

# Check gas balance
echo "💰 Gas balance:"
sui client gas
echo ""

# Build contract
echo "🔨 Building contract..."
sui move build

if [ $? -ne 0 ]; then
    echo "❌ Build failed!"
    exit 1
fi

echo ""
echo "✅ Build successful!"
echo ""

# Publish contract
echo "📤 Publishing contract..."
sui client publish --gas-budget 100000000

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Contract published successfully!"
    echo ""
    echo "📝 Next steps:"
    echo "1. Copy the Package ID from above (starts with 0x...)"
    echo "2. Add to Netlify Environment Variables:"
    echo "   - SUI_PACKAGE_ID = <package-id>"
    echo "   - SUI_NETWORK = testnet"
    echo "3. Deploy backend again on Netlify"
else
    echo "❌ Publish failed!"
    exit 1
fi

