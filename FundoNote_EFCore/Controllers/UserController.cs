﻿namespace FundoNote_EFCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using BusinessLayer.Interface;
    using DatabaseLayer.UserModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NLogger.Interface;
    using RepositoryLayer.Services;
    using RepositoryLayer.Services.Entity;

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
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
                var UniqueMailCheck = fundoContext.Users.Where(x => x.Email == userPostModel.Email).FirstOrDefault();
                if (UniqueMailCheck == null)
                {
                    this.userBL.AddUser(userPostModel);
                    this.logger.LogInfo($"User Regestration Successfull with Email: {userPostModel.Email}");
                    return this.Ok(new { success = true, Message = "User Added Sucessesfully.." });
                }

                this.logger.LogError($"User Regestration UnSuccessfull with Email: {userPostModel.Email}");
                return this.BadRequest(new { success = false, Message = "Email Already Exists!!" });
            }
            catch (Exception ex)
            {
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
                    return this.Ok(new { success = true, message = "User Data Restrieved Successfully...", data = getUsers });
                }

                this.logger.LogError($"No Users Exists at moment in DB...");
                return this.BadRequest(new { sucess = false, message = "You Dont have any User at the moment in DB!!" });
            }
            catch (Exception ex)
            {
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
                    this.logger.LogInfo($"Password ResetLink Sent Successfully for : {email}");
                    return this.Ok(new { success = true, message = $"Password Reset Link sent successfully for : {email}" });
                }

                this.logger.LogError($"Password ResetLink Sent UnSuccessfully for : {email}");
                return this.BadRequest(new { success = false, message = $"No User Exist with Email : {email}" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(PasswordModel modelPassword)
        {
            try
            {
                if (modelPassword.Password != modelPassword.ConfirmPassword)
                {
                    this.logger.LogError($"Password Reset UnSuccessfull Due to Invalid Credentials!!!");
                    return this.BadRequest(new { success = false, message = "New Password and Confirm Password are not equal." });
                }

                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    var email = claims.Where(p => p.Type == @"Email").FirstOrDefault()?.Value;
                    this.userBL.ResetPassword(email, modelPassword);
                    this.logger.LogInfo($"Password Reset Successfully for : {email}");
                    return this.Ok(new { success = true, message = "Password Reset Sucessfully...", email = $"{email}" });
                }
                else
                {
                    this.logger.LogError($"Password Reset UnSuccessfull!!!");
                    return this.BadRequest(new { success = false, message = "Password Reset Unsuccessful!!!" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
