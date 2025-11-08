# Use the official .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copy solution file and project files
COPY PinoyTodo.sln ./
COPY src/PinoyTodo.Api/PinoyTodo.Api.csproj ./src/PinoyTodo.Api/
COPY src/PinoyTodo.Application/PinoyTodo.Application.csproj ./src/PinoyTodo.Application/
COPY src/PinoyTodo.Contracts/PinoyTodo.Contracts.csproj ./src/PinoyTodo.Contracts/
COPY src/PinoyTodo.Domain/PinoyTodo.Domain.csproj ./src/PinoyTodo.Domain/
COPY src/PinoyTodo.Infrastructure/PinoyTodo.Infrastructure.csproj ./src/PinoyTodo.Infrastructure/
COPY PinoyPackages/ /PinoyPackages/

# Restore NuGet packages
# RUN dotnet restore PinoyTodo.sln
RUN dotnet restore --source "/PinoyPackages" --source "https://api.nuget.org/v3/index.json"


# Copy the entire source code
COPY . ./

# Build and publish the API project
RUN dotnet publish src/PinoyTodo.Api/PinoyTodo.Api.csproj -c Release -o out

# Use the official .NET 9.0 ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build-env /app/out .

# Install essential debugging tools as root
RUN apt-get update && \
    apt-get install -y \
    curl \
    iputils-ping \
    telnet \
    dnsutils \
    net-tools \
    wget \
    && rm -rf /var/lib/apt/lists/* \
    && apt-get clean

# Create a non-root user for security
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser


# Set the entry point for the container
ENTRYPOINT ["dotnet", "PinoyTodo.Api.dll"]