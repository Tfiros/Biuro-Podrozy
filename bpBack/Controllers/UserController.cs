using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TinProjektBackend.Models;
using TinProjektBackend.Models.RequestModels;
using TinProjektBackend.Models.ResponseModels;
using TinProjektBackend.Services;

namespace TinProjektBackend.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel request)
    {
        try
        {
            await _userService.RegisterUserAsync(request);
            return Ok("User registered successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestModel request)
    {
        try
        {
            var response = await _userService.LoginUserAsync(request);
            return Ok(response);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Unauthorized(e.Message);
        }catch (Exception e)
        {
            return StatusCode(500, "Error while proceeding: " +e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestModel refToken)
    {
        try
        {
            var res = await _userService.RefreshUserTokenAsync(refToken);
            return Ok(res);
        }
        catch (SecurityTokenException e)
        {
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error while proceeding: " + e.Message);
        }
    }
}




