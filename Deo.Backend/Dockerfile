FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.20 AS build-stage
WORKDIR /build
COPY . .
RUN dotnet publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine3.20
WORKDIR /app
COPY --from=build-stage /build/bin/Release/net8.0/publish/ .
ENTRYPOINT ["/app/Deo.Backend"]

# docker run --name deo-backend -d --network deo-network -p 5000:5000 deo-backend:latest --urls http://0.0.0.0:5000
