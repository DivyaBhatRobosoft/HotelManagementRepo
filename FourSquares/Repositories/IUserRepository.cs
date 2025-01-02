using FourSquares.Models;

namespace FourSquares.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
    }
}
