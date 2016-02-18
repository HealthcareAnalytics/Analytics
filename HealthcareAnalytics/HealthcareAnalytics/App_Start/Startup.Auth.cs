using System;
using System.Data.Entity;
using HealthcareAnalytics.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

namespace HealthcareAnalytics
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            Database.SetInitializer<HosptialDBContext>(new DropCreateDatabaseIfModelChanges<HosptialDBContext>());
        }
    }
}