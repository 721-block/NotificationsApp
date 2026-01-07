using RabbitMqModule.Common;
using RabbitMqModule.RpcServer;
using SmsNotificator;
using SmsNotificator.Extensions;
using SmsNotificator.Handlers;
using SmsNotificator.Models;
using SmsNotificator.Serializers;
using SmsNotificator.Sms;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddSingletonSettings<RpcServerSettings>(config, "RpcServerSettings")
    .AddSingletonSettings<RabbitMqSettings>(config, "RabbitMqSettings")
    .AddScopedSettings<MainSmsSettings>(config, "MainSmsSettings")
    .AddSingleton<ISmsMessageProvider, SmsMessageProvider>()
    .AddSingleton<IRabbitMqConnectionProvider, RabbitMqConnectionProvider>()
    .AddSingleton<IRpcMessageSerializer<SendSmsNotificationResponse, SendSmsNotificationRequest>, SmsSerializer>()
    .AddScoped<ISmsService, SmsService>()
    .AddSingleton<IRpcServerHandler<SendSmsNotificationRequest, SendSmsNotificationResponse>, SendSmsHandler>()
    .AddSingleton<IRpcServer, RpcServer<SendSmsNotificationRequest, SendSmsNotificationResponse>>();

builder.Services.AddHostedService<SmsNotificatorWorker>();

var host = builder.Build();
host.Run();