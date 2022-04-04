#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#https://docs.microsoft.com/en-us/visualstudio/containers/tutorial-multicontainer?view=vs-2022

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AccountAndJwt.AuthorizationService/AccountAndJwt.AuthorizationService.csproj", "AccountAndJwt.AuthorizationService/"]
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