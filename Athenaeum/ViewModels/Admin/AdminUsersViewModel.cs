using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Athenaeum.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Athenaeum.ViewModels.Admin
{
    public class AdminUsersViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}