﻿using Library_managment_system_API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Library_managment_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(ContextDb contextDb, EmailService emailService, JWTServices jWTServices)
        {
            ContextDb = contextDb;
            EmailService = emailService;
            JWTServices = jWTServices;
        }
        public ContextDb ContextDb { get; }
        public EmailService EmailService { get; }
        public JWTServices JWTServices { get; }

        [HttpPost("Register")]
        public IActionResult Register(User user)
        {
            user.AccountStatus = AccountStatus.UNAPROVED;
            user.UserType = UserType.STUDENT;
            user.CreatedOn = DateTime.Now;
            

            ContextDb.Users.Add(user);
            ContextDb.SaveChanges();

            #region for configuring mail
            //for configure with mail
            const string subject = "Account Created";
            var body = $"""
            <html>
                <body>
                    <h1>Hello, {user.FirstName} {user.LastName}</h1>
                    <h2>
                        Your Account has been created and Approval Request Already sent to Admin.
                        If Admin approved the request, you will receive email, and you will be able to login in to your account.
                    </h2>
                    <h3>Good Luck</h3>
                </body>
            </html>
            """;

            EmailService.SendingMail(user.Email, subject, body);
            #endregion

            return Ok(@"Thanks For Register. Wait For Admin Aproval. After Its Aproved you will get an email.");


        }


        [HttpGet("Login")]
        public IActionResult Login(string email, string password )
        {
            if (ContextDb.Users.Any(us => us.Email.Equals(email) && us.Password.Equals(password)))
            {
                var user = ContextDb.Users.Single(user => user.Email.Equals(email) && user.Password.Equals(password));

                if (user.AccountStatus == AccountStatus.UNAPROVED)
                {
                    return Ok("Not approved");
                }
                return Ok(JWTServices.GenerateToken(user));
            
            }
            return Ok("not found");
            
        }



    }
}
