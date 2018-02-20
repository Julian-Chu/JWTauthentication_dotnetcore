FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY JWTauthentication_dotnetcore/JWTauthentication_dotnetcore.csproj JWTauthentication_dotnetcore/
RUN dotnet restore
COPY . .
WORKDIR /src/JWTauthentication_dotnetcore
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "JWTauthentication_dotnetcore.dll"]
