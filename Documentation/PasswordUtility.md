# PasswordUtility

Provides secure hashing and verification utilities using PBKDF2 with HMAC-SHA256.  
Compatible with .NET Standard 2.0 for wide support.

---

## 📦 Namespace

`LightRail.DotNet.SecurityUtilities`

## 🔐 Purpose

This class is designed for hashing and verifying passwords securely. It implements:

- PBKDF2 using HMAC-SHA256
- Salted and iterated hashing for resistance to brute-force attacks
- Constant-time comparison to defend against timing attacks

---

## ✅ Usage Examples

### Hashing a Password

```csharp
string hash = PasswordUtility.HashPassword("MySecurePassword123!");
```

### Verifying a Password

```csharp
bool isValid = PasswordUtility.VerifyPassword("MySecurePassword123!", storedHash);
```

### Constant-Time Comparison

```csharp
bool matches = PasswordUtility.FixedTimeEquals(byteArray1, byteArray2);
```

---

## 🔧 Internal Algorithm

PBKDF2-HMAC-SHA256 is manually implemented for full control and compatibility. The final hash format is:

```text
PBKDF2-SHA256:100000:<base64-salt>:<base64-hash>
```

---

## 🛠️ Public Methods

| Method | Description |
|--------|-------------|
| `HashPassword(string password)` | Hashes a password with PBKDF2-HMAC-SHA256 and returns a string representation. |
| `VerifyPassword(string password, string storedHash, ILogger logger = null)` | Verifies a password against a stored hash. |
| `FixedTimeEquals(byte[] input, byte[] hash, ILogger logger = null)` | Compares two byte arrays using constant-time logic. |
| `Pbkdf2HmacSha256(string password, byte[] salt, int iterations, int outputBytes)` | Manual PBKDF2 key derivation with HMAC-SHA256. |

---

## 🧪 Security Notes

- Hashing uses 100,000 iterations by default (modifiable in code).
- Salt size: 16 bytes
- Key size: 32 bytes
- Encoded using Base64
- Uses normalized password input (`FormKC`) for consistent hash output

---

## 📝 Logging

Optional `ILogger` can be passed to `VerifyPassword` and `FixedTimeEquals` for error logging.
