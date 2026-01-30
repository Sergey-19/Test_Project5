using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.Core.Common.Navigation
{
    public class UserAccount
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public EPermission Permission { get; set; } = EPermission.Operator;

        public UserAccount CloneWithoutPassword()
        {
            return new UserAccount
            {
                UserName = UserName,
                Permission = Permission,
                Password = string.Empty
            };
        }
    }
}
