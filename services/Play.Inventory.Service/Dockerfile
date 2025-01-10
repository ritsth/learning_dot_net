# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the .csproj file and restore dependencies
COPY *.csproj .
RUN dotnet restore

# Copy the rest of the application files
COPY . .

# Build the application
RUN dotnet publish -c Release -o /out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the build artifacts from the build stage
COPY --from=build /out .

# Expose the application's port
EXPOSE 80
EXPOSE 443

# Set the entry point
ENTRYPOINT ["dotnet", "Play.Inventory.Service.dll"]
