namespace BusinessLayer.Servies
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BusinessLayer.Interface;
    using DatabaseLayer.UserModels;
    using RepositoryLayer.Interface;

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
    }
}
