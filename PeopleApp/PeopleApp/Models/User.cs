using PeopleApp.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Models
{
    public class User : TableData
    {
        public string Email { get; set; }
        public string Address { get; set; }
        public string Profession { get; set; }
        public string Privilege { get; set; }
        public string Username { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; }
        public string CultureInfo { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public override bool Equals(System.Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            User user = (User)obj;

            return Email.Equals(user.Email) && Address.Equals(user.Address) &&
                Profession.Equals(user.Profession) && Privilege.Equals(user.Privilege) &&
                Username.Equals(user.Username) && GivenName.Equals(user.GivenName) &&
                Surname.Equals(user.Surname) && Birthdate.Equals(user.Birthdate) &&
                Gender.Equals(user.Gender) && CultureInfo.Equals(user.CultureInfo) &&
                City.Equals(user.City) && Country.Equals(user.Country);
        }
    }
}
