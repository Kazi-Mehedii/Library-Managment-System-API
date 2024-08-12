using Library_managment_system_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_managment_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        public BookController(ContextDb contextDb) 
        {
           Context = contextDb;
        }

        public ContextDb Context { get; }

        [Authorize]
        [HttpGet("GetBooks")]
        public IActionResult GetBooks()
        {
            if (Context.Books.Any())
            {
                return Ok(Context.Books.Include(a => a.BookCategory).ToList());
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost("OrderBook")]
        public IActionResult OrderBook(int userId, int bookId)
        {
            var userOrder = Context.Orders.Count(o => o.UserId == userId && !o.Returned) < 3;
            if (userOrder)
            {
                Context.Orders.Add(new()
                {
                    UserId = userId,
                    BookId = bookId,
                    OrderDate = DateTime.Now,
                    ReturnDate = null,
                    Returned = false,
                    FinePaid = 0


                });

                var book = Context.Books.Find(bookId);
                if (book is not null)
                {
                    book.Ordered = true;
                }

                Context.SaveChanges();
                return Ok("Ordered");
            }
            return Ok("Can not order");
        }

        [Authorize]
        [HttpPost("AddCategory")]
        public IActionResult AddCategory(BookCategory bookCategory)
        {
            var exists = Context.BookCategories.Any(bcat => bcat.Category == bookCategory.Category && bcat.SubCategory == bookCategory.SubCategory);
            if(exists)
            {
                return Ok("cannot insert");
            }
            else
            {
                Context.BookCategories.Add(new()
                {
                    Category = bookCategory.Category,
                    SubCategory = bookCategory.SubCategory
                });
                Context.SaveChanges();
                return Ok("Inserted");
            }
        }
    }
}
