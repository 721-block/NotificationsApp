FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ApiGateway/ApiGateway.csproj", "ApiGateway/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["EmailNotificator/EmailNotificator.csproj", "EmailNotificator/"]
COPY ["PushNotificator/PushNotificator.csproj", "PushNotificator/"]
COPY ["SmsNotificator/SmsNotificator.csproj", "SmsNotificator/"]
COPY ["RabbitMqModule/RabbitMqModule.csproj", "RabbitMqModule/"]

RUN dotnet restore "ApiGateway/ApiGateway.csproj"

COPY . .

WORKDIR "/src/ApiGateway"
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]