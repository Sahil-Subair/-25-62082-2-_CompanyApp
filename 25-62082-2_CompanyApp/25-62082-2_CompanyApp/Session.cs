using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _25_62082_2_CompanyApp
{
    public static class Session
    {
        public static int UserID { get; set; }
        public static string Username { get; set; }

        public static void Clear()
        {
            UserID = 0;
            Username = null;
        }
    }
}