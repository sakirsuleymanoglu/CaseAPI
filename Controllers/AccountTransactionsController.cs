using CaseAPI.Abstractions.Accounts;
using CaseAPI.Models.Accounts.Transactions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class AccountTransactionsController(
    IAccountTransactionService accountTransactionService
    ) : ControllerBase
{
    [HttpPost("create-transfer")]
    public async Task<IActionResult> CreateTransfer(CreateTransfer model)
    {
        await accountTransactionService.CreateTransferAsync(model);
        return Ok();
    }
}
