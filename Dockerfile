# Étape de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copie des fichiers projet et restauration des dépendances
COPY GestBibliotheque/*.csproj ./GestBibliotheque/
RUN dotnet restore ./GestBibliotheque/GestBibliotheque.csproj

# Copie du reste du code et publication
COPY . ./
RUN dotnet publish ./GestBibliotheque/GestBibliotheque.csproj -c Release -o /app

# Étape de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copie de l'application publiée
COPY --from=build /app .

# Expose le port dynamique utilisé par Render/Railway
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE ${PORT:-8080}

ENTRYPOINT ["dotnet", "GestBibliotheque.dll"]
