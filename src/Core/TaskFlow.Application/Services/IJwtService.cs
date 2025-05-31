using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
    }
}
