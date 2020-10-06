#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Calabonga.BackgroundWorker.Api/Calabonga.BackgroundWorker.Api.Web/Calabonga.BackgroundWorker.Api.Web.csproj", "Calabonga.BackgroundWorker.Api/Calabonga.BackgroundWorker.Api.Web/"]
COPY ["Calabonga.BackgroundWorker.Api/Calabonga.BackgroundWorker.Api.Data/Calabonga.BackgroundWorker.Api.Data.csproj", "Calabonga.BackgroundWorker.Api/Calabonga.BackgroundWorker.Api.Data/"]
COPY ["Calabonga.BackgroundWorker.Api/Calabonga.BackgroundWorker.Api.Core/Calabonga.BackgroundWorker.Api.Core.csproj", "Calabonga.BackgroundWorker.Api/Calabonga.BackgroundWorker.Api.Core/"]
COPY ["Calabonga.BackgroundWorker.Api/Calabonga.BackgroundWorker.Api.Entities/Calabonga.BackgroundWorker.Api.Entities.csproj", "Calabonga.BackgroundWorker.Api/Calabonga.BackgroundWorker.Api.Entities/"]
RUN dotnet restore "Calabonga.BackgroundWorker.Api/Calabonga.BackgroundWorker.Api.Web/Calabonga.BackgroundWorker.Api.Web.csproj"
COPY . .
WORKDIR "/src/Calabonga.BackgroundWorker.Api/Calabonga.BackgroundWorker.Api.Web"
RUN dotnet build "Calabonga.BackgroundWorker.Api.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Calabonga.BackgroundWorker.Api.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Calabonga.BackgroundWorker.Api.Web.dll"]