#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameRecord.OBet/Tgs.GameRecord.OBet.csproj", "Tgs.GameRecord.OBet/"]
COPY ["Tgs.GameRecord.OBet.Common/Tgs.GameRecord.OBet.Common.csproj", "Tgs.GameRecord.OBet.Common/"]
COPY ["Tgs.GameRecord.OBet.Core/Tgs.GameRecord.OBet.Core.csproj", "Tgs.GameRecord.OBet.Core/"]
RUN dotnet restore "./Tgs.GameRecord.OBet/./Tgs.GameRecord.OBet.csproj"
COPY . .
WORKDIR "/src/Tgs.GameRecord.OBet"
RUN dotnet build "./Tgs.GameRecord.OBet.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.GameRecord.OBet.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameRecord.OBet.dll"]