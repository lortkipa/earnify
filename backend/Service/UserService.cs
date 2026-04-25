using AutoMapper;
using Dal;
using Data.Entities;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public interface IUserService
    {
        Task<UserDTO> GetByIdAsync(int id);
        Task<UserDTO> GetByGoogleIdAsync(string googleId);
        Task<UserDTO> GetByEmailAsync(string email);
        Task<UserDTO> CreateAsync(CreateUserDTO model);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var entity = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDTO>(entity);
        }

        public async Task<UserDTO> GetByGoogleIdAsync(string googleId)
        {
            var entity = await _userRepository.GetByGoogleIdAsync(googleId);
            return _mapper.Map<UserDTO>(entity);
        }

        public async Task<UserDTO> GetByEmailAsync(string email)
        {
            var entity = await _userRepository.GetByEmailAsync(email);
            return _mapper.Map<UserDTO>(entity);
        }
        public async Task<UserDTO> CreateAsync(CreateUserDTO model)
        {
            var entity = _mapper.Map<User>(model);
            entity = await _userRepository.AddAsync(entity);
            return _mapper.Map<UserDTO>(entity);
        }
    }
}
