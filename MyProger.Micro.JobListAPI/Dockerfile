FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MyProger.Micro.JobListAPI/MyProger.Micro.JobListAPI.csproj", "MyProger.Micro.JobListAPI/"]
RUN dotnet restore "MyProger.Micro.JobListAPI/MyProger.Micro.JobListAPI.csproj"
COPY . .
WORKDIR "/src/MyProger.Micro.JobListAPI"
RUN dotnet build "MyProger.Micro.JobListAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyProger.Micro.JobListAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyProger.Micro.JobListAPI.dll"]
