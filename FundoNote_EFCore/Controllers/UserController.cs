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

        [HttpPost("LoginUser")]
        public IActionResult LoginUser(UserLoginModel userModel)
        {
            try
            {
                string token = this.userBL.LoginUser(userModel);
                if (token == null)
                {
                    this.logger.LogError($"Login Unsuccessfull : {userModel.Email}");
                    return this.BadRequest(new { success = false, message = "Enter Valid Email and Password!!" });
                }

                this.logger.LogInfo($"User Login Successfull : {userModel.Email}");
                return this.Ok(new { success = true, message = "User Loged In Successfully...", data = token });
            }
            catch (Exception ex)
            {
                this.logger.LogError($"User Login Failed : {userModel.Email}");
                throw ex;
            }
        }

        [HttpPost("ForgetPassword/{email}")]
        public IActionResult ForgetUser(string email)
        {
            try
            {
                bool isExist = this.userBL.ForgetPasswordUser(email);
                if (isExist)
                {
                    this.logger.LogInfo($"Password RestLink Sent Successfully for : {email}");
                    return Ok(new { success = true, message = $"Password Reset Link sent successfully for : {email}" });
                }
                this.logger.LogInfo($"Password RestLink Sent UnSuccessfully for : {email}");
                return BadRequest(new { success = false, message = $"No User Exist with Email : {email}" });
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Something Went Wrong  : {email}");
                throw ex;
            }
        }
    }
}
