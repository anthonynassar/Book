using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Helpers
{
    public static class Locations
    {
//#if DEBUG
//        public static readonly string AppServiceUrl = "http://localhost:52192/";
//        public static readonly string AlternateLoginHost = "https://peopleapp3.azurewebsites.net";
//#else
        public static readonly string AppServiceUrl = "https://peopleapp3.azurewebsites.net";
        public static readonly string AlternateLoginHost = null;
//#endif
    }
}