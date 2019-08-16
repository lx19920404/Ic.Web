FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Ic.Registry/Ic.Registry.csproj", "Ic.Registry/"]
RUN dotnet restore "Ic.Registry/Ic.Registry.csproj"
COPY . .
WORKDIR "/src/Ic.Registry"
RUN dotnet build "Ic.Registry.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Ic.Registry.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Ic.Registry.dll"]