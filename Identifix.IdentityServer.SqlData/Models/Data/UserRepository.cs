using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net.Mime;

namespace Identifix.IdentityServer.Models.Data
{
    /// <summary>
    /// Handles Storage and retrieval of user related information
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Creates and Instance of a UserRepository
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(SqlContext context)
        {
            Guard.IsNotNull(context, "context");
            Context = context;
        }

        protected internal SqlContext Context { get; set; }

        /// <summary>
        /// Retreives a list of all countries
        /// </summary>
        /// <returns></returns>
        public IList<Country> RetrieveCountries()
        {
            return Context.Countries.OrderBy(country=>country.Name).ToList();
        }

        /// <summary>
        /// Retrieves a list of states having the specified countryId
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public IList<State> RetrieveStates(int countryId)
        {
            return Context.States.Where(state => state.CountryId == countryId).OrderBy(state=>state.Name).ToList();
        }

        public IList<State> RetrieveStates()
        {
            return Context.States.OrderBy(s => s.Name).ToList();
        }

        /// <summary>
        ///  Retrieves as list of states having a country with the specified code
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public IList<State> RetrieveStates(string countryCode)
        {
            string code = CultureInfo.InvariantCulture.TextInfo.ToUpper(countryCode);
            return
                Context.Countries.Include("States").SingleOrDefault(
                    country => country.Code==code)?
                    .States.OrderBy(state => state.Name).ToList() ?? new List<State>();
        }

        /// <summary>
        /// Checks to see if there is an existin guser with the spceified e-mail address.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public bool CheckIfEmailExists(string emailAddress)
        {
            string email = emailAddress.ToLower();
            return Context.Users.Any(user => user.Email.ToLower() == email);
        }

        public UserAccount SaveUser(UserAccount user)
        {
            UserAccount result = null;

            if (user.Id != 0)
            {
                result = user;  
                Context.Entry(user).State = EntityState.Modified;
                Context.Entry(user.Shop).State = EntityState.Modified;
                Context.Entry(user.Shop.Address).State = EntityState.Modified;
            }
            else
            {
                Context.Users.Add(user);
            }

            Context.SaveChanges();

            if (result == null)
            {
                result = Context.Users.FirstOrDefault(u => u.Email == user.Email);
            }

            return result;
        }
        
        public UserAccount RetrieveUser(int id)
        {
            var queryList = (from u in Context.Users
                join s in Context.Shops on u.ShopId equals s.Id
                join a in Context.Addresses on s.AddressId equals a.Id
                where u.Id == id
                select new
                {
                    UserId = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Password = u.Password,
                    BoShopId = s.ShopId,
                    AddressId = a.Id,
                    ShopId = s.Id,
                    Name = s.Name,
                    City = a.City,
                    CountryId = a.CountryId,
                    Line1 = a.Line1,
                    Line2 = a.Line2,
                    StateId = a.StateId,
                    PostalCode = a.PostalCode
                }).ToList();


              var result = queryList.Select(
                    q =>
                        new UserAccount()
                        {
                            Email = q.Email,
                            FirstName = q.FirstName,
                            Id = q.UserId,
                            LastName = q.LastName,
                            Password = q.Password,
                            ShopId = q.ShopId,
                            Shop = new Shop()
                            {
                                Id = q.ShopId,
                                ShopId = q.BoShopId,
                                AddressId = q.AddressId,
                                Name = q.Name,
                                Address = new Address()
                                {
                                    Id = q.AddressId,
                                    Line1 = q.Line1,
                                    Line2 = q.Line2,
                                    City = q.City,
                                    CountryId = q.CountryId,
                                    StateId =  q.StateId,
                                    PostalCode = q.PostalCode
                                }
                            }
                        }).FirstOrDefault();
                
                            
            return result;
        }

        public UserAccount RetrieveUser(string email)
        {
            var queryList = (from u in Context.Users
                             join s in Context.Shops on u.ShopId equals s.Id
                             join a in Context.Addresses on s.AddressId equals a.Id
                             where u.Email == email
                             select new
                             {
                                 UserId = u.Id,
                                 Email = u.Email,
                                 FirstName = u.FirstName,
                                 LastName = u.LastName,
                                 Password = u.Password,
                                 BoShopId = s.ShopId,
                                 AddressId = a.Id,
                                 ShopId = s.Id,
                                 Name = s.Name,
                                 City = a.City,
                                 CountryId = a.CountryId,
                                 Line1 = a.Line1,
                                 Line2 = a.Line2,
                                 StateId = a.StateId,
                                 PostalCode = a.PostalCode
                             }).ToList();


            var result = queryList.Select(
                  q =>
                      new UserAccount()
                      {
                          Email = q.Email,
                          FirstName = q.FirstName,
                          Id = q.UserId,
                          LastName = q.LastName,
                          Password = q.Password,
                          ShopId = q.ShopId,
                          Shop = new Shop()
                          {
                              Id = q.ShopId,
                              ShopId = q.BoShopId,
                              AddressId = q.AddressId,
                              Name = q.Name,
                              Address = new Address()
                              {
                                  Id = q.AddressId,
                                  Line1 = q.Line1,
                                  Line2 = q.Line2,
                                  City = q.City,
                                  CountryId = q.CountryId,
                                  StateId = q.StateId,
                                  PostalCode = q.PostalCode
                              }
                          }
                      }).FirstOrDefault();


            return result;
        }

        public Shop RetrieveShop(Address address)
        {
            var existingAddress = Context.Addresses.FirstOrDefault(a =>
                a.Line1 == address.Line1
                && a.City == address.City
                && a.PostalCode == address.PostalCode);

            if (existingAddress != null)
            {
                var result = (from s in Context.Shops
                    join a in Context.Addresses
                        on s.AddressId equals a.Id
                    where a.Line1 == address.Line1
                          && a.City == address.City
                          && a.PostalCode == address.PostalCode
                    select s).FirstOrDefault();

                if (result != null)
                {
                    result.AddressId = existingAddress.Id;
                    result.Address = existingAddress;
                }

                return result;
            }

            return null;
        }

        public string SaveResetPasswordData(int userId)
        {
            var existingToken = Context.ResetPasswordLinks.SingleOrDefault(x => x.UserId == userId && x.Expiration > DateTime.Now);
            if (existingToken != null)
            {
                existingToken.Expiration = DateTime.Now;
            }

            string token = Guid.NewGuid().ToString();

            Context.ResetPasswordLinks.Add(new ResetPasswordLink()
            {
                UserId = userId,
                Token = token,
                Expiration = DateTime.Now.AddMinutes(AppSettings.PasswordResetExpirationAmount),
            });

            Context.SaveChanges();
            return token;
        }

        public int? IsValidToken(string token)
        {
            Guard.IsNotNullOrWhiteSpace(token, "Password reset token");

            var validResetPasswordData = Context.ResetPasswordLinks.SingleOrDefault(x => x.Token == token && x.Expiration > DateTime.Now);
            return validResetPasswordData != null ? validResetPasswordData.UserId : (int?)null;
        }

        public string PasswordResetLinkExistsForUser(int userId)
        {
            var existingPasswordData = Context.ResetPasswordLinks.SingleOrDefault(x => x.User.Id == userId && x.Expiration > DateTime.Now);
            return existingPasswordData != null ? existingPasswordData.Token : "";
        }

        public void ExpireResetPasswordTokenForUser(int userId)
        {
            var tokenData = Context.ResetPasswordLinks.SingleOrDefault(x => x.UserId == userId && x.Expiration > DateTime.Now);
            if (tokenData != null)
            {
                tokenData.Expiration = DateTime.Now;
                Context.SaveChanges();
            }
        }
    }
}