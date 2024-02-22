using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RegionsUser.Models.Domains;
using RegionsUser.Models.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using User.Data;
using User.Models.Domains;
using User.Models.Dto;
using System.Security.Cryptography;

namespace RegionsUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly UsersDbContext usersDbContext;
        private readonly IConfiguration configuration;

        public UserLoginController(UsersDbContext usersDbContext, IConfiguration configuration)
        {
            this.usersDbContext = usersDbContext;
            this.configuration = configuration;
        }
        // GET
        [HttpGet]
        public IActionResult GetAll()
        {
            var clients = usersDbContext.Clients.ToList();
            return Ok(clients);
        }

        /*[HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var clients = await usersDbContext.Clients.FindAsync();

            return Ok(clients);

        }*/

        /* [HttpPost]
         public IActionResult Create([FromBody] AddUsersDto addUsersDto)
         {
             if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }
            // Check if the provided RegionalId exists in the regions table
            /* var region = usersDbContext.Regions.FirstOrDefault(r => r.Id == addUsersDto.RegionalId);
             if (region == null)
             {
                 // Return a 404 Not Found response if the RegionalId is invalid
                 return NotFound("The specified RegionalId does not exist.");
             }

             // Create a new User entity
             var user = new Client
             {
                 Id = Guid.NewGuid(),
                 UserName = addUsersDto.UserName,
                 Password = addUsersDto.Password,
                 Email = addUsersDto.Email,
             //RegionalId = addUsersDto.RegionalId // Assign regionsId from the DTO
             };

             // Add the user to the DbContext and save changes
             usersDbContext.Clients.Add(user);
             usersDbContext.SaveChanges();

             // Return 201 Created response with the newly created user
             return CreatedAtAction(nameof(GetById), new { Id = user.Id }, user);
         }
         [HttpGet]
         [Route("{UserId:Guid}")]
         public async Task<IActionResult> GetById(Guid UserId)
         {
             var users =await usersDbContext.Clients.FindAsync(UserId);
             if (users == null)
             {
                 return NotFound();
             }
             return Ok(users);
         }*/

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] AddUsersDto addUsersDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var client = await usersDbContext.Clients.FirstOrDefaultAsync(c => c.Email == addUsersDto.Email);
            if (client != null)
            {
                return BadRequest("User already exists");
            }
           // CreatepasswordHash(addUsersDto.Password, out byte[] passwordHash
            //    , out byte[] passwordsalt);




            var clientDomainModel = new Client
            {
                UserName = addUsersDto.UserName,
                Email = addUsersDto.Email,
                Password = addUsersDto.Password
            };

            usersDbContext.Clients.Add(clientDomainModel);
            await usersDbContext.SaveChangesAsync();

            var clientDto = new ClientDto   
            {
                Id = clientDomainModel.Id,
                UserName = clientDomainModel.UserName,
                Email = clientDomainModel.Email,
                Password = clientDomainModel.Password
            };

            return Ok("User created");
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = usersDbContext.Clients.FirstOrDefault(c => c.UserName == loginDto.UserName && c.Password == loginDto.Password);

            if (client == null)
            {
                return NotFound("Invalid username or password.");
            }

            return Ok(client);
        }

        // GET
        [HttpGet("{Id:Guid}")]

        public IActionResult GetById(Guid Id)
        {
            var client = usersDbContext.Clients.Find(Id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }
        // POST
        
        /*
        [HttpPost("login2")]
        public IActionResult Login2([FromBody] LoginDto loginDto)
        {
            var user = AuthenticateUser(loginDto.UserName, loginDto.Password);

            if (user == null)
                return Unauthorized(); // Return 401 Unauthorized if authentication fails

            var token = GenerateJwtToken(user);

            return Ok(new { Token = token }); // Return token as response
        }
        private Client AuthenticateUser(string username, string password)
        {
            // Retrieve user from the database based on the provided username
            var user = usersDbContext.Clients.FirstOrDefault(u => u.UserName == username);

            // Validate password (pseudo-code, actual implementation depends on your password hashing)
            if (user != null && user.Password == password)
            {
                return user; // Return authenticated user
            }

            return null; // Return null if authentication fails
        }
        private string GenerateJwtToken(Client user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
                    // Add more claims as needed (e.g., roles, permissions)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }*/








        // PUT: api/Clients/{UserId
        [HttpPut("{ClientId:Guid}")]
        public IActionResult Update(Guid ClientId, [FromBody] AddUsersDto updateClientDto)
        {
            var client = usersDbContext.Clients.Find(ClientId);
            if (client == null)
            {
                return NotFound();
            }

            // Update properties of the existing client entity
            client.UserName = updateClientDto.UserName;
            client.Email = updateClientDto.Email;
            client.Password = updateClientDto.Password;

            // Save the changes to the database
            usersDbContext.SaveChanges();

            // Return the updated client DTO
            var updatedClientDto = new ClientDto
            {
                Id = client.Id,
                UserName = client.UserName,
                Email = client.Email,
                Password = client.Password
            };

            // Return 200 OK response with the updated client DTO
            return Ok(updatedClientDto);
        }

        // DELETE: api/Clients/{UserId}
        [HttpDelete("{ClientId:Guid}")]
        public IActionResult DeleteById(Guid ClientId)
        {
            var client = usersDbContext.Clients.Find(ClientId);
            if (client == null)
            {
                return NotFound();
            }

            usersDbContext.Clients.Remove(client);
            usersDbContext.SaveChanges();

            return NoContent();
        }
        private void CreatepasswordHash(string password,out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash= hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}

