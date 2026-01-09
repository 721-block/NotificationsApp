FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["PushNotificator/PushNotificator.csproj", "PushNotificator/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["RabbitMqModule/RabbitMqModule.csproj", "RabbitMqModule/"]

RUN dotnet restore "PushNotificator/PushNotificator.csproj"

COPY . .

WORKDIR "/src/PushNotificator"
RUN dotnet publish "PushNotificator.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PushNotificator.dll"]

