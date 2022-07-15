namespace DatabaseLayer.UserModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class PasswordModel
    {
        [Required]
        [RegularExpression("(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$_])[a-zA-Z0-9@#$_]{8,}", ErrorMessage = "Please Enter For Password At least 1 numeric,1 Capital char,1 Special char")] //Password Validation
        public string Password { get; set; }

        [Required]
        [RegularExpression("(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$_])[a-zA-Z0-9@#$_]{8,}", ErrorMessage = "Confirm Password should be same as Password!!")] //Password Validation
        public string ConfirmPassword { get; set; }
    }
}
