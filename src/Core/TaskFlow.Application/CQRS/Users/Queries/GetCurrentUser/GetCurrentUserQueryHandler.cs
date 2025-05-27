using AutoMapper;
using MediatR;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.CQRS.Users.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetCurrentUserQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

    }
}
