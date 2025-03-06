using TinProjektBackend.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinProjektBackend.Models.RequestModels;
using TinProjektBackend.Models.ResponseModels;
using TinProjektBackend.Services;

namespace TinProjektBackend.Controllers;
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    public IOrderService OrderService { get; set; }
    public IConfiguration Configuration { get; set; }

    public OrdersController(IOrderService orderService, IConfiguration configuration)
    {
        OrderService = orderService;
        Configuration = configuration;
    }

    [Authorize(Roles = "Admin, Normal_user")]
    [HttpPost]
    public async Task<IActionResult> PlaceOrder(OrderRequestModel requestModel)
    {
        try
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var accessToken = authorizationHeader.Substring("Bearer ".Length);
            var secret = Configuration["JWT:SecretKey"];

            var userLogin = PasswordHasher.GetUserIdFromAccessToken(accessToken, secret, Configuration );
            if (string.IsNullOrEmpty(userLogin))
            {
                return Unauthorized("Invaild user");
            }

            await OrderService.PlaceOrderAsync(requestModel, userLogin);
            return Ok("Order succsesfully made!");
        } catch (KeyNotFoundException e)
        {
            return NotFound( e.Message );
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { Message = "An unexpected error occurred.", Details = e.Message });
        }
    }

    [Authorize(Roles = "Admin, Normal_user")]
    [HttpGet]
    public async Task<IActionResult> GetUsersOrders()
    {
        try
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var accessToken = authorizationHeader.Substring("Bearer ".Length);
            var secret = Configuration["JWT:SecretKey"];

            var userLogin = PasswordHasher.GetUserIdFromAccessToken(accessToken, secret, Configuration);
            if (string.IsNullOrEmpty(userLogin))
            {
                return Unauthorized("Invaild user");
            }

            var res = await OrderService.GetUsersOrdersAsync(userLogin);
            return Ok(res.OrderedTrips);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { Message = "An unexpected error occurred.", Details = e.Message });
        }
    }
}