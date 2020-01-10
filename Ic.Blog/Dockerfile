#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Ic.Blog/Ic.Blog.csproj", "Ic.Blog/"]
RUN dotnet restore "Ic.Blog/Ic.Blog.csproj"
COPY . .
WORKDIR "/src/Ic.Blog"
RUN dotnet build "Ic.Blog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ic.Blog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ic.Blog.dll"]