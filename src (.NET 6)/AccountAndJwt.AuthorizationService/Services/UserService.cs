using AccountAndJwt.AuthorizationService.Middleware.Config;
using AccountAndJwt.AuthorizationService.Services.Interfaces;
using AccountAndJwt.AuthorizationService.Services.Models;
using AccountAndJwt.AuthorizationService.Services.Models.Exceptions;
using AccountAndJwt.Common.Helpers;
using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Database.Models.Storage;
using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccountAndJwt.AuthorizationService.Services
{
	internal sealed class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IEmailService _emailService;
		private readonly AudienceConfig _audienceConfig;


		public UserService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, AudienceConfig audienceConfig)
		{
			_audienceConfig = audienceConfig;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_emailService = emailService;
		}


		// IUserService ///////////////////////////////////////////////////////////////////////////
		public async Task RegisterAsync(UserDto user)
		{
			var requestedUser = await _unitOfWork.Users.GetByLoginEagerAsync(user.Login);
			if(requestedUser != null)
				throw new LoginIsAlreadyUsedException("User name is already occupied!");

			var userDb = _mapper.Map<UserDb>(user);
			userDb.PasswordHash = KeyHelper.CreatePasswordHash(user.Password, _audienceConfig.PasswordSalt);

			await _unitOfWork.Users.AddAsync(userDb);
			await _unitOfWork.CommitAsync();
		}
		public async Task DeleteUserAsync(Int32 id)
		{
			var requestedUser = await _unitOfWork.Users.GetEagerAsync(id);
			if(requestedUser == null)
				throw new UserNotFoundException($"User with provided id: \"{id}\" was not found!");

			_unitOfWork.Users.Remove(requestedUser);
			await _unitOfWork.CommitAsync();
		}
		public async Task<UserDto> GetUserAsync(Int32 id)
		{
			var requestedUser = await _unitOfWork.Users.GetEagerAsync(id);
			if(requestedUser == null)
				throw new UserNotFoundException($"User with provided id: \"{id}\" was not found!");

			return _mapper.Map<UserDto>(requestedUser);
		}
		public async Task<UserDto[]> GetAllUsersAsync()
		{
			return _mapper.Map<UserDto[]>(await _unitOfWork.Users.GetAllEagerAsync());
		}
		public async Task UpdatePasswordAsync(Int32 userId, String oldPassword, String newPassword)
		{
			var requestedUser = await _unitOfWork.Users.GetAsync(userId);
			if(requestedUser == null)
				throw new UserNotFoundException($"User with provided id: \"{userId}\" was not found!");

			var providedOldPasswordHash = KeyHelper.CreatePasswordHash(oldPassword, _audienceConfig.PasswordSalt);
			if(!String.Equals(providedOldPasswordHash, requestedUser.PasswordHash))
				throw new IncorrectPasswordException($"The {nameof(oldPassword)} is wrong!");

			requestedUser.PasswordHash = KeyHelper.CreatePasswordHash(newPassword, _audienceConfig.PasswordSalt);
			_unitOfWork.Users.Update(requestedUser);
			await _unitOfWork.CommitAsync();
		}
		public async Task ChangeEmailAsync(Int32 userId, String newEmail)
		{
			var requestedUser = await _unitOfWork.Users.GetEagerAsync(userId);
			if(requestedUser == null)
				throw new UserNotFoundException($"User with provided id: \"{userId}\" was not found!");

			requestedUser.Email = newEmail;
			_unitOfWork.Users.Update(requestedUser);
			await _unitOfWork.CommitAsync();

			await _emailService.SendAsync(new EmailMessage()
			{
				Destination = requestedUser.Email,
				Subject = "Email successfully changed",
				Body = $"New Email {requestedUser.Email}"
			});
		}
		public async Task ChangeNameAsync(Int32 userId, String firstName, String lastName)
		{
			var requestedUser = await _unitOfWork.Users.GetEagerAsync(userId);
			if(requestedUser == null)
				throw new UserNotFoundException($"User with provided id: \"{userId}\" was not found!");

			requestedUser.FirstName = firstName;
			requestedUser.LastName = lastName;
			_unitOfWork.Users.Update(requestedUser);
			await _unitOfWork.CommitAsync();
		}
		public async Task AddRoleAsync(Int32 userId, String roleName)
		{
			var requestedUser = await _unitOfWork.Users.GetEagerAsync(userId);
			if(requestedUser == null)
				throw new UserNotFoundException($"User with provided id: \"{userId}\" was not found!");

			var requestedRole = await _unitOfWork.Users.GetRoleWithUserAsync(roleName);
			if(requestedUser.UserRoles.Any(p => String.Equals(p.Role.Name, requestedRole.Name)))
				throw new UserRoleException($"User with provided id already have the \"{roleName}\" role!");

			await _unitOfWork.Users.AddRoleAsync(requestedUser.Id, requestedRole.Id);
			await _unitOfWork.CommitAsync();
		}
		public async Task RemoveRoleAsync(Int32 userId, String roleName)
		{
			var requestedUser = await _unitOfWork.Users.GetEagerAsync(userId);
			if(requestedUser == null)
				throw new UserNotFoundException($"User with provided id: \"{userId}\" was not found!");

			var requestedRole = await _unitOfWork.Users.GetRoleWithUserAsync(roleName);
			var requestedUserRole = requestedUser.UserRoles.FirstOrDefault(p => String.Equals(p.Role.Name, requestedRole.Name));
			if(requestedUserRole == null)
				throw new UserRoleException($"User with provided id have no the \"{roleName}\" role");

			await _unitOfWork.Users.DeleteRoleAsync(requestedUser.Id, requestedRole.Id);
			await _unitOfWork.CommitAsync();
		}
	}
}