FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["coffee-shop-backend/coffee-shop-backend.csproj", "coffee-shop-backend/"]
RUN dotnet restore "coffee-shop-backend/coffee-shop-backend.csproj"
COPY . .
WORKDIR "/src/coffee-shop-backend"
RUN dotnet build "coffee-shop-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "coffee-shop-backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "coffee-shop-backend.dll"]
