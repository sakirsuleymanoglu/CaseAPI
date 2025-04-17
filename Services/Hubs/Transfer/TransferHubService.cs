using CaseAPI.Abstractions.Hubs;
using CaseAPI.Hubs;
using CaseAPI.Hubs.Models;
using Microsoft.AspNetCore.SignalR;

namespace CaseAPI.Services.Hubs.Transfer;

public sealed class TransferHubService(IHubContext<TransferHub> hubContext) : ITransferHubService
{
    public async Task SendNewTransferMessageAsync(
        Guid toUserId,
        NewTransferMessage model)
    {
        KeyValuePair<Guid, string> connection = TransferHub.Connections.Where(x => x.Key == toUserId).SingleOrDefault();

        await hubContext.Clients.Client(connection.Value).SendAsync("NewTransfer",
            model);
    }
}
