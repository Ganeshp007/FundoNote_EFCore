namespace RepositoryLayer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DatabaseLayer.UserModels;
    using Microsoft.Extensions.Configuration;
    using RepositoryLayer.Interface;
    using RepositoryLayer.Services.Entity;

    public class UserRL : IUserRL
    {
        private readonly FundoContext fundoContext;
        private readonly IConfiguration iconfiguration;

        public UserRL(FundoContext fundoContext,IConfiguration iconfiguration)
        {
            this.fundoContext = fundoContext;
            this.iconfiguration = iconfiguration;
        }

        public void AddUser(UserPostModel userPostModel)
        {
            try
            {
                User user = new User();
                user.Firstname = userPostModel.Firstname;
                user.Lastname = userPostModel.Lastname;
                user.Email = userPostModel.Email;
                user.Password = userPostModel.Password;
                user.CreateDate = DateTime.Now;
                user.ModifiedDate = DateTime.Now;
                this.fundoContext.Users.Add(user);
                this.fundoContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
