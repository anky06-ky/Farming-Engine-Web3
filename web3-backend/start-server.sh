#!/bin/bash

# Start Express Server Script

cd ~/public_html/backend/web3-backend || cd ~/domains/thanhphong.fun/public_html/backend/web3-backend || {
    echo "❌ Error: Cannot find web3-backend folder"
    exit 1
}

# Check if server.js exists
if [ ! -f server.js ]; then
    echo "❌ server.js not found! Run setup-hostinger.sh first"
    exit 1
fi

# Check if PM2 is installed
if command -v pm2 &> /dev/null; then
    echo "🚀 Starting server with PM2..."
    pm2 start server.js --name web3-backend
    pm2 save
    pm2 startup
    echo "✅ Server started with PM2"
    echo "📋 Commands:"
    echo "   pm2 status    - Check status"
    echo "   pm2 logs      - View logs"
    echo "   pm2 stop      - Stop server"
else
    echo "🚀 Starting server directly..."
    echo "⚠️  Note: Install PM2 for production: npm install -g pm2"
    node server.js
fi

