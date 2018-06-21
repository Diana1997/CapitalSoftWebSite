using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CapitalSoftWebSite.Models
{
    public class ConnectionParameters
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
}