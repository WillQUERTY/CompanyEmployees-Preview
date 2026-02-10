FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["preview/PreviewApi/PreviewApi.csproj", "preview/PreviewApi/"]
COPY ["preview/PreviewApi.Application/PreviewApi.Application.csproj", "preview/PreviewApi.Application/"]
COPY ["preview/PreviewApi.Infrastructure/PreviewApi.Infrastructure.csproj", "preview/PreviewApi.Infrastructure/"]

RUN dotnet restore "preview/PreviewApi/PreviewApi.csproj"

COPY . .
WORKDIR "/src/preview/PreviewApi"

RUN dotnet build "PreviewApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PreviewApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
EXPOSE 8080
EXPOSE 8443

COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Docker
ENTRYPOINT ["dotnet", "PreviewApi.dll"]
