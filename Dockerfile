# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY PermissionStack.API.sln .
COPY PermissionStack.API/*.csproj ./PermissionStack.API/
COPY PermissionStack.Application/*.csproj ./PermissionStack.Application/
COPY PermissionStack.Domain/*.csproj ./PermissionStack.Domain/
COPY PermissionStack.Infrastructure/*.csproj ./PermissionStack.Infrastructure/
COPY PermissionStack.Tests/*.csproj ./PermissionStack.Tests/

RUN dotnet restore

COPY . .
WORKDIR /src/PermissionStack.API
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
EXPOSE 8080

ENTRYPOINT ["dotnet", "PermissionStack.API.dll"]

