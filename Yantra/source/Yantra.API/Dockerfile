FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0.407 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /source

# Copy project files first
COPY ["source/Yantra.API/Yantra.API.csproj", "source/Yantra.API/"]
COPY ["source/Yantra.Notifications/Yantra.Notifications.csproj", "source/Yantra.Notifications/"]
COPY ["source/Yantra.Application/Yantra.Application.csproj", "source/Yantra.Application/"]
COPY ["source/Yantra.Infrastructure/Yantra.Infrastructure.csproj", "source/Yantra.Infrastructure/"]
COPY ["source/Yantra.Mongo/Yantra.Mongo.csproj", "source/Yantra.Mongo/"]
COPY ["source/Yantra.Mongo.Migration/Yantra.Mongo.Migration.csproj", "source/Yantra.Mongo.Migration/"]

# Restore packages
RUN dotnet restore "source/Yantra.API/Yantra.API.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/source/Yantra.API"
RUN dotnet build "Yantra.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Yantra.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Yantra.API.dll"]