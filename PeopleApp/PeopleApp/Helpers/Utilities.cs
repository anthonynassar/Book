using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Helpers
{
    public static class Utilities
    {
        public static string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
