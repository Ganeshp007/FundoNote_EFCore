namespace RepositoryLayer.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DatabaseLayer.UserModels;
    using RepositoryLayer.Services.Entity;

    public interface IUserRL
    {
        public void AddUser(UserPostModel userPostModel);

        public List<User> GetAllUsers();

        public string LoginUser(UserLoginModel userLoginModel);

        public bool ForgetPasswordUser(string email);

    }
}
