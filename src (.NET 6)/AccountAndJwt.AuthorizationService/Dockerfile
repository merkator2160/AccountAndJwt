#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AccountAndJwt.AuthorizationService/AccountAndJwt.AuthorizationService.csproj", "AccountAndJwt.AuthorizationService/"]
COPY ["AccountAndJwt.Contracts/AccountAndJwt.Contracts.csproj", "AccountAndJwt.Contracts/"]
COPY ["AccountAndJwt.Common/AccountAndJwt.Common.csproj", "AccountAndJwt.Common/"]
COPY ["AccountAndJwt.ApiClients/AccountAndJwt.ApiClients.csproj", "AccountAndJwt.ApiClients/"]
COPY ["AccountAndJwt.ApiClients.Http/AccountAndJwt.ApiClients.Http.csproj", "AccountAndJwt.ApiClients.Http/"]
COPY ["AccountAndJwt.Database/AccountAndJwt.AuthorizationService.Database.csproj", "AccountAndJwt.AuthorizationService.Database/"]
RUN dotnet restore "AccountAndJwt.AuthorizationService/AccountAndJwt.AuthorizationService.csproj"
COPY . .
WORKDIR "/src/AccountAndJwt.AuthorizationService"
RUN dotnet build "AccountAndJwt.AuthorizationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccountAndJwt.AuthorizationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccountAndJwt.AuthorizationService.dll"]