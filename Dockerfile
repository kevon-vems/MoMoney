# Build stage
FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0 AS build
WORKDIR /src
COPY . ./
RUN dotnet restore MoMoney.sln
RUN dotnet publish Website/Retire.csproj -c Release -o /app/Website
RUN dotnet publish ConsoleApp/Money.csproj -c Release -o /app/ConsoleApp

# Runtime stage
FROM mcr.microsoft.com/dotnet/nightly/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/Website ./Website
COPY --from=build /app/ConsoleApp ./ConsoleApp
EXPOSE 8080
ENTRYPOINT ["dotnet", "Website/Retire.dll"]
