using AutoMapper;
using BusinessLogic.Core.Repositories;
using BusinessLogic.Service.Extensions;
using BusinessLogic.Service.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace BusinessLogic.Service.ProtosServices;

public class AccountsService : Accounts.AccountsBase
{
    private readonly IMapper _mapper;
    private readonly IBankAccountsRepository _bankAccountsRepository;

    public AccountsService(
        IMapper mapper,
        IBankAccountsRepository bankAccountsRepository)
    {
        _mapper = mapper;
        _bankAccountsRepository = bankAccountsRepository;
    }

    [Authorize]
    public override async Task<GetAllAccountsResponse> GetAll(Empty empty, ServerCallContext context)
    {
        var accounts = await _bankAccountsRepository
            .GetByUserIdAsync(context.GetUserId());

        var accountsDtos = _mapper
            .Map<IReadOnlyCollection<BankAccountModel>>(accounts);

        var response = new GetAllAccountsResponse();

        response.Accounts
            .AddRange(accountsDtos);

        return response;
    }
}