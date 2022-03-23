//using Microsoft.AspNetCore.Mvc;
//using Snap.Dto;
//using Snap.Models;
//using System.Configuration;
//using System.Net.Http;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Web;
//using Snap.Services;

//namespace Snap.Controllers
//{
//    [Route("api/auth")]
//    [ApiController]
//    public class AuthController : Controller
//    {
//        [HttpPost]
//        [Route("login")]
//        [AllowAnonymous]
//        public IActionResult Login([FromBody] UserLoginDto loginDto)
//        {
//            IActionResult ret;

//            try
//            {
//#pragma warning disable CS8602 // Dereference of a possibly null reference.
//                loginDto.IPAddress = HttpContext.Connection.RemoteIpAddress.ToString();
//#pragma warning restore CS8602 // Dereference of a possibly null reference.
//                SessionDto session = UserManagementService.Authenticate(loginDto);
//                if (session == null)
//                    return Ok(false);

//                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
//                    1,
//                    session.User.UserName,
//                    session.StartDate,
//                    session.ExpireDate,
//                    true,
//                    session.Id.ToString());
//                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
//                //cookie.Expires = session.ExpireDate.AddDays(2);
//                HttpContext.Response.Cookies.Add(cookie);
//                return Ok(true); //TO-DO:  Add redirect to landing in javascript
//            }
//            catch (Exception ex)
//            {
//                return Ok(false);
//            }

//            // Return OK for now
//            ret = Ok(true);

//            return ret;
//        }

//        [HttpPost]
//        [Route("register")]
//        public IActionResult Register([FromBody] Registration value)
//        {
//            IActionResult ret;

//            // TODO: Register Logic

//            // Return OK for now
//            ret = Ok(true);

//            return ret;

//        }
//    }
//}
