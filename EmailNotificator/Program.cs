using EmailNotificator;
using EmailNotificator.Extensions;
using EmailNotificator.Handlers;
using EmailNotificator.Models;
using EmailNotificator.Serializers;
using RabbitMqModule.Common;
using RabbitMqModule.RpcServer;

var builder = Host.CreateApplicationBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

services.AddSingletonSettings<RpcServerSettings>(config, "RpcServerSettings")
    .AddSingletonSettings<RabbitMqSettings>(config, "RabbitMqSettings")
    .AddSingletonSettings<MailSettings>(config, "MailSettings")
    .AddSingleton<IRabbitMqConnectionProvider, RabbitMqConnectionProvider>()
    .AddSingleton<IRpcMessageSerializer<SendNotificationResponse, SendNotificationRequest>, EmailSerializer>()
    .AddSingleton<IMailService, MailService>()
    .AddSingleton<IRpcServerHandler<SendNotificationRequest, SendNotificationResponse>, SendOnEmailHandler>()
    .AddSingleton<IRpcServer, RpcServer<SendNotificationRequest, SendNotificationResponse>>();

builder.Services.AddHostedService<EmailNotificatorWorker>();

var host = builder.Build();
host.Run();