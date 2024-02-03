using Gateway.API.Consts;
using Gateway.API.Extensions;
using Gateway.API.Protos;
using Gateway.API.Services;
using Gateway.Shared.Responses;
using Grpc.Net.ClientFactory;
using Kernel.Shared.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.API.Controllers;

public class AuthorizationController : ApiControllerBase
{
    private readonly ICookiesWriteService _cookiesService;
    private readonly Authorization.AuthorizationClient _client;
    public AuthorizationController(
        ICookiesService cookiesService,
        GrpcClientFactory grpcClientFactory)
    {
        _cookiesService = cookiesService;

        _client = grpcClientFactory
            .CreateClient<Authorization.AuthorizationClient>(GrpcClientsNames.Authorization);
    }

    /// <summary>
    /// ������ �� ����������� ����������� �������
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("SignIn")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default)
    { 
        var response = await _client
            .SignInAsync(request, cancellationToken: cancellationToken);

        _cookiesService
            .AddAuthorizationCookies(HttpContext, response.Token);

        // ��� ��� ����� �������� � Command, Handler<Command> MediatR,
        // ��� � ��������� AuthorizationService

        return response.Token;
    }

    [Authorize]
    [HttpGet("State")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public ActionResult<UserStateDto> GetState() => new UserStateDto(HttpContext.GetUserFullName());

    // ��� ����� ����������� ����� SignOut, �������
    // ����� ������� Authorization Cookies ������������, � ��� ��
    // �������� �������� ����������� �� ����� ������������ ��� ���-������ ������...
}