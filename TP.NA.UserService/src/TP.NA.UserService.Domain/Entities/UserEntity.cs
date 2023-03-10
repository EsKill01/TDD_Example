using System.Diagnostics.Metrics;
using System.Net;
using System;
using TP.NA.Common.Repository.Entities;

namespace TP.NA.UserService.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
       
        public UserEntity(string id, string email, string name, string lastName, string address, string country, string phoneNumber, string password, bool active)
        {
            Id = id;
            Email = email;
            Name = name;
            LastName = lastName;
            Address = address;
            Country = country;
            PhoneNumber = phoneNumber;
            Password = password;
            Active = active;
        }
        public UserEntity()
        {

        }
    }
}