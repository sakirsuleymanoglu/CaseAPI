using CaseAPI.Abstractions.Accounts;
using CaseAPI.Models.Accounts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class AccountsController(
    IAccountService accountService,
    IAccountTransactionService accountTransactionService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await accountService.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create(CreateAccount model)
    {
        await accountService.CreateAsync(model);
        return Ok();
    }

    [HttpGet("{id}/transactions")]
    public async Task<IActionResult> Transactions(string id) => Ok(await accountTransactionService.GetAllAsync(id));
}
