using AccountAndJwt.Contracts.Const;
using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Api.Response;
using AspNetCore.Http.Extensions;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AccountAndJwt.IntegrationTests.Api
{
    public class AccountControllerTests : IDisposable
    {
        private readonly CustomWebApplicationFactory _factory;


        public AccountControllerTests()
        {
            _factory = new CustomWebApplicationFactory();
        }
        public void Dispose()
        {
            _factory?.Dispose();
        }


        // TESTS //////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task RegisterNewUserTest()
        {
            const String login = "test";
            var client = _factory.CreateClient();

            var registerUserResponse = await RegisterNewUser(client, login);

            Assert.True(registerUserResponse.AccessTokenUrl == "/api/Token/AuthorizeByCredentials");
            Assert.True(registerUserResponse.RefreshAccessTokenUrl == "/api/Token/RefreshToken");
            Assert.True(registerUserResponse.RevokeRefreshTokenUrl == "/api/Token/RevokeToken");

            await _factory.AuthorizeAsAdminAsync(client);
            var pagedUsers = await GetUsersPaged(client);

            Assert.True(pagedUsers.Users.Length == 3);
            Assert.All(pagedUsers.Users, Assert.NotNull);
        }

        [Fact]
        public async Task DeleteAccountTest()
        {
            const String login = "test";
            var client = _factory.CreateClient();

            await RegisterNewUser(client, login);
            await _factory.AuthorizeAsAdminAsync(client);
            var pagedUsers = await GetUsersPaged(client);

            Assert.True(pagedUsers.Users.Length == 3);
            Assert.All(pagedUsers.Users, Assert.NotNull);

            var userForDeletion = pagedUsers.Users.First(p => p.Login.Equals(login));
            var response = await client.DeleteAsync($"/api/Administration/DeleteAccount?userId={userForDeletion.Id}");
            response.CheckError();

            pagedUsers = await GetUsersPaged(client);

            Assert.True(pagedUsers.Users.Length == 2);
            Assert.All(pagedUsers.Users, Assert.NotNull);
        }

        [Fact]
        public async Task GetAvailableRolesTest()
        {
            var client = _factory.CreateClient();
            await _factory.AuthorizeAsAdminAsync(client);
            var roles = await GetAvailableRoles(client);

            Assert.True(roles.Length == 2);
        }

        [Fact]
        public async Task AddUserRoleTest()
        {
            const String login = "guest";

            var client = _factory.CreateClient();
            await _factory.AuthorizeAsAdminAsync(client);

            var pagedUsers = await GetUsersPaged(client);
            var guest = pagedUsers.Users.First(p => p.Login.Equals(login));
            var roles = await GetAvailableRoles(client);
            var moderatorRole = roles.First(p => p.Name.Equals(Role.Moderator));

            await AddUserRole(client, guest.Id, moderatorRole.Id);

            guest = await GetUser(client, guest.Id);

            Assert.True(guest.Login.Equals(login));
            Assert.True(guest.Roles.Length == 1);
            Assert.True(guest.Roles[0].Name.Equals(Role.Moderator));
        }

        [Fact]
        public async Task RemoveUserRoleTest()
        {
            const String login = "admin";

            var client = _factory.CreateClient();
            await _factory.AuthorizeAsAdminAsync(client);

            var pagedUsers = await GetUsersPaged(client);
            var admin = pagedUsers.Users.First(p => p.Login.Equals(login));
            var roles = await GetAvailableRoles(client);
            var moderatorRole = roles.First(p => p.Name.Equals(Role.Moderator));

            await RemoveUserRole(client, admin.Id, moderatorRole.Id);

            admin = await GetUser(client, admin.Id);

            Assert.True(admin.Login.Equals(login));
            Assert.True(admin.Roles.Length == 1);
            Assert.True(admin.Roles[0].Name.Equals(Role.Admin));
        }

        [Fact]
        public async Task ChangeEmailTest()
        {
            const String login = "admin";
            const String newEmail = "2170@inbox.ru";

            var client = _factory.CreateClient();
            await _factory.AuthorizeAsAdminAsync(client);

            var pagedUsers = await GetUsersPaged(client);
            var admin = pagedUsers.Users.First(p => p.Login.Equals(login));

            var response = await client.PutAsJsonAsync("/api/Account/ChangeEmail", new ChangeEmailRequestAm()
            {
                NewEmail = newEmail
            });
            response.CheckError();
            admin = await GetUser(client, admin.Id);

            Assert.True(admin.Login.Equals(login));
            Assert.True(admin.Email.Equals(newEmail));
        }

        [Fact]
        public async Task ChangeUserNameTest()
        {
            const String login = "admin";
            const String newFirstName = "New first name";
            const String newLastName = "New last name";

            var client = _factory.CreateClient();
            await _factory.AuthorizeAsAdminAsync(client);

            var pagedUsers = await GetUsersPaged(client);
            var admin = pagedUsers.Users.First(p => p.Login.Equals(login));

            var response = await client.PutAsJsonAsync("/api/Account/ChangeName", new ChangeNameRequestAm()
            {
                FirstName = newFirstName,
                LastName = newLastName
            });
            response.EnsureSuccessStatusCode();
            admin = await GetUser(client, admin.Id);

            Assert.True(admin.FirstName.Equals(newFirstName));
            Assert.True(admin.LastName.Equals(newLastName));
        }

        [Fact]
        public async Task GetUserTest()
        {
            const String login = "guest";

            var client = _factory.CreateClient();
            await _factory.AuthorizeAsAdminAsync(client);
            var pagedUsers = await GetUsersPaged(client);
            var guest = pagedUsers.Users.First(p => p.Login.Equals(login));

            var user = await GetUser(client, guest.Id);

            Assert.True(user.Login.Equals(login));
            Assert.True(user.Roles.Length == 0);
        }

        [Fact]
        public async Task GetUsersPagedTest()
        {
            var client = _factory.CreateClient();
            await _factory.AuthorizeAsAdminAsync(client);
            var pagedUsers = await GetUsersPaged(client);

            Assert.True(pagedUsers.Users.Length == 2);
            Assert.All(pagedUsers.Users, Assert.NotNull);
        }

        [Fact]
        public async Task ResetPasswordTest()
        {
            const String login = "admin";
            const String oldPassword = "ipANWvuFUA5e2qWk0iTd";
            const String newPassword = "5TxfJkRgvmQVbjrxdIP9";

            var client = _factory.CreateClient();
            await _factory.AuthorizeAsAdminAsync(client);
            var response = await client.PostAsJsonAsync("/api/Account/ResetPassword", new ResetPasswordRequestAm()
            {
                OldPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmPassword = newPassword
            });
            response.EnsureSuccessStatusCode();

            client = _factory.CreateClient();

            response = await client.PostAsJsonAsync("/api/Token/AuthorizeByCredentials", new AuthorizeRequestAm()
            {
                Login = login,
                Password = newPassword
            });
            response.EnsureSuccessStatusCode();
            var authorizationTokens = await response.DeserializeAsync<AuthorizeResponseAm>();

            Assert.NotNull(authorizationTokens.AccessToken);
            Assert.NotNull(authorizationTokens.RefreshToken);
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private async Task<RegisterUserResponseAm> RegisterNewUser(HttpClient client, String login)
        {
            var response = await client.PostAsJsonAsync("/api/Account/Register", new RegisterUserRequestAm()
            {
                Login = login,
                Password = "testPassword",
                ConfirmPassword = "testPassword",
                FirstName = "Test first name",
                LastName = "Test last name",
                Email = "qwerty@mail.ru"
            });
            response.CheckError();

            return await response.DeserializeAsync<RegisterUserResponseAm>();
        }
        private async Task<PagedUserResponseAm> GetUsersPaged(HttpClient client)
        {
            var response = await client.PostAsJsonAsync("/api/Administration/GetUsersPaged", new GetUsersPagedRequestAm());
            response.CheckError();

            return await response.DeserializeAsync<PagedUserResponseAm>();
        }
        private async Task<RoleAm[]> GetAvailableRoles(HttpClient client)
        {
            var response = await client.GetAsync("/api/Administration/GetAvailableRoles");
            response.CheckError();

            return await response.DeserializeAsync<RoleAm[]>();
        }
        private async Task<UserAm> GetUser(HttpClient client, Int32 userId)
        {
            var response = await client.GetAsync($"/api/Administration/GetUser?userId={userId}");
            response.CheckError();

            return await response.DeserializeAsync<UserAm>();
        }
        private async Task AddUserRole(HttpClient client, Int32 userId, Int32 roleId)
        {
            var response = await client.PostAsJsonAsync("/api/Administration/AddUserRole", new AddRemoveUserRoleRequestAm()
            {
                UserId = userId,
                RoleId = roleId
            });

            response.CheckError();
        }
        private async Task RemoveUserRole(HttpClient client, Int32 userId, Int32 roleId)
        {
            var response = await client.PostAsJsonAsync("/api/Administration/RemoveUserRole", new AddRemoveUserRoleRequestAm()
            {
                UserId = userId,
                RoleId = roleId
            });

            response.CheckError();
        }
    }
}