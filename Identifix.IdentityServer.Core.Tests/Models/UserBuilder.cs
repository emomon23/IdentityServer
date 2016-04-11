using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identifix.IdentityServer.Models;

namespace Identifix.IdentityServer.Tests.Models
{
    public class UserBuilder
    {
        public string DefaultFirstName = "John";
        public const string DefaultLastName = "Smith";
        public const string DefaultEmail = "john.smith@mail.non";
        public const int DefaultId = 6502;
        public const int DefaultShopId = 8086; 

        public UserBuilder()
        {
            _user = new User();
        }

        private User _user;

        public UserBuilder WithFirstName(string name)
        {
            _user.FirstName = name;
            return this;
        }

        public UserBuilder WithDefaultFirstName()
        {
            return WithFirstName(DefaultFirstName);
        }

        public UserBuilder WithLastName(string name)
        {
            _user.LastName = name;
            return this;
        }

        public UserBuilder WithDefaultLastName()
        {
            return WithLastName(DefaultLastName);
        }

        public UserBuilder WithEmail(string email)
        {
            _user.Email = email;
            return this;
        }

        public UserBuilder WithDefaultEmail()
        {
            return WithEmail(DefaultEmail);
        }

        public UserBuilder WithId(int id)
        {
            _user.Id = id;
            return this;
        }

        public UserBuilder WithDefaultId()
        {
            return WithId(DefaultId);
        }

        public UserBuilder WithShopId(int shopId)
        {
            _user.ShopId = shopId;
            return this;
        }

        public UserBuilder WithDefaultShopId()
        {
            return WithShopId(DefaultShopId);
        }

        public UserBuilder AsDefault()
        {
            return this.WithDefaultEmail().WithDefaultFirstName().WithDefaultLastName().WithDefaultId().WithDefaultShopId();
        }

        public User Build()
        {
            return _user;
        }

        public static implicit operator User(UserBuilder builder)
        {
            return builder.Build();
        } 
    }
}
