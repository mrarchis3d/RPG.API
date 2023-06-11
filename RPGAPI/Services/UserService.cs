using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using RPGAPI.Domain.UserAggregate;
using RPGAPI.Dtos.Requests;
using RPGAPI.Dtos.Responses;
using RPGAPI.Infrastructure;
using RPGAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RPGAPI.Services;

public class UserService : IUserService
{
    private readonly ServiceDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public UserService(ServiceDbContext dbContext, IConfiguration configuration, IMapper mapper)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _mapper = mapper;
    }

    public UserDto AddUser(CreateUserDto user)
    {
        byte[] salt = GenerateRandomSalt();
        byte[] passwordHash = CalculateHashPassword(user.Password, salt);
        var userEntity = new User { Email= user.Email, PasswordHash=passwordHash, Salt = salt, FullName= user.FullName, UserName = user.UserName };
        _dbContext.Users.Add(userEntity);
        _dbContext.SaveChanges();

        return _mapper.Map<UserDto>(userEntity);
        
    }

    public bool ValidateUserCredentials(UserCredentialsDto credentials)
    {
        return VerifyPassword(credentials.UserName, credentials.Password);
    }

    private bool VerifyPassword(string userName, string password)
    {
        var usuario = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);
        if (usuario == null)
            return false;

        byte[] contraseñaHashCalculado = CalculateHashPassword(password, usuario.Salt);

        return contraseñaHashCalculado.SequenceEqual(usuario.PasswordHash);
    }

    public string GenerateAccessToken(string username)
    {
        // Obtener la clave secreta para firmar el token desde la configuración de la aplicación
        string secretKey = _configuration["Jwt:SecretKey"];
        byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);

        // Crear los claims (datos) del token
        var claims = new[]
        {
                new Claim(ClaimTypes.Name, username)
                // Agrega cualquier otro claim que desees incluir en el token, como roles, permisos, etc.
            };

        // Generar el token de acceso utilizando JWT (JSON Web Tokens)
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"])),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256)
        );

        // Serializar el token a su representación en formato string
        string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return accessToken;
    }

    private byte[] GenerateRandomSalt()
    {
        byte[] sal = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(sal);
        }
        return sal;
    }

    private byte[] CalculateHashPassword(string contraseña, byte[] sal)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(contraseña, sal, 10000))
        {
            return pbkdf2.GetBytes(20); // Tamaño del hash de contraseña (20 bytes para SHA-1)
        }
    }
}
