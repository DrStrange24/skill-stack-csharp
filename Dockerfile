# Use the official image from Microsoft for runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["skill-stack-csharp/skill-stack-csharp.csproj", "skill-stack-csharp/"]
RUN dotnet restore "skill-stack-csharp/skill-stack-csharp.csproj"
COPY . .
WORKDIR "/src/skill-stack-csharp"
RUN dotnet build "skill-stack-csharp.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "skill-stack-csharp.csproj" -c Release -o /app/publish

# Copy the build output to the final image and set the entry point
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "skill-stack-csharp.dll"]
