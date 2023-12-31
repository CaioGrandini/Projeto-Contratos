﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web.Api.Contratos.Extensions;
using Web.Api.Contratos.Interfaces;
using Web.Api.Contratos.ViewModel;

namespace Web.Api.Contratos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : MainController
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        public AuthController(INotificador notificador,
                                 SignInManager<IdentityUser> signInManager,
                                 UserManager<IdentityUser> userManager, 
                                 IUser appUser, IOptions<AppSettings> appSettings) : base(notificador, appUser)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }
        
        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar([FromBody] RegisterUserViewModel registerUser)
        {
            //Validamos a modelState
            if(!ModelState.IsValid) return CustomResponse(ModelState);

            //Setamos a indentificacao no banco.
            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            //criamos o usuario e retornamos;
            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                // O mesmo nao é persistente
                await _signInManager.SignInAsync(user, isPersistent: false);
                //geramos o JWT
                return CustomResponse(await GerarJwt(registerUser.Email));
            }
            foreach(var error in result.Errors)
            {
                NotificarErro(error.Description);
            }
            return CustomResponse(registerUser);
        }

        [HttpPost("entrar")]
        public async Task<ActionResult> Login([FromBody] LoginUserViewModel loginUser)
        {    
            //Validamos a modelState
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);
            if (result.Succeeded)
            {
                return CustomResponse(await GerarJwt(loginUser.Email));
            }
            //if result.IsLockedOut > 5, o usuario é bloqueado temporariamente.
            if (result.IsLockedOut)
            {
                NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse(loginUser);
            }
            NotificarErro("Usuário ou senha incorreta");
            return CustomResponse(loginUser);
        }

        //Será gerado o token e retornado para o usuario.
        private async Task<LoginResponseViewModel> GerarJwt(string email)
        { 
            var user = await _userManager.FindByNameAsync(email);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                //Retornamos o Email dentro do Token junto com algumas outras informações.
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Email, email)
                    }),
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            //Logamos o user
            var encondedToken = tokenHandler.WriteToken(token);
            var response = new LoginResponseViewModel
            {
                AccessToken = encondedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UserToken = new UserTokenViewModel
                {
                    ID = user.Id,
                    Email = user.Email
                }
            };

            return response;
        }
    }
}

