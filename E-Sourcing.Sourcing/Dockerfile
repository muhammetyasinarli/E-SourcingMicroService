#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["E-Sourcing.Sourcing/E-Sourcing.Sourcing.csproj", "E-Sourcing.Sourcing/"]
COPY ["EventBusRabbitMQ/EventBusRabbitMQ.csproj", "EventBusRabbitMQ/"]
RUN dotnet restore "E-Sourcing.Sourcing/E-Sourcing.Sourcing.csproj"
COPY . .
WORKDIR "/src/E-Sourcing.Sourcing"
RUN dotnet build "E-Sourcing.Sourcing.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "E-Sourcing.Sourcing.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "E-Sourcing.Sourcing.dll"]