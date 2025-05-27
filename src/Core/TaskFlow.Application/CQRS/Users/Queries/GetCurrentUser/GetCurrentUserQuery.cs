using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.UserDTOs;

namespace TaskFlow.Application.CQRS.Users.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : IRequest<UserDto>
    {
        public int UserId { get; set; }

        public GetCurrentUserQuery(int userId)
        {
            UserId = userId;
        }
    }

}
