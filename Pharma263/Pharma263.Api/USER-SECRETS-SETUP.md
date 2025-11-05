# User Secrets Setup Guide

## Overview
This project uses **User Secrets** for local development to keep sensitive data out of source control. Follow this guide to configure your local environment.

## Prerequisites
- .NET 8.0 SDK installed
- Visual Studio 2022 or VS Code

## Setup Steps

### Option 1: Using .NET CLI (Recommended)

1. **Navigate to the API project**
   ```bash
   cd Pharma263/Pharma263.Api
   ```

2. **Verify User Secrets ID is configured** (already done in .csproj)
   ```bash
   dotnet user-secrets list
   ```

3. **Set the required secrets**
   ```bash
   # Database Connection Strings
   dotnet user-secrets set "ConnectionStrings:Pharma263Connection" "Data Source=SQL8002.site4now.net;Initial Catalog=db_a9107a_pharma263;User Id=db_a9107a_pharma263_admin;Password=YOUR_NEW_PASSWORD"

   dotnet user-secrets set "ConnectionStrings:DapperConnection" "Data Source=SQL8002.site4now.net;Initial Catalog=db_a9107a_pharma263;User Id=db_a9107a_pharma263_admin;Password=YOUR_NEW_PASSWORD"

   # JWT Settings
   dotnet user-secrets set "JwtSettings:Key" "YOUR_SECURE_256_BIT_KEY_HERE_MINIMUM_32_CHARACTERS_LONG"

   # SMTP Settings
   dotnet user-secrets set "Smtp:Password" "YOUR_SMTP_PASSWORD"

   # SendGrid API Key (if using SendGrid)
   dotnet user-secrets set "EmailSettings:ApiKey" "YOUR_SENDGRID_API_KEY"
   ```

4. **Verify secrets are set**
   ```bash
   dotnet user-secrets list
   ```

### Option 2: Using Visual Studio

1. Right-click on the **Pharma263.Api** project
2. Select **Manage User Secrets**
3. Add the following JSON:

```json
{
  "ConnectionStrings": {
    "Pharma263Connection": "Data Source=SQL8002.site4now.net;Initial Catalog=db_a9107a_pharma263;User Id=db_a9107a_pharma263_admin;Password=YOUR_NEW_PASSWORD",
    "DapperConnection": "Data Source=SQL8002.site4now.net;Initial Catalog=db_a9107a_pharma263;User Id=db_a9107a_pharma263_admin;Password=YOUR_NEW_PASSWORD"
  },
  "JwtSettings": {
    "Key": "YOUR_SECURE_256_BIT_KEY_HERE_MINIMUM_32_CHARACTERS_LONG"
  },
  "Smtp": {
    "Password": "YOUR_SMTP_PASSWORD"
  },
  "EmailSettings": {
    "ApiKey": "YOUR_SENDGRID_API_KEY"
  }
}
```

### Option 3: Manual File Creation

The secrets file is stored at:
- **Windows**: `%APPDATA%\Microsoft\UserSecrets\pharma263-api-secrets-2024\secrets.json`
- **Linux/Mac**: `~/.microsoft/usersecrets/pharma263-api-secrets-2024/secrets.json`

Create the file manually and add the JSON from Option 2.

## Generating a Secure JWT Key

You need a cryptographically secure key (minimum 256 bits / 32 characters). Here are some options:

### Using PowerShell (Windows)
```powershell
-join ((48..57) + (65..90) + (97..122) | Get-Random -Count 64 | ForEach-Object {[char]$_})
```

### Using OpenSSL
```bash
openssl rand -base64 32
```

### Using Online Generator
Visit: https://www.random.org/strings/

Settings:
- 64 characters
- Include uppercase and lowercase letters, numbers, symbols
- Click "Get Strings"

## Getting Database Credentials

**IMPORTANT**: The old credentials have been exposed and must be rotated!

1. Contact your database administrator
2. Request new credentials for:
   - Database: `db_a9107a_pharma263`
   - Server: `SQL8002.site4now.net`
3. Update the connection strings with the new credentials

## Getting SMTP Credentials

Contact your IT administrator to get:
- SMTP password for `noreply@pharma263.com`
- Or SendGrid API key

## Production Configuration

For production deployments, use **Azure Key Vault** instead of User Secrets:

1. Store secrets in Azure Key Vault
2. Configure Managed Identity for your App Service
3. Grant Key Vault access to the Managed Identity
4. The application will automatically load secrets from Key Vault

See `SECURITY-ACTION-PLAN.md` Task 2.1 for detailed instructions.

## Troubleshooting

### Secrets not loading
- Verify you're in the correct project directory
- Check that UserSecretsId matches in .csproj
- Restart your IDE/terminal

### Invalid JWT Key
- Minimum 32 characters required
- Use cryptographically secure random generation
- Avoid simple passwords or patterns

### Database connection fails
- Verify credentials are correct
- Check network connectivity
- Ensure firewall allows SQL Server traffic

## Security Best Practices

✅ **DO**:
- Use User Secrets for local development
- Use Azure Key Vault for production
- Generate strong, random JWT keys
- Rotate credentials regularly

❌ **DON'T**:
- Commit secrets to git
- Share secrets via email or chat
- Use simple or predictable keys
- Store production secrets in User Secrets

## Questions?

Contact the development team lead for access to credentials.
