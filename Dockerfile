# ========================
# Etapa 1: Build
# ========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copiar archivo de proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el código y compilar en Release
COPY . ./
RUN dotnet publish -c Release -o out

# ========================
# Etapa 2: Runtime
# ========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copiar la salida del build
COPY --from=build-env /app/out .

# Render usa $PORT automáticamente → configurar ASP.NET Core
ENV ASPNETCORE_URLS=http://+:${PORT}

# Nombre de tu DLL (verificado con `dotnet publish -c Release -o out`)
CMD ["dotnet", "Entregable2-VilchezGuardia_JF.dll"]
