# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar csproj y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar todo y compilar
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Nombre del DLL generado (asegúrate de que coincida con tu proyecto)
ENV DOTNET_RUNNING_APP Entregable2_VilchezGuardia_JF.dll

# Exponer el puerto que Render usa
ENV ASPNETCORE_URLS=http://+:$PORT

# Ejecutar la app
CMD ["sh", "-c", "dotnet $DOTNET_RUNNING_APP"]
