using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class LoginDto
    {
        public string username { get; set; }

        public string password { get; set; }

        public bool rememberMe { get; set; }

        public LoginDto()
        {

        }

        public LoginDto(String username, String password, Boolean rememberMe)
        {
            
            this.username = username;
            this.password = password;
            this.rememberMe = rememberMe;
        }
    }
}
