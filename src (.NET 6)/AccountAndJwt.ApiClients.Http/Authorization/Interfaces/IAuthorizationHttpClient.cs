﻿using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Api.Response;

namespace AccountAndJwt.ApiClients.Http.Authorization.Interfaces
{
    public interface IAuthorizationHttpClient
    {
        // Token //
        Task<AuthorizeResponseAm> AuthorizeByCredentialsAsync(String login, String password);
        Task<String> RefreshTokenAsync(String refreshToken);
        Task RevokeTokenAsync(String refreshToken);

        // Account //
        Task<RegisterUserResponseAm> RegisterAsync(RegisterUserRequestAm request);
        Task DeleteAccountAsync(Int32 userId, String accessToken);
        Task<RoleAm[]> GetAvailableRolesAsync(String accessToken);
        Task AddUserRoleAsync(AddRemoveUserRoleRequestAm request, String accessToken);
        Task RemoveUserRoleAsync(AddRemoveUserRoleRequestAm request, String accessToken);
        Task ChangeEmailAsync(String newEmail, String accessToken);
        Task ChangeNameAsync(ChangeNameRequestAm request, String accessToken);
        Task<UserAm> GetUserAsync(String accessToken);
        Task<PagedUserResponseAm> GetUsersPagedAsync(GetUsersPagedRequestAm request, String accessToken);
        Task ResetPasswordAsync(ResetPasswordRequestAm request, String accessToken);

        // Debug //
        Task<WeatherForecastResponseAm[]> GetWeatherForecastAsync();
    }
}