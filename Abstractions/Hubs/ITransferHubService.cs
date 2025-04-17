using CaseAPI.Hubs.Models;

namespace CaseAPI.Abstractions.Hubs;

public interface ITransferHubService
{
    Task SendNewTransferMessageAsync(
       Guid toUserId,
       NewTransferMessage model);
}
