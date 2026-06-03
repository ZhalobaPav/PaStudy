# Етап 1: Збірка додатка
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо файли проєктів для відновлення пакетів (щоб закешувати цей крок)
COPY ["PavStudy.API/PavStudy.API.csproj", "PavStudy.API/"]
COPY ["PaStudy.Core/PaStudy.Core.csproj", "PaStudy.Core/"]
COPY ["PaStudy.Infrastructure/PaStudy.Infrastructure.csproj", "PaStudy.Infrastructure/"]
COPY ["PaStudy.Contracts/PaStudy.Contracts.csproj", "PaStudy.Contracts/"]

# Відновлюємо залежності головного API
RUN dotnet restore "PavStudy.API/PavStudy.API.csproj"

# Копіюємо абсолютно всі початкові коди
COPY . .

# Збираємо проєкт
WORKDIR "/src/PavStudy.API"
RUN dotnet publish "PavStudy.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Етап 2: Запуск
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "PavStudy.API.dll"]