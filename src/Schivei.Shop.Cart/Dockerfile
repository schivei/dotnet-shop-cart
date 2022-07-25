#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ./ .
RUN dotnet restore
RUN dotnet test --collect:"Code Coverage" --nologo
WORKDIR "/src/src/Schivei.Shop.Cart"
RUN dotnet build "Schivei.Shop.Cart.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Schivei.Shop.Cart.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Schivei.Shop.Cart.dll"]