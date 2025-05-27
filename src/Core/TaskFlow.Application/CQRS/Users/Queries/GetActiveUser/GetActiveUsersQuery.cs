using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.UserDTOs;

namespace TaskFlow.Application.CQRS.Users.Queries.GetActiveUser
{
    public class GetActiveUsersQuery : IRequest<List<UserDto>>
    {
    }
}
