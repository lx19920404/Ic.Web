FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Ic.Identity/Ic.Identity.csproj", "Ic.Identity/"]
RUN dotnet restore "Ic.Identity/Ic.Identity.csproj"
COPY . .
WORKDIR "/src/Ic.Identity"
RUN dotnet build "Ic.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ic.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ic.Identity.dll"]