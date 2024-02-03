using Gateway.API.Consts;
using Gateway.API.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.ClientFactory;
using Kernel.Shared.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.API.Controllers;

public class AccountsController : ApiControllerBase
{
    private readonly Accounts.AccountsClient _client;
    public AccountsController(GrpcClientFactory grpcClientFactory)
    {
        _client = grpcClientFactory
            .CreateClient<Accounts.AccountsClient>(GrpcClientsNames.Accounts);
    }

    /// <summary>
    /// Выполнить запрос на получение банковских счетов текущего авторизованного клиента
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyCollection<BankAccountModel>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _client
            .GetAllAsync(new Empty(), cancellationToken: cancellationToken);

        // Тут мы должны возвращать
        // PagedList<BankAccountModel>(items: IReadOnlyCollection<BankAccountModel>, totalItems: totalItemsCount)
        // Так же мы должны передавать в метод offset & quantity или currentPage, pageSize

        return response.Accounts;
    }
}