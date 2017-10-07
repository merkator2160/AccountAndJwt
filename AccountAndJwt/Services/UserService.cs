using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Models.Database;
using AccountAndJwt.Models.Exceptions;
using AccountAndJwt.Models.Service;
using AccountAndJwt.Services.Interfaces;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.Services
{
    internal class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;


        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
        }


        // IUserService ///////////////////////////////////////////////////////////////////////////
        public void Register(UserDto user)
        {
            var existedUser = _unitOfWork.Users.GetByLogin(user.Login);
            if (existedUser != null)
                throw new LoginIsAlreadyUsedException("User name is already occupied");

            var userDb = _mapper.Map<UserDb>(user);
            userDb.Id = Guid.NewGuid();
            _unitOfWork.Users.Add(userDb);
            _unitOfWork.Commit();
        }
        public void DeleteUser(String id)
        {
            var requestedUser = _unitOfWork.Users.Get(id);
            if (requestedUser == null)
                throw new UserNotFoundException("User with provided id was not found");

            _unitOfWork.Users.Remove(requestedUser);
            _unitOfWork.Commit();
        }
        public void UpdatePassword(String userId, String newPassword)
        {
            var requestedUser = _unitOfWork.Users.Get(userId);
            if (requestedUser == null)
                throw new UserNotFoundException("User with provided id was not found");

            requestedUser.Password = newPassword;
            _unitOfWork.Users.Update(requestedUser);
            _unitOfWork.Commit();
        }
        public async Task ChangeEmailAsync(String userId, String newEmail)
        {
            var requestedUser = _unitOfWork.Users.Get(userId);
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
        public void ChangeName(String userId, String firstName, String lastName)
        {
            var requestedUser = _unitOfWork.Users.Get(userId);
            if (requestedUser == null)
                throw new UserNotFoundException("User with provided id was not found");

            requestedUser.FirstName = firstName;
            requestedUser.LastName = lastName;
            _unitOfWork.Users.Update(requestedUser);
            _unitOfWork.Commit();
        }
    }
}