FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["EmailNotificator/EmailNotificator.csproj", "EmailNotificator/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["RabbitMqModule/RabbitMqModule.csproj", "RabbitMqModule/"]

RUN dotnet restore "EmailNotificator/EmailNotificator.csproj"

COPY . .

WORKDIR "/src/EmailNotificator"
RUN dotnet publish "EmailNotificator.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmailNotificator.dll"]

