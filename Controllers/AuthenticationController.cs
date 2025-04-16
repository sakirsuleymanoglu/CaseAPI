﻿using CaseAPI.Abstractions.Accounts;
using CaseAPI.Abstractions.Authentication;
using CaseAPI.Models.Accounts.Transactions;
using CaseAPI.Models.Authentication;
using CaseAPI.Models.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class AuthenticationController(
    IAuthenticationService authenticationService,
    IAccountTransactionService accountTransactionService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(Login model)
    {
        CreatedJwt jwt = await authenticationService.LoginAsync(model);
        return Ok(jwt);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("test-authorize")]
    public IActionResult TestAuthorize()
    {
        return Ok(new
        {
            Message = "You are authorized to access this endpoint.",
            UserName = User.Identity?.Name,
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        });
    }

    [HttpPost("create-deposit")]
    public async Task<IActionResult> CreateDeposit(CreateDeposit model)
    {
        await accountTransactionService.CreateDepositAsync(model);
        return Ok();
    }

    [HttpPost("create-each-other-deposit")]
    public async Task<IActionResult> CreateEacherOtherDeposit(CreateEachOtherDeposit model)
    {
        await accountTransactionService.CreateEachOtherDepositAsync(model);
        return Ok();
    }

    [HttpPost("create-withdrawal")]
    public async Task<IActionResult> CreateWithdrawal(CreateWithdrawal model)
    {
        await accountTransactionService.CreateWithdrawalAsync(model);
        return Ok();
    }
}
