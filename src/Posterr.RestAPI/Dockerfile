#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Posterr.RestAPI/Posterr.RestAPI.csproj", "src/Posterr.RestAPI/"]
COPY ["src/Posterr.Domain/Posterr.Domain.csproj", "src/Posterr.Domain/"]
COPY ["src/Posterr.Domain.Core/Posterr.Domain.Core.csproj", "src/Posterr.Domain.Core/"]
COPY ["src/Posterr.Infra.CrossCutting.IoC.RestAPI/Posterr.Infra.CrossCutting.IoC.RestAPI.csproj", "src/Posterr.Infra.CrossCutting.IoC.RestAPI/"]
COPY ["src/Posterr.Infra.Data/Posterr.Infra.Data.csproj", "src/Posterr.Infra.Data/"]
RUN dotnet restore "src/Posterr.RestAPI/Posterr.RestAPI.csproj"
COPY . .
WORKDIR "/src/src/Posterr.RestAPI"
RUN dotnet build "Posterr.RestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Posterr.RestAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Posterr.RestAPI.dll"]