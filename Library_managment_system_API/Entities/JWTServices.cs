using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library_managment_system_API.Entities
{
    public class JWTServices
    {
        private readonly string key = string.Empty;
        private readonly int duration;

        public JWTServices(IConfiguration configuration)
        {
            key = configuration["Jwt:Key"]!;
            duration = int.Parse(configuration["Jwt:Duration"]!);
        }

        public string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.key));
            var signinkey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("id",user.Id.ToString()),
                new Claim("firstName",user.FirstName),
                new Claim("lastName",user.LastName),
                new Claim("email",user.Email),
                new Claim("mobileNumber",user.MobileNumber),
                new Claim("userType",user.UserType.ToString()),
                new Claim("accountStatus",user.AccountStatus.ToString()),
                new Claim("createdOn", user.CreatedOn.ToString()),
            };

            var jwtToken = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(this.duration),
                signinkey
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);

        }
    }
}
