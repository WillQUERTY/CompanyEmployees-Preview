namespace PreviewApi.Controllers;

using PreviewApi.Application.Common;
using PreviewApi.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// API Controller para autenticación
/// Demuestra el patrón de JWT tokens
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IJwtTokenService jwtTokenService, ILogger<AuthController> logger)
    {
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    /// <summary>
    /// Login - obtener JWT token
    /// [DEMO] Con fines de demostración, aceptamos cualquier email
    /// </summary>
    [HttpPost("login")]
    public ActionResult<LoginResponse> Login(LoginRequest request)
    {
        _logger.LogInformation("Login attempt for: {Email}", request.Email);

        // [DEMO] Simulamos autenticación (en producción validar contra DB)
        var userId = request.Email.GetHashCode();
        var token = _jwtTokenService.GenerateToken(userId, request.Email, "User");

        return Ok(new LoginResponse
        {
            UserId = userId,
            Email = request.Email,
            Token = token
        });
    }
}
