# Use the base image for ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image for building the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file to the container
COPY ["skill-stack-csharp.csproj", "./"]  # Assuming .csproj is in the root of the context
COPY . .

# Restore dependencies
RUN dotnet restore "skill-stack-csharp.csproj"

# Build the project
RUN dotnet build "skill-stack-csharp.csproj" -c Release -o /app/build --verbosity detailed

# Publish the application
FROM build AS publish
RUN dotnet publish "skill-stack-csharp.csproj" -c Release -o /app/publish

# Create the final image using the base image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "skill-stack-csharp.dll"]