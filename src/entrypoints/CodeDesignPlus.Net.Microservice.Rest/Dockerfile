#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# Install clang/zlib1g-dev dependencies for publishing to native
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev \
    && apt-get clean

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/domain/", "domain/"]
COPY ["src/entrypoints/CodeDesignPlus.Net.Microservice.Rest/", "entrypoints/CodeDesignPlus.Net.Microservice.Rest/"]
RUN dotnet restore "./entrypoints/CodeDesignPlus.Net.Microservice.Rest/CodeDesignPlus.Net.Microservice.Rest.csproj"

WORKDIR "/src/entrypoints/CodeDesignPlus.Net.Microservice.Rest"
RUN dotnet build "./CodeDesignPlus.Net.Microservice.Rest.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
WORKDIR /src
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./entrypoints/CodeDesignPlus.Net.Microservice.Rest/CodeDesignPlus.Net.Microservice.Rest.csproj" \
    -c "$BUILD_CONFIGURATION" \
    -o /app/publish /p:UseAppHost=true

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
USER app
WORKDIR /app
EXPOSE 5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeDesignPlus.Net.Microservice.Rest.dll"]