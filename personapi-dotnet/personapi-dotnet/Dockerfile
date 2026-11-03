# Etapa 1: compilar y publicar la app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY personapi-dotnet.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app

# Etapa 2: imagen liviana solo con el runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .

EXPOSE 8080
ENTRYPOINT ["dotnet", "personapi-dotnet.dll"]
