FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["libfintx.api/libfintx.api.csproj", "libfintx.api/"]
RUN dotnet restore "libfintx.api/libfintx.api.csproj"
COPY . .
WORKDIR "/src/libfintx.api"
RUN dotnet build "libfintx.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "libfintx.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "libfintx.api.dll"]
