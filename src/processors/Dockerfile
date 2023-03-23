FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY processors/Processors.csproj processors/
RUN dotnet restore "processors/Processors.csproj"
COPY . .
WORKDIR "/src/processors"
RUN dotnet build "Processors.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Processors.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Processors.dll"]
