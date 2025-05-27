using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.CQRS.Users.Queries.GetActiveUser
{
    public class GetActiveUsersQueryHandler : IRequestHandler<GetActiveUsersQuery, List<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetActiveUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetActiveUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetActiveUsersAsync();
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}
