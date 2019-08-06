FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Ic.Web/Ic.Web.csproj", "Ic.Web/"]
RUN dotnet restore "Ic.Web/Ic.Web.csproj"
COPY . .
WORKDIR "/src/Ic.Web"
RUN dotnet build "Ic.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Ic.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Ic.Web.dll"]