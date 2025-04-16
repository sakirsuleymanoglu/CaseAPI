using CaseAPI.Abstractions.Accounts;
using CaseAPI.Data;
using CaseAPI.Models.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController(
    ApplicationDbContext context,
    IAccountService accountService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? appUserId = null)
    {
        var query = context.Accounts
            .AsNoTrackingWithIdentityResolution();

        if (!string.IsNullOrEmpty(appUserId))
            query = query.Where(x => x.AppUserId == Guid.Parse(appUserId));

        var result = await query
            .Select(x => new
            {
                x.Title,
                x.Balance,
                x.AppUserId,
                x.Id,
                x.Code,
            }).ToListAsync();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAccount model)
    {
        await accountService.CreateAsync(model);
        return Ok();
    }

    [HttpGet("{id}/transactions")]
    public async Task<IActionResult> Transactions(string id)
    {
        var result = await context.AccountTransactions.Where(x => x.AccountId == Guid.Parse(id))
            .AsNoTrackingWithIdentityResolution()
            .Select(x => new
            {
                Channel = x.Channel.ToString(),
                x.Balance,
                x.UpdatedBalance,
                x.CreatedDate,
                x.Id,
                x.AccountId,
                x.Amount,
                Type = x.Type.ToString(),
                x.Description,
            }).OrderByDescending(x => x.CreatedDate).ToListAsync();

        return Ok(result);
    }
}
