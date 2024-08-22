using PersonalWebApp.Models;

namespace PersonalWebApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        void AddUser(User user);
        void UpdateUser(User user);
        void RemoveUser(User user);
        Task SaveChangesAsync();
    }
}
