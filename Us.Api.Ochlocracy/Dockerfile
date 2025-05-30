# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Us.Api.Ochlocracy/Us.Api.Ochlocracy.csproj", "Us.Api.Ochlocracy/"]
COPY ["Us.Ochlocracy.Model/Us.Ochlocracy.Model.csproj", "Us.Ochlocracy.Model/"]
COPY ["Us.Ochlocracy.Service/Us.Ochlocracy.Service.csproj", "Us.Ochlocracy.Service/"]
COPY ["Us.Congress.Proxy/Us.Congress.Proxy.csproj", "Us.Congress.Proxy/"]
COPY ["Us.Proxy.Common/Us.Proxy.Common.csproj", "Us.Proxy.Common/"]
COPY ["Us.Ochlocracy.Data/Us.Ochlocracy.Data.csproj", "Us.Ochlocracy.Data/"]
COPY ["Us.Ochlocracy.Interfaces/Us.Ochlocracy.Interfaces.csproj", "Us.Ochlocracy.Interfaces/"]
RUN dotnet restore "./Us.Api.Ochlocracy/Us.Api.Ochlocracy.csproj"
COPY . .
WORKDIR "/src/Us.Api.Ochlocracy"
RUN dotnet build "./Us.Api.Ochlocracy.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Us.Api.Ochlocracy.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Us.Api.Ochlocracy.dll", "--environment=Development"]
