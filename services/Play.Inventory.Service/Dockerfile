# Use the official .NET 8 SDK as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the .csproj file and restore the dependencies
COPY "Play.Catalog.Service.csproj" .
RUN dotnet restore "Play.Catalog.Service.csproj"

# Copy the entire source code
COPY . .

# Publish the application to the /out directory
RUN dotnet publish "Play.Catalog.Service/Play.Catalog.Service.csproj" -c Release -o /out

# Use the official .NET 8 runtime as the base image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Set the working directory
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /out .

# Expose the port that the app will run on
EXPOSE 80

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "Play.Catalog.Service.dll"]
