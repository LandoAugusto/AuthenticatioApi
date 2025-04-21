using AuthenticatioApi.Application.Interfaces;
using AuthenticatioApi.Core.Entities.Enumrators;
using AuthenticatioApi.Core.Models;
using AuthenticatioApi.Infra.Data.Interfaces;
using AutoMapper;

namespace AuthenticatioApi.Application.Services
{
    internal class UserAppService(IMapper mapper, IUserRepository userRepository) : IUserAppService
    {         
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _userRepository = userRepository;        

        public async Task<UserModel?> GetAsync(int userId, RecordStatusEnum recordStatus)
        {
            var entidade = await _userRepository.GetAsync(userId, recordStatus);
            if (entidade == null) return null;

            return _mapper.Map<UserModel>(entidade);
        }
    }
}
