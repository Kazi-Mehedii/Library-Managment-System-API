using Library_managment_system_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


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
        public IActionResult Login(string email, string password)
        {
            if (ContextDb.Users.Any(us => us.Email.Equals(email) && us.Password.Equals(password)))
            {
                var user = ContextDb.Users.Single(user => user.Email.Equals(email) && user.Password.Equals(password));

                if (user.AccountStatus == AccountStatus.UNAPROVED)
                {
                    return Ok("Not approved");
                }

                if (user.AccountStatus == AccountStatus.BLOCKED)
                {
                    return Ok("blocked");
                }

                return Ok(JWTServices.GenerateToken(user));
            }
            return Ok("not found");

        }
        [Authorize]
        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            var returnsUser = ContextDb.Users.ToList();
            return Ok(returnsUser);
        }

        [Authorize]
        [HttpGet("ApproveRequest")]
        public IActionResult ApproveRequest(int userId)
        {
            var user = ContextDb.Users.Find(userId);

            if (user is not null)
            {
                if (user.AccountStatus == AccountStatus.UNAPROVED)
                {
                    user.AccountStatus = AccountStatus.ACTIVE;
                    ContextDb.SaveChanges();
                }

                EmailService.SendingMail
                    (user.Email, "Account Approved", $""" 
                <html>
                  <body>
                     <h2>
                      Hi, {user.FirstName} {user.LastName}
                     </h2>

                     <h3>
                     Your Account Has been approoved by Admin. You are ready for Login.
                     </h3>
                  </body>
                </html>
                """);
                return Ok("Approved");
            }
            return Ok("not approved");
        }

        //[Authorize]
        //[HttpGet("SendEamilForPendingReturn")]
        //public IActionResult SendEamilForPendingReturn()
        //{
        //    var orders = ContextDb.Orders.Include(o => o.Book).Include(o => o.User).ToList();

        //    var emailwithFine = orders.Where(o => DateTime.Now > o.OrderDate.AddDays(10)).ToList();

        //    emailwithFine.ForEach(o => o.FinePaid = (DateTime.Now - o.OrderDate.AddDays(10)).Days * 50);

        //    var firstFine = emailwithFine.Where(x => x.FinePaid == 50).ToList();
        //    firstFine.ForEach(first =>
        //    {
        //        var body = $"""
        //        <html>
        //            <body>
        //            <h2>Hi, {first.User?.FirstName}{first.User?.LastName}</h2>
        //            <h4> Yesterday was your last day to retrurn Book: "{first.Book?.Title}."</h4>
        //            <h4> From today, every day a fine of 5o tk will be added for this book.</h4>
        //            <h4>If your fine exceeds 500 tk, your account will be blocked.</h4>
        //            <h4>Thanks</h4>
        //            </body>
        //        </html>
        //        """;

        //        EmailService.SendingMail(first.User!.Email, "Return Due", body);
        //    });


        //    var regularFineEmails = emailwithFine.Where(reg => reg.FinePaid > 50 && reg.FinePaid <= 500).ToList();
        //    regularFineEmails.ForEach(reg =>
        //    {
        //        var regularBody = $"""

        //        <html>
        //            <body>
        //                 <h2>Hi, {reg.User?.FirstName}{reg.User?.LastName}</h2>

        //                 <h4>You Have {reg.FinePaid} tk fine on book "{reg.Book?.Title}"</h4>

        //                <h4>Plaese pay and return book as soon as possible.</h4>
        //                 <h4>Thanks</h4>

        //            </body>
        //        </html>

        //        """;

        //        EmailService.SendingMail(reg.User!.Email, "Fine to Pay", regularBody);
        //    });


        //    var overDueFine = emailwithFine.Where(ovr => ovr.FinePaid > 500).ToList();
        //    overDueFine.ForEach(ovr =>
        //    {
        //        var overDueBody = $"""

        //        <h2>Hi, {ovr.User?.FirstName}{ovr.User?.LastName}</h2>

        //        <h4>You Have {ovr.FinePaid} tk fine on Book: {ovr.Book?.Title}</h4>

        //        <h4>Your account is BLOCKED. Please pay it as soon as possible to UNBLOCK your Account</h4>

        //        <h4>Thanks</h4>

        //        """;

        //        EmailService.SendingMail(ovr.User!.Email, "Fine Over Due", overDueBody);
        //    });

        //    return Ok("sent");



        //}

        [Authorize]
        [HttpGet("SendEamilForPendingReturn")]
        public IActionResult SendEamilForPendingReturn()
        {
            // Filter only pending returns (i.e., books not yet returned)
            var pendingOrders = ContextDb.Orders.Include(o => o.Book).Include(o => o.User)
                                .Where(o => !o.Returned) // Ensure only non-returned orders are processed
                                .ToList();

            var emailwithFine = pendingOrders.Where(o => DateTime.Now > o.OrderDate.AddDays(10)).ToList();

            emailwithFine.ForEach(o => o.FinePaid = (DateTime.Now - o.OrderDate.AddDays(10)).Days * 50);

            var firstFine = emailwithFine.Where(x => x.FinePaid == 50).ToList();
            firstFine.ForEach(first =>
            {
                var body = $"""
        <html>
            <body>
            <h2>Hi, {first.User?.FirstName}{first.User?.LastName}</h2>
            <h4> Yesterday was your last day to return Book: "{first.Book?.Title}."</h4>
            <h4> From today, every day a fine of 50 tk will be added for this book.</h4>
            <h4>If your fine exceeds 500 tk, your account will be blocked.</h4>
            <h4>Thanks</h4>
            </body>
        </html>
        """;

                EmailService.SendingMail(first.User!.Email, "Return Due", body);
            });


            var regularFineEmails = emailwithFine.Where(reg => reg.FinePaid > 50 && reg.FinePaid <= 500).ToList();
            regularFineEmails.ForEach(reg =>
            {
                var regularBody = $"""

        <html>
            <body>
                 <h2>Hi, {reg.User?.FirstName}{reg.User?.LastName}</h2>

                 <h4>You Have {reg.FinePaid} tk fine on book "{reg.Book?.Title}"</h4>

                <h4>Please pay and return the book as soon as possible.</h4>
                 <h4>Thanks</h4>
                 
            </body>
        </html>

        """;

                EmailService.SendingMail(reg.User!.Email, "Fine to Pay", regularBody);
            });


            var overDueFine = emailwithFine.Where(ovr => ovr.FinePaid > 500).ToList();
            overDueFine.ForEach(ovr =>
            {
                var overDueBody = $"""
            
        <h2>Hi, {ovr.User?.FirstName}{ovr.User?.LastName}</h2>

        <h4>You Have {ovr.FinePaid} tk fine on Book: {ovr.Book?.Title}</h4>

        <h4>Your account is BLOCKED. Please pay it as soon as possible to UNBLOCK your Account</h4>

        <h4>Thanks</h4>

        """;

                EmailService.SendingMail(ovr.User!.Email, "Fine Over Due", overDueBody);
            });

            return Ok("sent");
        }


        [Authorize]
        [HttpGet("BlockForFineOverDue")]
        public IActionResult BlockForFineOverDue()
        {
            var blockeduser = ContextDb.Orders.Include(b => b.User).Include(b => b.Book).Where(b => !b.Returned).ToList();

            var emailwithFine = blockeduser.Where(o => DateTime.Now > o.OrderDate.AddDays(10)).ToList();

            emailwithFine.ForEach(o => o.FinePaid = (DateTime.Now - o.OrderDate.AddDays(10)).Days * 50);

            var user = emailwithFine.Where(b => b.FinePaid > 500).Select(b => b.User).Distinct().ToList();

            if (user is not null && user.Any())
            {
                foreach (var u in user)
                {
                    u.AccountStatus = AccountStatus.BLOCKED;
                }
                ContextDb.SaveChanges();
                return Ok("blocked");
            }
            else { return Ok("not blocked"); }
        }

        [Authorize]
        [HttpGet("UnblockUser")]
        public IActionResult UnblockUser(int userId)
        {
            var user = ContextDb.Users.Find(userId);
            if (user is not null)
            {
                user.AccountStatus = AccountStatus.ACTIVE;
                ContextDb.SaveChanges();
                return Ok("unblocked");
            }
            return Ok("not unblocked");
        }

    }
}

