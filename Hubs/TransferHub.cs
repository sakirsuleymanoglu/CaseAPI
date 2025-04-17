using CaseAPI.Abstractions.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CaseAPI.Hubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class TransferHub(IUserService userService) : Hub
{
    private readonly static Dictionary<Guid, string> _connections = [];

    public static IReadOnlyDictionary<Guid, string> Connections => _connections;

    public async override Task OnConnectedAsync()
    {
        _connections.Add(userService.GetAuthenticatedUserId(), Context.ConnectionId);
    }

    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        _connections.Remove(userService.GetAuthenticatedUserId());
    }
}
