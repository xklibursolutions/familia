#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
LABEL org.opencontainers.image.source=https://github.com/xklibursolutions/identity-api
LABEL org.opencontainers.image.description="Identity API"
LABEL org.opencontainers.image.licenses=MIT
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/IdentityApi/IdentityApi.csproj", "src/IdentityApi/"]
RUN dotnet restore "./src/IdentityApi/IdentityApi.csproj"
COPY . .
WORKDIR "/src/src/IdentityApi"
RUN dotnet build "./IdentityApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IdentityApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityApi.dll"]
