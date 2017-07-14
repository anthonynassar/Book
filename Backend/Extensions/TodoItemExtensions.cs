using Backend.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Extensions
{
    public static class TodoItemExtensions
    {
        public static IQueryable<TodoItem> PerUserFilter(this IQueryable<TodoItem> query, string userid)
        {
            return query.Where(item => item.UserId.Equals(userid));
        }
    }
}