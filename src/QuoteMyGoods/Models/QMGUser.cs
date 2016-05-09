﻿using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;

namespace QuoteMyGoods.Models
{
    public class QMGUser:IdentityUser
    {
        public string JoinTheDotsKey { get; set; }
    }

    public static class PrincipleExtensions
    {
        public static string GetJoinTheDots(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("JoinTheDots");
        }
    }

}