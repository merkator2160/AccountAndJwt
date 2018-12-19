using AccountAndJwt.Api.Database.Interfaces;
using AccountAndJwt.Api.Database.Models.Storage;
using AccountAndJwt.Api.Middleware.Config.Models;
using AccountAndJwt.Api.Services.Exceptions;
using AccountAndJwt.Api.Services.Interfaces;
using AccountAndJwt.Api.Services.Models;
using AccountAndJwt.Common.Helpers;
using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccountAndJwt.Api.Services
{
	internal class UserService : IUserService
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
				throw new LoginIsAlreadyUsedException("User name is already occupied");

			var userDb = _mapper.Map<UserDb>(user);
			_unitOfWork.Users.Add(userDb);
			await _unitOfWork.CommitAsync();
		}
		public async Task DeleteUserAsync(Int32 id)
		{
			var requestedUser = await _unitOfWork.Users.GetEagerAsync(id);
			if(requestedUser == null)
				throw new UserNotFoundException("User with provided id was not found");

			_unitOfWork.Users.Remove(requestedUser);
			await _unitOfWork.CommitAsync();
		}
		public async Task<UserDto> GetUserAsync(Int32 id)
		{
			var requestedUser = await _unitOfWork.Users.GetEagerAsync(id);
			if(requestedUser == null)
				throw new UserNotFoundException("User with provided id was not found");

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
				throw new UserNotFoundException("User with provided id was not found");

			var providedOldPasswordHash = KeyHelper.CreatePasswordHash(oldPassword, _audienceConfig.PasswordSalt);
			if(!String.Equals(providedOldPasswordHash, requestedUser.PasswordHash))
				throw new IncorrectPasswordException($"The {nameof(oldPassword)} is wrong");

			requestedUser.PasswordHash = KeyHelper.CreatePasswordHash(newPassword, _audienceConfig.PasswordSalt);
			_unitOfWork.Users.Update(requestedUser);
			await _unitOfWork.CommitAsync();
		}
		public async Task ChangeEmailAsync(Int32 userId, String newEmail)
		{
			var requestedUser = await _unitOfWork.Users.GetEagerAsync(userId);
			if(requestedUser == null)
				throw new UserNotFoundException("User with provided id was not found");

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
				throw new UserNotFoundException("User with provided id was not found");

			requestedUser.FirstName = firstName;
			requestedUser.LastName = lastName;
			_unitOfWork.Users.Update(requestedUser);
			await _unitOfWork.CommitAsync();
		}
		public async Task AddRoleAsync(Int32 userId, String roleName)
		{
			var requestedUser = await _unitOfWork.Users.GetEagerAsync(userId);
			if(requestedUser == null)
				throw new UserNotFoundException("User with provided id was not found");

			var requestedRole = await _unitOfWork.Roles.GetByNameEagerAsync(roleName);
			if(requestedUser.UserRoles.Any(p => String.Equals(p.Role.Name, requestedRole.Name)))
				throw new UserRoleException($"User with provided id already have the \"{roleName}\" role");

			await _unitOfWork.Users.AddRoleAsync(requestedUser.Id, requestedRole.Id);
			await _unitOfWork.CommitAsync();
		}
		public async Task RemoveRoleAsync(Int32 userId, String roleName)
		{
			var requestedUser = await _unitOfWork.Users.GetEagerAsync(userId);
			if(requestedUser == null)
				throw new UserNotFoundException("User with provided id was not found");

			var requestedRole = await _unitOfWork.Roles.GetByNameEagerAsync(roleName);
			var requestedUserRole = requestedUser.UserRoles.FirstOrDefault(p => String.Equals(p.Role.Name, requestedRole.Name));
			if(requestedUserRole == null)
				throw new UserRoleException($"User with provided id have no the \"{roleName}\" role");

			await _unitOfWork.Users.DeleteRoleAsync(requestedUser.Id, requestedRole.Id);
			await _unitOfWork.CommitAsync();
		}
	}
}