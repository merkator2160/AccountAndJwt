#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# https://stackoverflow.com/questions/64829076/blazor-webassembly-app-with-docker-support-linux

FROM nginx:alpine AS base
WORKDIR /var/www/web
COPY --from=publish /app/publish/wwwroot .
COPY AccountAndJwt.Ui/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AccountAndJwt.Ui/AccountAndJwt.Ui.csproj", "AccountAndJwt.Ui/"]
COPY ["AccountAndJwt.Contracts/AccountAndJwt.Contracts.csproj", "AccountAndJwt.Contracts/"]
COPY ["AccountAndJwt.Common/AccountAndJwt.Common.csproj", "AccountAndJwt.Common/"]
COPY ["AccountAndJwt.ApiClients.Http/AccountAndJwt.ApiClients.Http.csproj", "AccountAndJwt.ApiClients.Http/"]
RUN dotnet restore "AccountAndJwt.Ui/AccountAndJwt.Ui.csproj"
COPY . .
WORKDIR "/src/AccountAndJwt.Ui"
RUN dotnet build "AccountAndJwt.Ui.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccountAndJwt.AuthorizationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /var/www/web
COPY --from=publish /app/publish .
EXPOSE 80