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

        [Authorize]
        [HttpGet("GetCategory")]
        public IActionResult GetCategory()
        {
            var category = Context.BookCategories.ToList();
            if (category.Any())
            {
                return Ok(category);
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost("AddNewBook")]
        public IActionResult AddNewBook(Book book)
        {
            book.BookCategory = null;
            Context.Books.Add(book);
            Context.SaveChanges();
            return Ok("inserted");
        }

        [Authorize]
        [HttpDelete("DeleteBook")]
        public IActionResult DeleteBook(int id)
        {
            var exists = Context.Books.Any(bc => bc.Id == id);
            if(exists)
            {
                var book = Context.Books.Find(id);
                Context.Books.Remove(book!);
                Context.SaveChanges();
                return Ok("deleted");
            }
            return NotFound();

        }
    }
}
