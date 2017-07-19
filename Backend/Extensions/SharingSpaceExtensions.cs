using Backend.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Extensions
{
    public static class SharingSpaceExtensions
    {
        public static IQueryable<SharingSpace> PerUserFilter(this IQueryable<SharingSpace> query, string userid)
        {
            return query.Where(item => item.UserId.Equals(userid));
        }
    }
}