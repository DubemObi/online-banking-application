# Banking API

This repository contains the Banking API project and corresponding unit tests.

## Dockerization
A multi-stage Dockerfile (`Dockerfile`) builds the application and produces a minimal runtime image:

- **Build stage** uses `mcr.microsoft.com/dotnet/sdk:8.0` to restore, build, and publish.
- **Runtime stage** uses `mcr.microsoft.com/dotnet/aspnet:8.0` and copies the published output.
- Secrets and configuration data are not baked into the image; they are provided through build arguments or runtime environment variables.

Example build command:
```bash
# supply secrets via build arguments or let them be set at container runtime
docker build \
  --build-arg ConnectionStrings__Connection="${CONN}" \
  --build-arg Jwt__Key="${JWTKEY}" \
  --build-arg Jwt__Issuer="${JWTISSUER}" \
  -t banking-api:latest .
```

Run container:
```bash
docker run -e ConnectionStrings__Connection="${CONN}" \
           -e Jwt__Key="${JWTKEY}" \
           -e Jwt__Issuer="${JWTISSUER}" \
           -e EmailSettings__SmtpServer="smtp.example.com" \
           -e EmailSettings__Port=587 \
           -e EmailSettings__Username="user" \
           -e EmailSettings__Password="pass" \
           -p 80:80 banking-api:latest
```

#### Environment Variables
Configuration relies on the default ASP.NET Core configuration providers, which automatically read environment variables. The colon (`:`) is replaced by double underscore (`__`) in variables:

- `ConnectionStrings__Connection` – database connection string (SQLite filename/URL)
- `Jwt__Key` – secret key for token signing (minimum 128 bits)
- `Jwt__Issuer` – issuer/audience value
- `EmailSettings__*` – SMTP configuration

Avoid committing any `.env` files containing secrets; the `.dockerignore` excludes them.

## CI/CD
A GitHub Actions workflow (`.github/workflows/ci.yml`) runs on push/PR to `main`:

1. Restores and builds the solution
2. Executes all unit tests
3. Builds the Docker image using build arguments from repository secrets
4. Optionally pushes the image to GitHub Container Registry if `CR_PAT` is provided

Secrets used in CI:
- `CONNECTION_STRING`
- `JWT_KEY`
- `JWT_ISSUER`
- `CR_PAT` (optional for pushing images)

---

Follow these guidelines to ensure secure handling of sensitive configuration when deploying the API in production.