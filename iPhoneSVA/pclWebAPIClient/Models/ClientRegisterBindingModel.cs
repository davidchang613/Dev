using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIClient.Models
{
    public class ClientRegisterBindingModel : RegisterBindingModel
    {

    }

    public class RegisterBindingModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Number { get; set; }

        public string BaseUrl { get; set; }

    }
}
