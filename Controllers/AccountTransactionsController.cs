using CaseAPI.Abstractions.Accounts;
using CaseAPI.Models.Accounts.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace CaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
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
