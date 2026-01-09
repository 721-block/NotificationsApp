using Core.Models;
using DAL;

namespace ApiGateway.Models.Responses;

public class NotificationStatusResponse
{
    public Guid Id { get; set; }
    public NotificationStatus Status { get; set; }
}