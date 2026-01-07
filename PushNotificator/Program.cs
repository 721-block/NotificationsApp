using PushNotificator;
using PushNotificator.Extensions;
using PushNotificator.Handlers;
using PushNotificator.Models;
using PushNotificator.Push;
using PushNotificator.Serializers;
using RabbitMqModule.Common;
using RabbitMqModule.RpcServer;

var builder = Host.CreateApplicationBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

services.AddSingletonSettings<RpcServerSettings>(config, "RpcServerSettings")
    .AddSingletonSettings<RabbitMqSettings>(config, "RabbitMqSettings")
    .AddScopedSettings<PushSettings>(config, "PushSettings")
    .AddScoped<IPushBulletClientFactory, PushBulletClientFactory>()
    .AddSingleton<IRabbitMqConnectionProvider, RabbitMqConnectionProvider>()
    .AddSingleton<IRpcMessageSerializer<SendPushNotificationResponse, SendPushNotificationRequest>, PushSerializer>()
    .AddScoped<IPushService, PushService>()
    .AddSingleton<IRpcServerHandler<SendPushNotificationRequest, SendPushNotificationResponse>, SendPushHandler>()
    .AddSingleton<IRpcServer, RpcServer<SendPushNotificationRequest, SendPushNotificationResponse>>();

builder.Services.AddHostedService<PushNotificatorWorker>();

var host = builder.Build();
host.Run();