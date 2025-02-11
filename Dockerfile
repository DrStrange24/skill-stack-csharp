FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["skill-stack-csharp.csproj", "skill-stack-csharp/"]

RUN dotnet restore "skill-stack-csharp.csproj"

COPY . .
WORKDIR "/src/skill-stack-csharp"
RUN dotnet build "skill-stack-csharp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "skill-stack-csharp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "skill-stack-csharp.dll"]
