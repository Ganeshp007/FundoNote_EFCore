namespace BusinessLayer.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DatabaseLayer.UserModels;

    public interface IUserBL
    {
        public void AddUser(UserPostModel userPostModel);

    }
}
