namespace FundoNote_EFCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using BusinessLayer.Interface;
    using DatabaseLayer.UserModels;
    using Microsoft.AspNetCore.Mvc;
    using NLogger.Interface;
    using RepositoryLayer.Services;
    using RepositoryLayer.Services.Entity;

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

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {

                List<User> getUsers = new List<User>();
                getUsers = this.userBL.GetAllUsers();
                if (getUsers.Count > 0)
                {
                    this.logger.LogInfo($"User Data Retrieved Succesfully...");
                    return Ok(new { success = true, message = "User Data Restrieved Successfully...", data = getUsers });
                }
                this.logger.LogInfo($"No Users Exists at moment in DB...");
                return BadRequest(new { sucess = false, message = "You Dont have any User at the moment in DB!!" });
            }
            catch (Exception ex)
            {
                this.logger.LogInfo($"User Data Retrieve UnSuccesfull...");
                throw ex;
            }
        }
    }
}
