FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy props files first
COPY ["Directory.Build.props", "./"]
COPY ["Yantra.sln", "./"]

# Copy project files first
COPY ["source/Yantra.API/Yantra.API.csproj", "src/Yantra.API/"]
COPY ["source/Yantra.Notifications/Yantra.Notifications.csproj", "src/Yantra.Notifications/"]
COPY ["source/Yantra.Application/Yantra.Application.csproj", "src/Yantra.Application/"]
COPY ["source/Yantra.Infrastructure/Yantra.Infrastructure.csproj", "src/Yantra.Infrastructure/"]
COPY ["source/Yantra.Mongo/Yantra.Mongo.csproj", "src/Yantra.Mongo/"]
COPY ["source/Yantra.Mongo.Migration/Yantra.Mongo.Migration.csproj", "src/Yantra.Mongo.Migration/"]

# Restore packages
RUN dotnet restore "src/Yantra.API/Yantra.API.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build "src/Yantra.API/Yantra.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Yantra.API/Yantra.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Yantra.API.dll"]
