#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80


#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#COPY ["Presentation/ChatStock.API/ChatStock.API.csproj", "Presentation/ChatStock.API/"]
#RUN dotnet restore "Presentation/ChatStock.API/ChatStock.API.csproj"
#COPY . .
#WORKDIR "/src/Presentation/ChatStock.API"
#RUN dotnet build "ChatStock.API.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "ChatStock.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Presentation/ChatStok.UI/ChatStok.UI.csproj", "Presentation/ChatStok.UI/"]
RUN dotnet restore "Presentation/ChatStok.UI/ChatStok.UI.csproj"
COPY . .
WORKDIR "/src/Presentation/ChatStok.UI"
RUN dotnet build "ChatStok.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatStok.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatStok.UI.dll"]