FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Ic.Ocelot.ClientService/Ic.Ocelot.ClientService.csproj", "Ic.Ocelot.ClientService/"]
RUN dotnet restore "Ic.Ocelot.ClientService/Ic.Ocelot.ClientService.csproj"
COPY . .
WORKDIR "/src/Ic.Ocelot.ClientService"
RUN dotnet build "Ic.Ocelot.ClientService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Ic.Ocelot.ClientService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Ic.Ocelot.ClientService.dll"]