using BlueCarGpsLib.Helper;
using BlueCarGpsLib.Model.UserAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueCarGpsLib.Data
{
   public partial class User
    {
        public RoleType roleTypeEnum
        {
            get
            {
                return (RoleType)this.roleType;
            }
        }
        
        public string roleTypeDisplay
        {
            get
            {
                return EnumHelper.GetDescriptionByFiledName(this.roleTypeEnum.ToString(), typeof(RoleType));
            }
        }

        public string GenSalt()
        {
            // return Guid.NewGuid().ToString();
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[36];
            rng.GetBytes(saltBytes);
            string salt = Convert.ToBase64String(saltBytes);
            return salt;
        }
    }
}
