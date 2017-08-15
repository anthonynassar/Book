using Backend.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Extensions
{
    public static class UserExtensions
    {
        public static IQueryable<User> PerUserFilter(this IQueryable<User> query, string userid)
        {
            return query.Where(item => item.Id.Equals(userid));
        }
    }
}