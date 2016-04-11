using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Identifix.IdentityServer.Models;

namespace Identifix.IdentityServer.App_Start
{
    public static class AutoMapperConfiguration
    {
        /// <summary>
        /// Should get called once per app domain (see Startup.cs or perhaps global.asax.cs)
        /// </summary>
        public static void DefineAutoMapConfiguration()
        {
            //Incoming: FROM UserProfile (DTO) TO a User
            Mapper.CreateMap<UserProfile, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));

            Mapper.CreateMap<UserProfile,Shop>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ShopName))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ShopId));

            Mapper.CreateMap<UserProfile,Address>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressId))
                .ForMember(dest => dest.Line1, opt => opt.MapFrom(src => src.Address1))
                .ForMember(dest => dest.Line2, opt => opt.MapFrom(src => src.Address2))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.ZipCode));

            //Outgoing: From User to UserProfile (DTO)
            Mapper.CreateMap<User, UserProfile>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.ShopId, opt => opt.MapFrom(src => src.Shop.Id))
                .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Shop.Name))
                .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.Shop.Address.Id))
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(src => src.Shop.Address.Line1))
                .ForMember(dest => dest.Address2, opt => opt.MapFrom(src => src.Shop.Address.Line2))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Shop.Address.PostalCode))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Shop.Address.City))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.Shop.Address.CountryId))
                .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => src.Shop.Address.StateId));


            //Incoming NewUserRegistration (DTO) to Address
            Mapper.CreateMap<NewUserRegistration, Address>()
                .ForMember(dest => dest.Line1, opt => opt.MapFrom(src => src.Address1))
                .ForMember(dest => dest.Line2, opt => opt.MapFrom(src => src.Address2))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.ZipCode));

            Mapper.CreateMap<NewUserRegistration, Shop>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ShopName));

            Mapper.CreateMap<NewUserRegistration, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

            MappingConfig.Configure();
        }
    }
}