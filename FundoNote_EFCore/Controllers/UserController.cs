namespace FundoNote_EFCore.Controllers
{
    using System;
    using BusinessLayer.Interface;
    using DatabaseLayer.UserModels;
    using Microsoft.AspNetCore.Mvc;
    using NLogger.Interface;
    using RepositoryLayer.Services;

    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILoggerManager logger;
        private readonly FundoContext fundoContext;
        private readonly IUserBL userBL;

        public UserController(FundoContext fundoContext,IUserBL userBL, ILoggerManager logger)
        {
            this.fundoContext = fundoContext;
            this.userBL = userBL;
            this.logger = logger;
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserPostModel userPostModel)
        {
            try
            {
                this.logger.LogInfo($"User Regestration Email : {userPostModel.Email}");
                this.userBL.AddUser(userPostModel);
                return this.Ok(new { success = true, Message = "User Added Sucessesfully.." });
            }
            catch (Exception ex)
            {
                logger.LogError($"User Regestration Fail: {userPostModel.Email}");
                throw ex;
            }
        }
    }
}
