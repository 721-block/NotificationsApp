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
    .AddScopedSettings<MailSettings>(config, "MailSettings")
    .AddSingleton<IRabbitMqConnectionProvider, RabbitMqConnectionProvider>()
    .AddSingleton<IRpcMessageSerializer<SendMailNotificationResponse, SendMailNotificationRequest>, EmailSerializer>()
    .AddScoped<IMailService, MailService>()
    .AddSingleton<IRpcServerHandler<SendMailNotificationRequest, SendMailNotificationResponse>, SendOnEmailHandler>()
    .AddSingleton<IRpcServer, RpcServer<SendMailNotificationRequest, SendMailNotificationResponse>>();

builder.Services.AddHostedService<EmailNotificatorWorker>();

var host = builder.Build();
host.Run();