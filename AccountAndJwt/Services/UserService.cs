using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Middleware;
using AccountAndJwt.Models.Config;
using AccountAndJwt.Models.Database;
using AccountAndJwt.Models.Exceptions;
using AccountAndJwt.Models.Service;
using AccountAndJwt.Services.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccountAndJwt.Services
{
    internal class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly AudienceConfig _audienceConfig;


        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, IOptions<AudienceConfig> audienceConfig)
        {
            _audienceConfig = audienceConfig.Value;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
        }


        // IUserService ///////////////////////////////////////////////////////////////////////////
        public void Register(UserDto user)
        {
            var requestedUser = _unitOfWork.Users.GetByLoginEager(user.Login);
            if (requestedUser != null)
                throw new LoginIsAlreadyUsedException("User name is already occupied");

            var userDb = _mapper.Map<UserDb>(user);
            _unitOfWork.Users.Add(userDb);
            _unitOfWork.Commit();
        }
        public void DeleteUser(Int32 id)
        {
            var requestedUser = _unitOfWork.Users.GetEager(id);
            if (requestedUser == null)
                throw new UserNotFoundException("User with provided id was not found");

            _unitOfWork.Users.Remove(requestedUser);
            _unitOfWork.Commit();
        }
        public UserDto GetUser(Int32 id)
        {
            var requestedUser = _unitOfWork.Users.GetEager(id);
            if (requestedUser == null)
                throw new UserNotFoundException("User with provided id was not found");

            return _mapper.Map<UserDto>(requestedUser);
        }
        public UserDto[] GetAllUsers()
        {
            return _mapper.Map<UserDto[]>(_unitOfWork.Users.GetAllEager());
        }
        public void UpdatePassword(Int32 userId, String oldPassword, String newPassword)
        {
            var requestedUser = _unitOfWork.Users.Get(userId);
            if (requestedUser == null)
                throw new UserNotFoundException("User with provided id was not found");

            var providedOldPasswordHash = JwtAuthMiddleware.CreatePasswordHash(oldPassword, _audienceConfig.PasswordSalt);
            if (!String.Equals(providedOldPasswordHash, requestedUser.PasswordHash))
                throw new IncorrectPasswordException($"The {nameof(oldPassword)} is wrong");

            requestedUser.PasswordHash = JwtAuthMiddleware.CreatePasswordHash(newPassword, _audienceConfig.PasswordSalt);
            _unitOfWork.Users.Update(requestedUser);
            _unitOfWork.Commit();
        }
        public async Task ChangeEmailAsync(Int32 userId, String newEmail)
        {
            var requestedUser = _unitOfWork.Users.GetEager(userId);
            if (requestedUser == null)
                throw new UserNotFoundException("User with provided id was not found");

            requestedUser.Email = newEmail;
            _unitOfWork.Users.Update(requestedUser);
            _unitOfWork.Commit();

            await _emailService.SendAsync(new EmailMessage()
            {
                Destination = requestedUser.Email,
                Subject = "Email successfully changed",
                Body = $"New Email {requestedUser.Email}"
            });
        }
        public void ChangeName(Int32 userId, String firstName, String lastName)
        {
            var requestedUser = _unitOfWork.Users.GetEager(userId);
            if (requestedUser == null)
                throw new UserNotFoundException("User with provided id was not found");

            requestedUser.FirstName = firstName;
            requestedUser.LastName = lastName;
            _unitOfWork.Users.Update(requestedUser);
            _unitOfWork.Commit();
        }
        public void AddRole(Int32 userId, String roleName)
        {
            var requestedUser = _unitOfWork.Users.GetEager(userId);
            if (requestedUser == null)
                throw new UserNotFoundException("User with provided id was not found");

            var requestedRole = _unitOfWork.Roles.GetByNameEager(roleName);
            if (requestedUser.UserRoles.Any(p => String.Equals(p.Role.RoleName, requestedRole.RoleName)))
                throw new UserRoleException($"User with provided id already have the \"{roleName}\" role");

            _unitOfWork.Users.AddRole(requestedUser.Id, requestedRole.Id);
            _unitOfWork.Commit();
        }
        public void RemoveRole(Int32 userId, String roleName)
        {
            var requestedUser = _unitOfWork.Users.GetEager(userId);
            if (requestedUser == null)
                throw new UserNotFoundException("User with provided id was not found");

            var requestedRole = _unitOfWork.Roles.GetByNameEager(roleName);
            var requestedUserRole = requestedUser.UserRoles.FirstOrDefault(p => String.Equals(p.Role.RoleName, requestedRole.RoleName));
            if (requestedUserRole == null)
                throw new UserRoleException($"User with provided id have no the \"{roleName}\" role");

            _unitOfWork.Users.RemoveRole(requestedUser.Id, requestedRole.Id);
            _unitOfWork.Commit();
        }
    }
}