# Étape de construction
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copie des fichiers de projet
COPY back-end-plante.csproj ./
COPY appsettings.json ./
COPY . .

# Restauration des dépendances NuGet
RUN dotnet restore

# Construction de l'application
RUN dotnet publish -c Release -o out

# Étape d'exécution test
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Copie des fichiers binaires de l'étape de construction
COPY --from=build-env /app/out .

# Exposition du port de l'API
EXPOSE 8888
EXPOSE 5236


# Exécution de l'application
ENTRYPOINT ["dotnet", "back-end-plante.dll"]
