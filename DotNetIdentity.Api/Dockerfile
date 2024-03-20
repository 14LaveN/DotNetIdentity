FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DotNetIdentity.Api/DotNetIdentity.Api.csproj", "DotNetIdentity.Api/"]
RUN dotnet restore "DotNetIdentity.Api/DotNetIdentity.Api.csproj"
COPY . .
WORKDIR "/src/DotNetIdentity.Api"
RUN dotnet build "DotNetIdentity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNetIdentity.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNetIdentity.Api.dll"]
