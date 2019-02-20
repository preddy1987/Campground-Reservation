using System;
using System.Collections.Generic;
using System.Text;

namespace EncryptionMessenger.Models
{
   public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
