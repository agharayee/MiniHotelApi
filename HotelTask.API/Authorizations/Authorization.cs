using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelTask.API.Authorizations
{
    public class Authorization
    {
        public enum Roles
        {
            Guest,
            Administrator,
            Super_Administrator
        }


        public const string default_SuperAdminUserName = "superAdmin";
        public const string default_SuperAdminemail = "superadmin@hotel.com";
        public const string default_SuperAdminpassword = "Password1234#";
        public const Roles default_SuperAdminrole = Roles.Super_Administrator;

        public const string default_Adminusername = "admin";
        public const string default_Adminemail = "admin@hotel.com";
        public const string default_Adminpassword = "Password1234#";
        public const Roles default_Adminrole = Roles.Administrator;

        public const string default_Guestusername = "guest";
        public const string default_Guestemail = "guest@hotel.com";
        public const string default_Guestpassword = "Password1234#";
        public const Roles default_Guestrole = Roles.Guest;
    }
}
