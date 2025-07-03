FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copy csproj and restore
COPY GestBibliotheque/*.csproj ./GestBibliotheque/
RUN dotnet restore ./GestBibliotheque/GestBibliotheque.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish ./GestBibliotheque/GestBibliotheque.csproj -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .

# Configuration pour Render - utiliser la variable PORT fournie par Render
ENV ASPNETCORE_URLS=http://+:$PORT
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "GestBibliotheque.dll"]