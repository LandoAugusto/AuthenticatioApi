﻿using Microsoft.AspNetCore.Identity;

namespace AuthenticatioApi.Infra.Identity.Models
{
    public class ApplicationUserClaim : IdentityUserClaim<int>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
