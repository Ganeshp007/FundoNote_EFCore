namespace DatabaseLayer.UserModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class UserGetModel
    {
        public int UserId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime MoidifyDate { get; set; }
    }
}
