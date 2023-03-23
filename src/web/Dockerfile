FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY web/Web.csproj web/
RUN dotnet restore "web/Web.csproj"
COPY . .
WORKDIR "/src/web"
RUN dotnet build "Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.csproj" -c Release -o /app/publish

#SPA build
FROM node:16 as nodebuilder

# set working directory
RUN mkdir /usr/src/app
WORKDIR /usr/src/app

# add `/usr/src/app/node_modules/.bin` to $PATH
ENV PATH /usr/src/app/node_modules/.bin:$PATH


# install and cache app dependencies
COPY web/ClientApp/package.json /usr/src/app/package.json
RUN npm install

# add app

COPY web/ClientApp/. /usr/src/app

RUN npm run build

#End SPA build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# RUN mkdir -p /app/ClientApp/build
COPY --from=nodebuilder /usr/src/app/build/. /app/wwwroot/dist/public/
ENTRYPOINT ["dotnet", "Web.dll"]
