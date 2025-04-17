using CaseAPI.Abstractions.Authentication;
using CaseAPI.Models.Authentication;
using CaseAPI.Models.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace CaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class AuthenticationController(
    IAuthenticationService authenticationService
   ) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(Login model)
    {
        CreatedJwt jwt = await authenticationService.LoginAsync(model);
        return Ok(jwt);
    }

    //[HttpPost("create-deposit")]
    //public async Task<IActionResult> CreateDeposit(CreateDeposit model)
    //{
    //    await accountTransactionService.CreateDepositAsync(model);
    //    return Ok();
    //}

    //[HttpPost("create-each-other-deposit")]
    //public async Task<IActionResult> CreateEacherOtherDeposit(CreateEachOtherDeposit model)
    //{
    //    await accountTransactionService.CreateEachOtherDepositAsync(model);
    //    return Ok();
    //}

    //[HttpPost("create-withdrawal")]
    //public async Task<IActionResult> CreateWithdrawal(CreateWithdrawal model)
    //{
    //    await accountTransactionService.CreateWithdrawalAsync(model);
    //    return Ok();
    //}
}
