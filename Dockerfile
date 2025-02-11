FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["skill-stack-csharp/skill-stack-csharp.csproj", "skill-stack-csharp/"]
COPY . .

RUN dotnet restore "skill-stack-csharp/skill-stack-csharp.csproj"

WORKDIR "/src/skill-stack-csharp"

# Add --verbosity detailed to get more detailed build logs
RUN dotnet build "skill-stack-csharp.csproj" -c Release -o /app/build --verbosity detailed

FROM build AS publish
RUN dotnet publish "skill-stack-csharp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "skill-stack-csharp.dll"]