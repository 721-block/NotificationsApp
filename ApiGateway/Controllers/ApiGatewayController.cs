using ApiGateway.Models;
using ApiGateway.Models.Responses;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Mvc;
using RabbitMqModule.RpcClient;

namespace ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiGatewayController(
    Func<NotificationBase, IRpcClient<NotificationRequest, NotificationResponse>> rpcClientFactory,
    INotificationStatusRepository notificationStatusRepository)
    : ControllerBase
{
    [HttpPost(Name = "PostNotificationMessage")]
    public async Task<ActionResult<NotificationCreatedResponse>> Post([FromBody] NotificationBase notificationBase)
    {
        var notificationId = Guid.NewGuid();
        rpcClientFactory(notificationBase).Call(new NotificationRequest
        {
            Id = notificationId,
            RetryCount = 0, // Get retry number from settings
            Recipient = notificationBase.Recipient,
            Subject = notificationBase.Subject,
            Body = notificationBase.Body,
            Metadata = notificationBase.Metadata,
        });
        
        await notificationStatusRepository.UpdateNotificationStatus(notificationId, NotificationStatus.Pending);

        return new NotificationCreatedResponse
        {
            Id = notificationId,
        };
    }

    [HttpGet("{id}", Name = "GetNotificationStatus")]
    public async Task<ActionResult<NotificationStatusResponse>> GetNotificationStatus(Guid id)
    {
        var notificationStatus = await notificationStatusRepository.GetById(id);

        if (notificationStatus is null)
            return NotFound();

        return new NotificationStatusResponse
        {
            Id = notificationStatus.Id,
            Status = notificationStatus.NotificationStatus
        };
    }
}