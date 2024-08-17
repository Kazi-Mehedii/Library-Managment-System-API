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
        [HttpPost("OrderBook")]
        public IActionResult OrderBook(int userId, int bookId)
        {
            var userOrder = ContextDb.Orders.Count(o => o.UserId == userId && !o.Returned) < 3;
            if (userOrder)
            {
                ContextDb.Orders.Add(new()
                {
                    UserId = userId,
                    BookId = bookId,
                    OrderDate = DateTime.Now,
                    ReturnDate = null,
                    Returned = false,
                    FinePaid = 0


                });

                var book = ContextDb.Books.Find(bookId);
                if (book is not null)
                {
                    book.Ordered = true;
                }

                ContextDb.SaveChanges();
                return Ok("Ordered");
            }
            return Ok("Can not order");
        }

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

        [Authorize]
        [HttpGet("ReturnBook")]
        public IActionResult ReturnBook(int userId, int bookId, int fine)
        {
            var orders = ContextDb.Orders.SingleOrDefault(or => or.UserId == userId && or.BookId == bookId);

            if (orders is not null)
            {
                orders.Returned = true;
                orders.ReturnDate = DateTime.Now;
                orders.FinePaid = fine;

                var book = ContextDb.Books.Single(rb => rb.Id == orders.BookId);

                book.Ordered = false;

                ContextDb.SaveChanges();

                return Ok("returned");
            }

            return Ok("Not Returned");

        }
    }
}
