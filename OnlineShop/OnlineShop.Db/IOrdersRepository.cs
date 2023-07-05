using OnlineShop.Db.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Db
{
    public interface IOrdersRepository
    {
        Task<List<Order>> GetAllAsync();
        Task AddAsync(List<CartItem> items, DeliveryInformation deliveryInformation);
        Task<Order> TryGetByIdAsync(Guid id);
        Task UpdateStateAsync(Guid orderId, OrderState newState);
    }
}
