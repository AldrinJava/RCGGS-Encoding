# Use the Microsoft's official .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy the csproj file and restore any dependencies (via NuGet)
COPY ["Base64EncoderApi.csproj", "./"]
RUN dotnet restore

# Copy the project files and build the release
COPY . .
RUN dotnet publish -c Release -o out

# Generate the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Base64EncoderApi.dll"]