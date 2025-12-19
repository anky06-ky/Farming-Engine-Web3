#!/bin/bash

# Hostinger Backend Setup Script
# Chạy script này sau khi deploy từ Git

echo "🚀 Starting Hostinger Backend Setup..."

# Navigate to backend folder
cd ~/public_html/backend/web3-backend || cd ~/domains/thanhphong.fun/public_html/backend/web3-backend || {
    echo "❌ Error: Cannot find web3-backend folder"
    echo "Current directory: $(pwd)"
    echo "Please navigate to: ~/public_html/backend/web3-backend"
    exit 1
}

echo "✅ Found web3-backend folder: $(pwd)"

# Check Node.js
if ! command -v node &> /dev/null; then
    echo "❌ Node.js not found!"
    echo "Please install Node.js or upgrade to Business plan"
    exit 1
fi

echo "✅ Node.js version: $(node --version)"
echo "✅ npm version: $(npm --version)"

# Install dependencies
echo "📦 Installing dependencies..."
npm install

if [ $? -ne 0 ]; then
    echo "❌ npm install failed"
    exit 1
fi

# Build TypeScript
echo "🔨 Building TypeScript..."
npm run build

if [ $? -ne 0 ]; then
    echo "❌ Build failed"
    exit 1
fi

# Create .env file if not exists
if [ ! -f .env ]; then
    echo "📝 Creating .env file..."
    cat > .env << EOF
NODE_ENV=production
PORT=3000
SUI_NETWORK=testnet
# Add your SUI_PACKAGE_ID and SUI_PRIVATE_KEY here if needed
EOF
    echo "✅ .env file created. Please edit it with your environment variables."
fi

# Check if server.js exists
if [ ! -f server.js ]; then
    echo "❌ server.js not found!"
    exit 1
fi

echo ""
echo "✅ Setup completed!"
echo ""
echo "📋 Next steps:"
echo "1. Edit .env file with your environment variables"
echo "2. Start server: node server.js"
echo "3. Or use PM2: pm2 start server.js --name web3-backend"
echo ""
echo "🌐 Your API will be available at:"
echo "   https://thanhphong.fun/api/health"
echo ""

