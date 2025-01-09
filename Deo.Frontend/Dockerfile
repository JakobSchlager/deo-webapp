FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.20 AS build-stage
WORKDIR /build
COPY . .
RUN dotnet publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine3.20
WORKDIR /app
COPY --from=build-stage /build/bin/Release/net8.0/publish/ .
ENTRYPOINT ["/app/Deo.Frontend"]

# docker run --name deo-frontend -d --network deo-network -p 4000:4000 -e DEO_BACKEND_URL=http://deo-backend:5000 deo-frontend:latest --urls http://0.0.0.0:4000
