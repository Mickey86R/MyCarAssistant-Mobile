using System.Collections.Generic;

namespace MyCarAssistant.Models
{

    public class User
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}