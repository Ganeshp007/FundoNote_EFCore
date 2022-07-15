namespace BusinessLayer.Servies
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BusinessLayer.Interface;
    using DatabaseLayer.UserModels;
    using RepositoryLayer.Interface;
    using RepositoryLayer.Services.Entity;

    public class UserBL : IUserBL
    {
        IUserRL userRL;

        public UserBL(IUserRL userRL)
        {
          this.userRL=userRL;
        }

        public void AddUser(UserPostModel userPostModel)
        {
            try
            {
                this.userRL.AddUser(userPostModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                return this.userRL.GetAllUsers();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string LoginUser(UserLoginModel userLoginModel)
        {
            try
            {
                return this.userRL.LoginUser(userLoginModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
