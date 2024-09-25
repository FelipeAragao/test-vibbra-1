# Etapa 1: Imagem base para a construção
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo de solução e o arquivo .csproj
COPY ./src/EcommerceVibbra.csproj ./EcommerceVibbra/

# Restaura as dependências
RUN dotnet restore ./EcommerceVibbra/EcommerceVibbra.csproj

# Copia todo o restante do código-fonte
COPY ./src ./EcommerceVibbra/

# Define o diretório de trabalho correto
WORKDIR /src/EcommerceVibbra

# Publica a aplicação
RUN dotnet publish -c Release -o /app

# Etapa 2: Imagem base para a execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia os binários da aplicação gerados na etapa de build
COPY --from=build /app .

# Expõe a porta padrão para aplicações ASP.NET Core
EXPOSE 8080

# Define o comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "EcommerceVibbra.dll"]
