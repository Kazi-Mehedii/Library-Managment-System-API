using Library_managment_system_API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_managment_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(ContextDb contextDb)
        {
            ContextDb = contextDb;
        }
        public ContextDb ContextDb { get; }

        [HttpPost("Register")]
        public IActionResult Register(User user)
        {
            user.AccountStatus = AccountStatus.UNAPROVED;
            user.UserType = UserType.STUDENT;
            user.CreatedOn = DateTime.Now;
            

            ContextDb.Users.Add(user);
            ContextDb.SaveChanges();

            return Ok(@"Thanks For Register. Wait For Admin Aproval. After Its Aproved you will get an email.");


        }

    }
}
