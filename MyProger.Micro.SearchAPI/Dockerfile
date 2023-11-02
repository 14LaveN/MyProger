FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MyProger.Mciro.SearchAPI/MyProger.Mciro.SearchAPI.csproj", "MyProger.Mciro.SearchAPI/"]
RUN dotnet restore "MyProger.Mciro.SearchAPI/MyProger.Mciro.SearchAPI.csproj"
COPY . .
WORKDIR "/src/MyProger.Mciro.SearchAPI"
RUN dotnet build "MyProger.Mciro.SearchAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyProger.Mciro.SearchAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyProger.Mciro.SearchAPI.dll"]
