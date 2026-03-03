syntax=docker/dockerfile:1

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only project files needed for restore to leverage cache
COPY Presentation/StructureCodeSolution.API/StructureCodeSolution.API.csproj Presentation/StructureCodeSolution.API/
COPY Core/StructureCodeSolution.Application/StructureCodeSolution.Application.csproj Core/StructureCodeSolution.Application/
COPY Infrastructure/StructureCodeSolution.Infrastructure.MessageBus/StructureCodeSolution.Infrastructure.MessageBus.csproj Infrastructure/StructureCodeSolution.Infrastructure.MessageBus/
COPY Infrastructure/StructureCodeSolution.Infrastructure/StructureCodeSolution.Infrastructure.csproj Infrastructure/StructureCodeSolution.Infrastructure/
COPY Infrastructure/StructureCodeSolution.Persistence/StructureCodeSolution.Persistence.csproj Infrastructure/StructureCodeSolution.Persistence/

# Restore (cached unless csproj changes)
RUN dotnet restore Presentation/StructureCodeSolution.API/StructureCodeSolution.API.csproj

# Copy full source and publish
COPY . .
ARG CONFIGURATION=Release
RUN dotnet publish Presentation/StructureCodeSolution.API/StructureCodeSolution.API.csproj -c $CONFIGURATION -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install curl for HEALTHCHECK then clean apt cache
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl ca-certificates \
    && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Copy published artifacts
COPY --from=build /app/publish .

# Create non-root user and set ownership
RUN groupadd -r appuser && useradd -r -g appuser appuser \
    && chown -R appuser:appuser /app

USER appuser

# Simple healthcheck (adjust path to your real health endpoint if present)
HEALTHCHECK --interval=30s --timeout=5s --start-period=5s --retries=3 CMD curl -f http://localhost/ || exit 1

ENTRYPOINT ["dotnet", "StructureCodeSolution.API.dll"]
