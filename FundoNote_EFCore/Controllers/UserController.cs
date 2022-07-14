using BusinessLayer.Interface;
using DatabaseLayer.UserModels;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Services;
using System;

namespace FundoNote_EFCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private FundoContext fundoContext;
        private IUserBL userBL; 
        public UserController(FundoContext fundoContext,IUserBL userBL)
        {
            this.fundoContext = fundoContext;
            this.userBL = userBL;   
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserPostModel userPostModel)
        {
            try
            {
                this.userBL.AddUser(userPostModel);
                return this.Ok(new { success = true, Message = "User Added Sucessesfully.." });
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
