FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ResumeGenerator.API/ResumeGenerator.API.csproj", "ResumeGenerator.API/"]
COPY ["ResumeGenerator.Core/ResumeGenerator.Core.csproj", "ResumeGenerator.Core/"]
COPY ["ResumeGenerator.Infrastructure/ResumeGenerator.Infrastructure.csproj", "ResumeGenerator.Infrastructure/"]
RUN dotnet restore "ResumeGenerator.API/ResumeGenerator.API.csproj"
COPY . .
WORKDIR "/src/ResumeGenerator.API"
RUN dotnet build "ResumeGenerator.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ResumeGenerator.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ResumeGenerator.API.dll"]
