using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IgrajKarte.Helpers
{
    public class SessionManager
    {
        public string ReturnUrl { get; set; }
        public User CurrentUser { get; set; }
    }
}