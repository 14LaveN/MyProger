FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MyProger.Micro.MessageAPI/MyProger.Micro.MessageAPI.csproj", "MyProger.Micro.MessageAPI/"]
RUN dotnet restore "MyProger.Micro.MessageAPI/MyProger.Micro.MessageAPI.csproj"
COPY . .
WORKDIR "/src/MyProger.Micro.MessageAPI"
RUN dotnet build "MyProger.Micro.MessageAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyProger.Micro.MessageAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyProger.Micro.MessageAPI.dll"]
