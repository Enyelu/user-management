#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base image for the runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["user-management.api/user-management.api.csproj", "user-management.api/"]
COPY ["user-management.core/user-management.core.csproj", "user-management.core/"]

COPY ["user-management.domain/user-management.domain.csproj", "user-management.domain/"]
COPY ["user-management.infrastructure/user-management.infrastructure.csproj", "user-management.infrastructure/"]
COPY ["user-management.test/user-management.test.csproj", "user-management.test/"]
RUN dotnet restore "user-management.api/user-management.api.csproj"

# Copy the entire source code and build the project
COPY . .
WORKDIR "/src/user-management.api"
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "user-management.api.dll"]
