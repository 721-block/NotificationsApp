FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["SmsNotificator/SmsNotificator.csproj", "SmsNotificator/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["RabbitMqModule/RabbitMqModule.csproj", "RabbitMqModule/"]

RUN dotnet restore "SmsNotificator/SmsNotificator.csproj"

COPY . .

WORKDIR "/src/SmsNotificator"
RUN dotnet publish "SmsNotificator.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SmsNotificator.dll"]

