# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy source code
COPY . ./

# Publish
RUN dotnet publish -c Release -o /app/publish 

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy compiled app
COPY --from=build /app/publish .

# Expose Port
EXPOSE 8080

# Set entry point
ENTRYPOINT ["dotnet", "AuthService.dll"]