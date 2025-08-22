using AuthenticatioApi.Application.Interfaces;
using AuthenticatioApi.Core.Entities;
using AuthenticatioApi.Core.Entities.Enumrators;
using AuthenticatioApi.Core.Models;
using AuthenticatioApi.Infra.Data.Interfaces;
using AutoMapper;

namespace AuthenticatioApi.Application.Services
{
    internal class UserAppService(IMapper mapper, IUserRepository userRepository) 
        : IUserAppService
    {         
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _userRepository = userRepository;        

        public async Task<UserModel?> GetAsync(int userId, RecordStatusEnum recordStatus)
        {
            var entity = await _userRepository.GetAsync(userId, recordStatus);
            if (entity == null) return null;

            return _mapper.Map<UserModel>(entity);
        }

        public async Task<int> InsertAsync(int inclusionUserId, UserModel model)
        {
            var entity = _mapper.Map<User>(model);
            entity.InclusionDate = DateTime.UtcNow; 
            entity.InclusionUserId = inclusionUserId;
            entity.Status = 1;
            var response = await _userRepository.AddAsync(entity);
            
            return response.Id;
        }
    }
}
