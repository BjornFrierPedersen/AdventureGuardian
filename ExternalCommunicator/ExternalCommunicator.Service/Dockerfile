FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ExternalCommunicator/ExternalCommunicator.Service/ExternalCommunicator.Service.csproj", "ExternalCommunicator/ExternalCommunicator.Service/"]
COPY ["ExternalCommunicator/ExternalCommunicator.Infrastructure/ExternalCommunicator.Infrastructure.csproj", "ExternalCommunicator/ExternalCommunicator.Infrastructure/"]
COPY ["Shared/Configuration/Configuration.csproj", "Shared/Configuration/"]
COPY ["Shared/MessageGateway/MessageGateway.csproj", "Shared/MessageGateway/"]
COPY ["Shared/WebConfiguration/WebConfiguration.csproj", "Shared/WebConfiguration/"]
RUN dotnet restore "ExternalCommunicator/ExternalCommunicator.Service/ExternalCommunicator.Service.csproj"
COPY . .
WORKDIR "/src/ExternalCommunicator/ExternalCommunicator.Service"
RUN dotnet build "ExternalCommunicator.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExternalCommunicator.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExternalCommunicator.Service.dll"]
