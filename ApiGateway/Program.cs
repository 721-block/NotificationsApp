using ApiGateway.Extensions;
using ApiGateway.Services;
using DAL;
using EmailNotificator.Models;
using EmailNotificator.Serializers;
using Microsoft.EntityFrameworkCore;
using PushNotificator.Models;
using PushNotificator.Serializers;
using RabbitMqModule.Common;
using RabbitMqModule.RpcClient;
using RabbitMqModule.RpcServer;
using SmsNotificator.Models;
using SmsNotificator.Serializers;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddDbContext<NotificationStatusDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<INotificationStatusRepository, NotificationStatusRepository>();
builder.Services.AddSingletonSettings<RpcServerSettings>(config, "RpcServerSettings")
    .AddSingletonSettings<RabbitMqSettings>(config, "RabbitMqSettings")
    .AddSingleton<IRabbitMqConnectionProvider, RabbitMqConnectionProvider>()
    
    .AddSingleton<IRpcMessageSerializer<SendSmsNotificationRequest, SendSmsNotificationResponse>, SmsSerializer>()
    .AddSingleton<IRpcClient<SendSmsNotificationRequest, SendSmsNotificationResponse>, RpcClient<SendSmsNotificationRequest, SendSmsNotificationResponse>>()
    
    .AddSingleton<IRpcMessageSerializer<SendPushNotificationRequest, SendPushNotificationResponse>, PushSerializer>()
    .AddSingleton<IRpcClient<SendPushNotificationRequest, SendPushNotificationResponse>, RpcClient<SendPushNotificationRequest, SendPushNotificationResponse>>()
    
    .AddSingleton<IRpcMessageSerializer<SendMailNotificationRequest, SendMailNotificationResponse>, EmailSerializer>()
    .AddSingleton<IRpcClient<SendMailNotificationRequest, SendMailNotificationResponse>, RpcClient<SendMailNotificationRequest, SendMailNotificationResponse>>();

services.AddHostedService<NotificationProviderWorker<SendSmsNotificationRequest, SendSmsNotificationResponse>>();
services.AddHostedService<NotificationProviderWorker<SendPushNotificationRequest, SendPushNotificationResponse>>();
services.AddHostedService<NotificationProviderWorker<SendMailNotificationRequest, SendMailNotificationResponse>>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();