# Pharma263 Security Fix - Quick Checklist

**Start Date**: _____________
**Target Completion**: _____________

---

## 🔴 CRITICAL - Must Complete Before Production (Day 1-2)

### Secrets Management
- [ ] Set up User Secrets for local development
- [ ] Remove all hardcoded passwords from appsettings.json
- [ ] Remove all hardcoded API keys from appsettings.json
- [ ] Remove JWT signing key from appsettings.json
- [ ] Update .gitignore to exclude secret files
- [ ] Document secret setup in README

### Credential Rotation
- [ ] Change database password
- [ ] Change SMTP password
- [ ] Generate new JWT signing key (256-bit)
- [ ] Revoke and regenerate SendGrid API key
- [ ] Update all secrets in secure storage
- [ ] Verify old credentials no longer work

### CORS Security
- [ ] Add AllowedOrigins configuration
- [ ] Update Program.cs to use specific origins in production
- [ ] Keep development policy for local testing
- [ ] Enable credentials for production policy
- [ ] Test from legitimate domains
- [ ] Verify unauthorized origins are blocked

### Authorization
- [ ] Add global AuthorizeFilter
- [ ] Mark login endpoint with [AllowAnonymous]
- [ ] Mark forgot-password with [AllowAnonymous]
- [ ] Mark reset-password with [AllowAnonymous]
- [ ] Mark confirm-email with [AllowAnonymous]
- [ ] Test all endpoints return 401 without token
- [ ] Test public endpoints work without authentication

### Error Handling
- [ ] Update ExceptionMiddleware to hide stack traces in production
- [ ] Test error responses in development (shows details)
- [ ] Test error responses in production (hides details)
- [ ] Verify errors are logged for monitoring

---

## 🟠 HIGH PRIORITY (Day 3)

### Azure Key Vault
- [ ] Create Azure Key Vault resource
- [ ] Add all secrets to Key Vault
- [ ] Install Azure.Identity package
- [ ] Update Program.cs to load from Key Vault
- [ ] Configure Managed Identity in Azure
- [ ] Grant Key Vault access to app
- [ ] Test application loads secrets

### JWT Improvements
- [ ] Add refresh token duration to config
- [ ] Create RefreshToken entity
- [ ] Add database migration
- [ ] Implement RefreshToken service
- [ ] Add refresh-token endpoint
- [ ] Add token revocation support
- [ ] Test refresh token flow

### Testing Infrastructure
- [ ] Create test project
- [ ] Add testing packages (xUnit, FluentAssertions)
- [ ] Create WebApplicationFactory
- [ ] Write authentication tests
- [ ] Write authorization tests
- [ ] Write validation tests
- [ ] Configure CI/CD to run tests

### PDF Library Upgrade
- [ ] Choose replacement library (QuestPDF recommended)
- [ ] Remove iTextSharp package
- [ ] Install new PDF library
- [ ] Migrate BaseReport class
- [ ] Migrate SalesInvoiceReport
- [ ] Migrate PurchaseInvoiceReportNew
- [ ] Migrate QuotationInvoiceReport
- [ ] Compare visual output
- [ ] Performance test

---

## 🟡 MEDIUM PRIORITY (Day 4-5)

### Unit Tests
- [ ] Write CalculationService tests
- [ ] Write validation logic tests
- [ ] Write repository tests
- [ ] Write AuthService tests
- [ ] Write SalesService tests
- [ ] Achieve 80% code coverage
- [ ] Tests run in under 30 seconds

### API Documentation
- [ ] Enable XML documentation in .csproj
- [ ] Configure Swagger to use XML comments
- [ ] Add XML comments to AccountController
- [ ] Add XML comments to SaleController
- [ ] Add XML comments to key endpoints
- [ ] Verify Swagger shows descriptions

### Dependency Management
- [ ] Create Directory.Build.props
- [ ] Add centralized package versions
- [ ] Update all .csproj files
- [ ] Align JWT Bearer version (8.0.4)
- [ ] Build all projects successfully

### Rate Limiting
- [ ] Add rate limiting service
- [ ] Configure global rate limit (100/min)
- [ ] Configure login rate limit (5/5min)
- [ ] Apply to login endpoint
- [ ] Test rate limit enforcement
- [ ] Verify 429 responses

### Health Checks
- [ ] Install health check packages
- [ ] Add database health check
- [ ] Add SQL Server health check
- [ ] Configure health check endpoints
- [ ] Test /health endpoint
- [ ] Test /health/ready endpoint

---

## 🔵 FUTURE ENHANCEMENTS (Later)

- [ ] API versioning
- [ ] Application Insights monitoring
- [ ] Solution consolidation analysis
- [ ] SPA frontend evaluation
- [ ] Performance optimization
- [ ] Advanced logging features

---

## Pre-Deployment Verification

### Security
- [ ] No secrets in git history
- [ ] CORS allows only production domains
- [ ] All sensitive endpoints require authentication
- [ ] JWT tokens properly configured
- [ ] Rate limiting active

### Testing
- [ ] All integration tests pass
- [ ] Authentication tests pass
- [ ] Authorization tests pass
- [ ] Manual testing complete

### Configuration
- [ ] Azure Key Vault configured
- [ ] Production connection strings tested
- [ ] Environment variables set
- [ ] Managed Identity configured

### Monitoring
- [ ] Health checks responding
- [ ] Logging configured
- [ ] Error tracking enabled
- [ ] Alerts configured

### Documentation
- [ ] README updated with setup instructions
- [ ] Security practices documented
- [ ] Team trained on new processes
- [ ] Deployment guide updated

---

## Sign-Off

| Role | Name | Date | Signature |
|------|------|------|-----------|
| Developer | | | |
| Security Lead | | | |
| Tech Lead | | | |
| DevOps | | | |

---

**Notes**:
- Mark completed items with an [x]
- Add notes for any blockers
- Update dates as tasks are completed
- Review daily with team
