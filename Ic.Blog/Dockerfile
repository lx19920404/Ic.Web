#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5202

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster AS build
WORKDIR /src
COPY ["Ic.Blog.csproj", "Ic.Blog/"]
RUN dotnet restore "Ic.Blog/Ic.Blog.csproj"
COPY [".","Ic.Blog"]
WORKDIR "/src/Ic.Blog"
RUN dotnet build "Ic.Blog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ic.Blog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ic.Blog.dll", "--ip","*","--port","5202"]