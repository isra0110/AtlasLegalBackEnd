
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000


FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY Atlas.Legal.sln ./
COPY Atlas.Legal.Application/*.csproj ./Atlas.Legal.Application/
COPY Atlas.Legal.Core/*.csproj ./Atlas.Legal.Core/
COPY Atlas.Legal.EntityFrameworkCore/*.csproj ./Atlas.Legal.EntityFrameworkCore/
COPY Atlas.Legal.Web/*.csproj ./Atlas.Legal.Web/

RUN dotnet restore
COPY . .
WORKDIR /src/Atlas.Legal.Application
RUN dotnet build -c Release -o /app

WORKDIR /src/Atlas.Legal.Core
RUN dotnet build -c Release -o /app

WORKDIR /src/Atlas.Legal.EntityFrameworkCore
RUN dotnet build -c Release -o /app

WORKDIR /src/Atlas.Legal.Web
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Atlas.Legal.Web.dll"]

