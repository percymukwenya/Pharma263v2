# Refresh Token Implementation Guide

**Status**: 🟡 Database & Models Complete - Service Layer Needed
**Date**: 2025-11-05

---

## ✅ What's Already Done

### 1. Database Layer ✅
- ✅ `RefreshToken` entity created in Domain layer
- ✅ `RefreshTokens` table SQL script created (`Database/AddRefreshTokensTable.sql`)
- ✅ `ApplicationDbContext` updated with `DbSet<RefreshToken>`
- ✅ Entity configuration created (`RefreshTokenEntityConfiguration.cs`)

### 2. Models & DTOs ✅
- ✅ `AuthResponse` updated with `RefreshToken` and `RefreshTokenExpiry` properties
- ✅ `RefreshTokenRequest` DTO created
- ✅ `JWTOptions` configuration updated with `RefreshTokenDurationInDays`
- ✅ `appsettings.json` updated (15 min access token, 7 day refresh token)

---

## 🔧 What You Need to Do Next

### Step 1: Run the Database Migration

Execute the SQL script to create the RefreshTokens table:

```sql
-- Run this on your database
-- File: Database/AddRefreshTokensTable.sql
```

**Or using SQL Server Management Studio:**
1. Open SSMS
2. Connect to `SQL8002.site4now.net`
3. Select database: `db_a9107a_pharma263`
4. Open and execute: `Database/AddRefreshTokensTable.sql`

**Verify the table was created:**
```sql
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RefreshTokens'
```

---

### Step 2: Update AuthService

Add these methods to `Pharma263.Application/Services/Identity/AuthService.cs`:

#### A. Add Helper Methods

```csharp
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

// Add these private fields to the class
private readonly IUnitOfWork _unitOfWork;
private readonly IHttpContextAccessor _httpContextAccessor;

// Update constructor
public AuthService(
    UserManager<ApplicationUser> userManager,
    IOptions<JWTOptions> jwtSettings,
    SignInManager<ApplicationUser> signInManager,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor)
{
    _userManager = userManager;
    _jwtSettings = jwtSettings.Value;
    _signInManager = signInManager;
    _emailSender = emailSender;
    _unitOfWork = unitOfWork;
    _httpContextAccessor = httpContextAccessor;
}

// Add these helper methods

private string GenerateRefreshToken()
{
    var randomNumber = new byte[64];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
}

private string GetIpAddress()
{
    if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
        return _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
    else
        return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
}

private async Task<RefreshToken> CreateRefreshToken(string userId)
{
    var refreshToken = new RefreshToken
    {
        Token = GenerateRefreshToken(),
        UserId = userId,
        ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays),
        CreatedByIp = GetIpAddress()
    };

    await _unitOfWork.Repository<RefreshToken>().AddAsync(refreshToken);
    await _unitOfWork.SaveChangesAsync();

    return refreshToken;
}
```

#### B. Update Login Method

```csharp
public async Task<ApiResponse<AuthResponse>> Login(AuthRequest request)
{
    var user = await _userManager.Users
        .SingleOrDefaultAsync(x => x.UserName == request.UserName.ToLower());

    if (user == null)
    {
        return ApiResponse<AuthResponse>.CreateFailure("Username or password is incorrect", (int)HttpStatusCode.BadRequest);
    }
    else
    {
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            return ApiResponse<AuthResponse>.CreateFailure("Username or password is incorrect", (int)HttpStatusCode.BadRequest);
        }
        else
        {
            // Generate JWT access token
            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

            // Generate refresh token
            var refreshToken = await CreateRefreshToken(user.Id);

            var data = new AuthResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiry = refreshToken.ExpiryDate
            };

            return ApiResponse<AuthResponse>.CreateSuccess(data, "Login successful", (int)HttpStatusCode.OK);
        }
    }
}
```

#### C. Add RefreshToken Method

```csharp
public async Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string token)
{
    var refreshToken = await _unitOfWork.Repository<RefreshToken>()
        .FirstOrDefaultAsync(rt => rt.Token == token);

    if (refreshToken == null)
    {
        return ApiResponse<AuthResponse>.CreateFailure("Invalid refresh token", (int)HttpStatusCode.BadRequest);
    }

    if (!refreshToken.IsActive)
    {
        return ApiResponse<AuthResponse>.CreateFailure("Refresh token expired or revoked", (int)HttpStatusCode.BadRequest);
    }

    // Get user
    var user = await _userManager.FindByIdAsync(refreshToken.UserId);
    if (user == null)
    {
        return ApiResponse<AuthResponse>.CreateFailure("User not found", (int)HttpStatusCode.NotFound);
    }

    // Revoke old refresh token
    refreshToken.IsRevoked = true;
    refreshToken.RevokedDate = DateTime.UtcNow;
    refreshToken.RevokedByIp = GetIpAddress();

    // Generate new tokens
    var jwtSecurityToken = await GenerateToken(user);
    var newRefreshToken = await CreateRefreshToken(user.Id);

    // Store replacement chain
    refreshToken.ReplacedByToken = newRefreshToken.Token;

    _unitOfWork.Repository<RefreshToken>().Update(refreshToken);
    await _unitOfWork.SaveChangesAsync();

    var data = new AuthResponse
    {
        Id = user.Id,
        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
        Email = user.Email,
        UserName = user.UserName,
        RefreshToken = newRefreshToken.Token,
        RefreshTokenExpiry = newRefreshToken.ExpiryDate
    };

    return ApiResponse<AuthResponse>.CreateSuccess(data, "Token refreshed successfully", (int)HttpStatusCode.OK);
}

public async Task<ApiResponse<bool>> RevokeTokenAsync(string token)
{
    var refreshToken = await _unitOfWork.Repository<RefreshToken>()
        .FirstOrDefaultAsync(rt => rt.Token == token);

    if (refreshToken == null || !refreshToken.IsActive)
    {
        return ApiResponse<bool>.CreateFailure("Invalid refresh token", (int)HttpStatusCode.BadRequest);
    }

    // Revoke token
    refreshToken.IsRevoked = true;
    refreshToken.RevokedDate = DateTime.UtcNow;
    refreshToken.RevokedByIp = GetIpAddress();

    _unitOfWork.Repository<RefreshToken>().Update(refreshToken);
    await _unitOfWork.SaveChangesAsync();

    return ApiResponse<bool>.CreateSuccess(true, "Token revoked successfully", (int)HttpStatusCode.OK);
}
```

#### D. Update IAuthService Interface

Add these method signatures to the `IAuthService` interface:

```csharp
// Location: Pharma263.Application/Contracts/Identity/IAuthService.cs

Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string token);
Task<ApiResponse<bool>> RevokeTokenAsync(string token);
```

---

### Step 3: Add Controller Endpoints

Add these endpoints to `AccountController.cs`:

```csharp
[HttpPost("refresh-token")]
[AllowAnonymous]
public async Task<ActionResult<ApiResponse<AuthResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
{
    var result = await _authService.RefreshTokenAsync(request.RefreshToken);
    return StatusCode(result.StatusCode, result);
}

[HttpPost("revoke-token")]
[Authorize]
public async Task<ActionResult<ApiResponse<bool>>> RevokeToken([FromBody] RefreshTokenRequest request)
{
    var result = await _authService.RevokeTokenAsync(request.RefreshToken);
    return StatusCode(result.StatusCode, result);
}
```

---

### Step 4: Add Required Using Statements

Make sure you have these using statements in `AuthService.cs`:

```csharp
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
```

---

## 🧪 Testing the Refresh Token Flow

### 1. Login and Get Tokens

```bash
curl -X POST http://localhost:5000/api/Account/login \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "youruser",
    "password": "yourpass"
  }'
```

**Expected Response:**
```json
{
  "statusCode": 200,
  "succeeded": true,
  "message": "Login successful",
  "data": {
    "id": "user-id-here",
    "userName": "youruser",
    "email": "user@example.com",
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "ABC123XYZ...",
    "refreshTokenExpiry": "2025-11-12T10:30:00Z"
  }
}
```

### 2. Use Access Token (Works for 15 minutes)

```bash
curl http://localhost:5000/api/Sale/GetSales \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN"
```

### 3. Wait for Access Token to Expire (or test immediately)

```bash
# After 15 minutes, access token expires
curl http://localhost:5000/api/Sale/GetSales \
  -H "Authorization: Bearer EXPIRED_ACCESS_TOKEN"

# Returns 401 Unauthorized
```

### 4. Refresh the Access Token

```bash
curl -X POST http://localhost:5000/api/Account/refresh-token \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "YOUR_REFRESH_TOKEN"
  }'
```

**Expected Response:**
```json
{
  "statusCode": 200,
  "succeeded": true,
  "message": "Token refreshed successfully",
  "data": {
    "id": "user-id-here",
    "userName": "youruser",
    "email": "user@example.com",
    "token": "NEW_ACCESS_TOKEN...",
    "refreshToken": "NEW_REFRESH_TOKEN...",
    "refreshTokenExpiry": "2025-11-12T10:30:00Z"
  }
}
```

### 5. Use New Access Token

```bash
curl http://localhost:5000/api/Sale/GetSales \
  -H "Authorization: Bearer NEW_ACCESS_TOKEN"

# Works! Returns sales data
```

### 6. Revoke Refresh Token (Logout)

```bash
curl -X POST http://localhost:5000/api/Account/revoke-token \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "YOUR_REFRESH_TOKEN"
  }'
```

---

## 🔒 Security Benefits

### Before Refresh Tokens:
- ❌ Access tokens valid for 30 minutes
- ❌ No way to revoke tokens
- ❌ User has to re-login every 30 minutes
- ❌ Or keep tokens valid for days (security risk)

### After Refresh Tokens:
- ✅ Access tokens valid for only 15 minutes (reduced attack window)
- ✅ Refresh tokens valid for 7 days (better UX)
- ✅ Can revoke tokens (logout, security breach response)
- ✅ Token rotation (old refresh tokens automatically revoked)
- ✅ IP tracking for security audit
- ✅ Replacement chain for forensics

---

## 📊 Token Lifecycle

```
1. Login
   ↓
2. Receive Access Token (15 min) + Refresh Token (7 days)
   ↓
3. Use Access Token for API calls
   ↓
4. Access Token Expires (15 min)
   ↓
5. Use Refresh Token to get new Access Token
   ↓
6. Receive New Access Token (15 min) + New Refresh Token (7 days)
   ↓
7. Old Refresh Token automatically revoked
   ↓
8. Repeat from step 3
   ↓
9. After 7 days, Refresh Token expires → User must login again
   ↓
10. Or user explicitly revokes token (logout)
```

---

## 🎯 Frontend Integration

### Store Tokens in Frontend

```javascript
// After login
const response = await fetch('/api/Account/login', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ userName, password })
});

const { data } = await response.json();

// Store tokens securely
localStorage.setItem('accessToken', data.token);
localStorage.setItem('refreshToken', data.refreshToken);
localStorage.setItem('refreshTokenExpiry', data.refreshTokenExpiry);
```

### Automatic Token Refresh

```javascript
// API interceptor (example with axios)
axios.interceptors.response.use(
  response => response,
  async error => {
    const originalRequest = error.config;

    // If 401 and haven't retried yet
    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        // Refresh token
        const refreshToken = localStorage.getItem('refreshToken');
        const response = await axios.post('/api/Account/refresh-token', {
          refreshToken
        });

        const { data } = response.data;

        // Update stored tokens
        localStorage.setItem('accessToken', data.token);
        localStorage.setItem('refreshToken', data.refreshToken);

        // Retry original request with new token
        originalRequest.headers['Authorization'] = `Bearer ${data.token}`;
        return axios(originalRequest);
      } catch (refreshError) {
        // Refresh failed - redirect to login
        localStorage.clear();
        window.location.href = '/login';
        return Promise.reject(refreshError);
      }
    }

    return Promise.reject(error);
  }
);
```

---

## 📋 Checklist

### Database Setup
- [ ] Run `Database/AddRefreshTokensTable.sql` on database
- [ ] Verify table created: `SELECT * FROM RefreshTokens`

### Code Updates
- [ ] Update `AuthService` constructor with `IUnitOfWork` and `IHttpContextAccessor`
- [ ] Add `GenerateRefreshToken()` helper method
- [ ] Add `GetIpAddress()` helper method
- [ ] Add `CreateRefreshToken()` helper method
- [ ] Update `Login()` method to generate refresh token
- [ ] Add `RefreshTokenAsync()` method
- [ ] Add `RevokeTokenAsync()` method
- [ ] Update `IAuthService` interface with new method signatures
- [ ] Add `refresh-token` endpoint to `AccountController`
- [ ] Add `revoke-token` endpoint to `AccountController`

### Testing
- [ ] Test login returns both tokens
- [ ] Test using access token works
- [ ] Test refresh token flow
- [ ] Test old refresh token is revoked after refresh
- [ ] Test expired refresh token is rejected
- [ ] Test revoked refresh token is rejected
- [ ] Test explicit token revocation (logout)

### Frontend Integration (Optional)
- [ ] Store tokens in localStorage/sessionStorage
- [ ] Implement automatic token refresh on 401
- [ ] Implement logout (revoke token)
- [ ] Clear tokens on logout

---

## 🐛 Troubleshooting

### Issue: "RefreshTokens table does not exist"
**Solution**: Run the SQL script in `Database/AddRefreshTokensTable.sql`

### Issue: "Cannot resolve service for type 'IUnitOfWork'"
**Solution**: `IUnitOfWork` should already be registered. Check `Pharma263.Persistence` DI registration.

### Issue: "Cannot resolve service for type 'IHttpContextAccessor'"
**Solution**: Already registered in `Program.cs` via `.AddHttpContextAccessor()`

### Issue: Refresh token returns "Invalid refresh token"
**Check**:
1. Token exists in database: `SELECT * FROM RefreshTokens WHERE Token = 'YOUR_TOKEN'`
2. Token not expired: `ExpiryDate > GETUTCDATE()`
3. Token not revoked: `IsRevoked = 0`

### Issue: IP address is null
**Solution**: Running behind proxy? Check `X-Forwarded-For` header configuration

---

## 📚 Additional Resources

- JWT Best Practices: https://tools.ietf.org/html/rfc8725
- OWASP Authentication Cheat Sheet: https://cheatsheetseries.owasp.org/cheatsheets/Authentication_Cheat_Sheet.html
- Refresh Token Security: https://auth0.com/blog/refresh-tokens-what-are-they-and-when-to-use-them/

---

## ✅ Summary

**What We Built:**
1. Database table for storing refresh tokens with audit trail
2. Entity and configuration for EF Core
3. DTOs for request/response
4. Configuration for token expiry (15 min access, 7 day refresh)

**What You Need to Add:**
1. Run database migration
2. Update AuthService with refresh token logic
3. Add API endpoints
4. Test the complete flow

**Estimated Time**: 1-2 hours

**Benefits**:
- ✅ Better security (short-lived access tokens)
- ✅ Better UX (long-lived refresh tokens)
- ✅ Token revocation capability
- ✅ Security audit trail (IP tracking)

---

**Ready to implement?** Start with Step 1 (database migration), then follow steps 2-4 in order.

**Questions?** Review the testing section for examples of expected behavior.

Good luck! 🚀
