using BusinessLogic.Core.Repositories;
using BusinessLogic.Service.Auth.Abstractions;
using BusinessLogic.Service.Protos;
using BusinessLogic.Service.Providers;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace BusinessLogic.Service.ProtosServices;

public class AuthorizationService : Authorization.AuthorizationBase
{
    private readonly IJwtTokenProvider _jwtTokenProvider;
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHashProvider _passwordHashProvider;
    public AuthorizationService(
        IJwtTokenProvider jwtTokenProvider,
        IUsersRepository usersRepository,
        IPasswordHashProvider passwordHash)
    {
        _usersRepository = usersRepository;
        _jwtTokenProvider = jwtTokenProvider;
        _passwordHashProvider = passwordHash;
    }

    [AllowAnonymous]
    public override async Task<SignInResponse> SignIn(SignInRequest request, ServerCallContext context)
    {
        var user = await _usersRepository
            .GetByPhoneAsync(request.PhoneNumber);

        if (user is null)
        {
            throw new RpcException(
                new Status(StatusCode.PermissionDenied, "�� ������ ����� ��� ������"));
        }
        
        // ����� user.Password ��� ���, �� ���� ���������
        if (!await _passwordHashProvider.ValidateAsync(
            request.Password, user.Password)) 
        {
            throw new RpcException(
                new Status(StatusCode.PermissionDenied, "�� ������ ����� ��� ������"));
        }

        // ����� ���� ������������ ���� ������ Jwt - ����� � Refresh - �����,
        // Refreh - ����� ����� ���������� � �� � ��������� ��� ��������� Jwt - ������
        // � ������������ �� ��� ������ �����

        return new SignInResponse { Token = _jwtTokenProvider.CreateToken(user) };
    }

    // ��� ����� �������� RefreshToken �����
}