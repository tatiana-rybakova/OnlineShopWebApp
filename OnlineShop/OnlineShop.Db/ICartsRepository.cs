using OnlineShop.Db.Models;
using System.Threading.Tasks;

namespace OnlineShop.Db
{
    public interface ICartsRepository
    {
        Task<Cart> TryGetByUserIdAsync(string userId);
        Task AddAsync(Product product, string userId);
        Task DeleteAsync(Product product, string userId);
        Task ClearAsync(string userId);
    }
}