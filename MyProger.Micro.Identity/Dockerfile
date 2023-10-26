FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MyProger.Micro.Identity/MyProger.Micro.Identity.csproj", "MyProger.Micro.Identity/"]
RUN dotnet restore "MyProger.Micro.Identity/MyProger.Micro.Identity.csproj"
COPY . .
WORKDIR "/src/MyProger.Micro.Identity"
RUN dotnet build "MyProger.Micro.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyProger.Micro.Identity.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyProger.Micro.Identity.dll"]
