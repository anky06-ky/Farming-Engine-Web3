# Commands để Deploy Contract

Copy và chạy từng lệnh:

## 1. Check active address
```bash
sui client active-address
```

Nếu không đúng, switch:
```bash
sui client switch --address 0x8e2c64a60b96346cd780d95e9b9600630af06c9ee198dc0b59256d1f982df532
```

## 2. Check gas balance
```bash
sui client gas
```

Cần có ít nhất 0.1 SUI để deploy.

## 3. Vào thư mục contract
```bash
cd /Users/phongnguyen/MiniHackathon/web3-backend/sui-contract
```

## 4. Build contract
```bash
sui move build
```

## 5. Publish contract
```bash
sui client publish --gas-budget 100000000
```

## 6. Copy Package ID

Từ output, tìm dòng có "Published Objects" hoặc "Package ID"
Format: `0x...` (64 hex chars)

Ví dụ:
```
Published Objects:
Package ID: 0x1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef
```

## 7. Export Private Key (nếu cần)

```bash
# List addresses
sui client addresses

# Export (sẽ hiện recovery phrase)
sui client export stoic-sphene
# hoặc
sui client export suspicious-felspar
```

Lưu private key để thêm vào Netlify env vars sau.

