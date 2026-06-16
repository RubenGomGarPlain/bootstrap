# Security Checklist

Extracted from engineering-conventions security section.

## Secrets Management

- [ ] No secrets in code or config files
- [ ] Secrets managed via Azure Key Vault or similar
- [ ] Connection strings use managed identity where possible
- [ ] .env files excluded from source control (.gitignore)

## Transport & Network

- [ ] HTTPS enforced (HSTS enabled)
- [ ] CORS properly configured (no wildcard in production)
- [ ] Rate limiting configured
- [ ] Security headers set (HSTS, CSP, X-Content-Type-Options, X-Frame-Options)

## Authentication & Authorization

- [ ] Authentication on all non-public endpoints
- [ ] Authorization checked at handler level
- [ ] Token validation configured correctly
- [ ] Session timeout and refresh policies defined
- [ ] No sensitive data in JWT claims

## Input & Output

- [ ] Input validation at API boundary
- [ ] Parameterized queries (no SQL injection)
- [ ] Output encoding to prevent XSS
- [ ] File upload validation (type, size, content)

## Supply Chain & Scanning

- [ ] Dependency scanning enabled (`dotnet list package --vulnerable`)
- [ ] Container images scanned for CVEs
- [ ] SAST tools in CI pipeline
- [ ] License compliance checked
- [ ] Base images pinned to specific digests

## Operational Security

- [ ] Audit logging for sensitive operations
- [ ] Error messages don't leak internal details
- [ ] Admin endpoints on separate port or network
- [ ] Principle of least privilege for service accounts
