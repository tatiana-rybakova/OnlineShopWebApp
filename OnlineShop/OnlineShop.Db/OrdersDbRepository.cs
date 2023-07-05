using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Db
{
    public class OrdersDbRepository : IOrdersRepository
    {
        private readonly DatabaseContext databaseContext;

        public OrdersDbRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await databaseContext.Orders.Include(x=>x.DeliveryInformation).Include(x=>x.Items).ThenInclude(x => x.Product).ToListAsync();
        }

        public async Task<Order> TryGetByIdAsync(Guid id)
        {
            return await databaseContext.Orders.Include(x => x.DeliveryInformation).Include(x => x.Items).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task AddAsync(List<CartItem> items, DeliveryInformation deliveryInformation)
        {
            var newOrder = new Order
            {
                Id = new Guid(),
                Items = items,
                DeliveryInformation = deliveryInformation,
                Date = DateTime.Now.ToString("dd MMMM yyyy"),
                Time = DateTime.Now.ToString("HH:mm:ss")
            };

            await databaseContext.Orders.AddAsync(newOrder);
            await databaseContext.SaveChangesAsync();
        }

        public async Task UpdateStateAsync(Guid orderId, OrderState newState)
        {
            var existingOrder = await TryGetByIdAsync(orderId);
            if (existingOrder != null)
            {
                existingOrder.State = newState;
                await databaseContext.SaveChangesAsync();
            }
        }
    }
}
