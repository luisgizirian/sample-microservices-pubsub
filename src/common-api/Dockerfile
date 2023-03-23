FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY common-api/CommonApi.csproj common-api/
RUN dotnet restore "common-api/CommonApi.csproj"
COPY . .
WORKDIR "/src/common-api"
RUN dotnet build "CommonApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CommonApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommonApi.dll"]
