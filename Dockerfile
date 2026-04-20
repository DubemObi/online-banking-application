# syntax=docker/dockerfile:1.4

# ----- build stage -----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY Banking.API/Banking.API.csproj Banking.API/
RUN dotnet restore Banking.API/Banking.API.csproj

# copy everything else and build/publish
COPY . .
WORKDIR /src/Banking.API
RUN dotnet publish -c Release -o /app/out --no-restore

# ----- runtime stage -----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Environment variables can be supplied at runtime or build time via --build-arg
# Do NOT hardcode secrets here; pass them when running the container.
ARG ConnectionStrings__DefaultConnection
ARG Jwt__Key
ARG Jwt__Issuer
ARG EmailSettings__SmtpServer
ARG EmailSettings__SmtpPort
ARG EmailSettings__SmtpUsername
ARG EmailSettings__SmtpPassword
ARG ASPNETCORE_ENVIRONMENT=Production

# optional: set defaults or leave unset
ENV ASPNETCORE_URLS=http://+:80 \
    ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT} \
    ConnectionStrings__DefaultConnection=${ConnectionStrings__DefaultConnection} \
    Jwt__Key=${Jwt__Key} \
    Jwt__Issuer=${Jwt__Issuer} \
    EmailSettings__SmtpServer=${EmailSettings__SmtpServer} \
    EmailSettings__SmtpPort=${EmailSettings__SmtpPort} \
    EmailSettings__SmtpUsername=${EmailSettings__SmtpUsername} \
    EmailSettings__SmtpPassword=${EmailSettings__SmtpPassword}

# copy published output
COPY --from=build /app/out .

# expose port
EXPOSE 80

ENTRYPOINT ["dotnet", "Banking.API.dll"]
