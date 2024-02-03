using AutoMapper;
using BusinessLogic.Core.Entities;
using BusinessLogic.Service.Protos;

namespace BusinessLogic.Service.Mapping;

public class BankAccountMapperProfile : Profile
{
    public BankAccountMapperProfile()
    {
        CreateMap<BankAccount, BankAccountModel>();
    }
}