using System;
using AutoMapper;
using CodingKitchen.AzureFunctions.Extensions;
using CodingKitchen.Homemoney.Models;
using CodingKitchen.Monobank.Models;

namespace CodingKitchen.AzureFunctions.MappingProfiles
{
    public class TransactionMappingProfile : Profile
    {
        public TransactionMappingProfile(HomemoneyConfiguration homemoneyConfiguration)
        {
            CreateMap<StatementItem, AccountingTransaction>()
                // Preconfigured fields
                .ForMember(d => d.Token, opts => opts.MapFrom(s => homemoneyConfiguration.Token))
                .ForMember(d => d.AccountId, opts => opts.MapFrom(s => homemoneyConfiguration.AccountId))
                .ForMember(d => d.CurrencyId, opts => opts.MapFrom(s => homemoneyConfiguration.CurencyId))
                .ForMember(d => d.CategoryId, opts => opts.MapFrom(s => s.Amount < 0
                    ? homemoneyConfiguration.CreditCategoryId : homemoneyConfiguration.DebitCategoryId))
                .ForMember(d => d.Type, opts => opts.MapFrom(s => s.Amount < 0
                    ? homemoneyConfiguration.CreditType : homemoneyConfiguration.DebitType))
                .ForMember(d => d.TransAccountId, opts => opts.MapFrom(s => homemoneyConfiguration.TransAccountId))
                .ForMember(d => d.TransCurrencyId, opts => opts.MapFrom(s => homemoneyConfiguration.TransCurencyId))
                .ForMember(d => d.IsPlan, opts => opts.MapFrom(s => false))
                .ForMember(d => d.GUID, opts => opts.MapFrom(s => Guid.NewGuid()))
                // Actual transaction contents
                .ForMember(d => d.Total, opts => opts.MapFrom(s => (double) Math.Abs(s.Amount) / 100))
                .ForMember(d => d.Date, opts => opts.MapFrom(s => s.Time.ToDateTime()))
                .ForMember(d => d.Description, opts => opts.MapFrom(s => s.Description))
                .ForMember(d => d.TransTotal, opts => opts.MapFrom(s => s.Amount / 100))
                .ReverseMap()
                ;
        }
    }
}
