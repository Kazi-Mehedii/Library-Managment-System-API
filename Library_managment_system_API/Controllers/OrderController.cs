using Library_managment_system_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_managment_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public OrderController(ContextDb contextDb)
        {
            ContextDb = contextDb;
        }

        public ContextDb ContextDb { get; }

        [Authorize]
        [HttpGet("GetOrderOfUser")]
        public IActionResult GetOrderOfUser(int userId)
        {
            var orders = ContextDb.Orders
                .Include(o => o.Book)
                .Include(o => o.User)
                .Where(o => o.UserId == userId).ToList();

            if(orders.Any())
            {
                return Ok(orders);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
