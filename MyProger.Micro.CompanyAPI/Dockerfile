FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MyProger.Micro.CompanyAPI/MyProger.Micro.CompanyAPI.csproj", "MyProger.Micro.CompanyAPI/"]
RUN dotnet restore "MyProger.Micro.CompanyAPI/MyProger.Micro.CompanyAPI.csproj"
COPY . .
WORKDIR "/src/MyProger.Micro.CompanyAPI"
RUN dotnet build "MyProger.Micro.CompanyAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyProger.Micro.CompanyAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyProger.Micro.CompanyAPI.dll"]
