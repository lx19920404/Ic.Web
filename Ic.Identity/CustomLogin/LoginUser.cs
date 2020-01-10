using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Identity
{
    [Table("TLoginUsers")]
    public class LoginUser
    {
        [Column("LoginUserId")]
        public string Id { get; set; }

        [Column("LoginUserName")]
        public string UserName { get; set; }

        [Column("LoginUserPassword")]
        public string Password { get; set; }

        [Column("LoginUserRealName")]
        public string RealName { get; set; }

        [Column("LoginUserEmail")]
        public string Email { get; set; }
    }
}
