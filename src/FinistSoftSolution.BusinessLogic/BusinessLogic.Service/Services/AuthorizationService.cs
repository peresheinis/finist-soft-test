using BusinessLogic.Core.Repositories;
using BusinessLogic.Service.Providers;
using Grpc.Core;
using Kernel.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace BusinesLogic.Service.Services;

public class AuthorizationService : Authorization.AuthorizationBase
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHashProvider _passwordHashProvider;
    private readonly ILogger<AuthorizationService> _logger;
    public AuthorizationService(
        IUsersRepository usersRepository,
        IPasswordHashProvider passwordHash,
        ILogger<AuthorizationService> logger)
    {
        _logger = logger;
        _usersRepository = usersRepository;
        _passwordHashProvider = passwordHash;
    }

    [AllowAnonymous]
    public override async Task<SignInResponse> SignIn(SignInRequest request, ServerCallContext context)
    {
        var user = await _usersRepository
            .GetByPhoneAsync(request.PhoneNumber);

        if (user is null)
        { 
            throw ConflictException
                .PermissionDenied("Не верный логин или пароль.");
        }

        if (!await _passwordHashProvider.ValidateAsync(
            request.Password, user.Password)) // здесь user.Password как хеш, не стал усложнять
        {
            throw ConflictException
                .PermissionDenied("Не верный логин или пароль.");
        }

        return new SignInResponse { Token = "" };
    }
}