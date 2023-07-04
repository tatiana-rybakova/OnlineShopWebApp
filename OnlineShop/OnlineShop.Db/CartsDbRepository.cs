using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Db
{
    public class CartsDbRepository : ICartsRepository
    {
        private readonly DatabaseContext databaseContext;

        public CartsDbRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }        
        public async Task<Cart> TryGetByUserIdAsync(string userId)
        {
            return await databaseContext.Carts.Include(x => x.Items).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task AddAsync(Product product, string userId)
        {
            var existingCart = await TryGetByUserIdAsync(userId);
            if (existingCart == null)
            {
                var newCart = new Cart
                {
                    UserId = userId,
                };
                newCart.Items = new List<CartItem>
                    {
                        new CartItem
                        {
                            Amount =1,
                            Product = product,
                            Cart = newCart
                        }
                    };

                await databaseContext.Carts.AddAsync(newCart);
            }
            else
            {
                var existingCartItem = existingCart.Items.FirstOrDefault(x => x.Product.Id == product.Id);
                if (existingCartItem != null)
                {
                    existingCartItem.Amount += 1;
                }
                else
                {
                    existingCart.Items.Add(new CartItem
                    {
                        Amount = 1,
                        Product = product,
                        Cart = existingCart
                    });
                }
            }
            await databaseContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product, string userId)
        {
            var existingCart = await TryGetByUserIdAsync(userId);
            var existingCartItem = existingCart.Items.FirstOrDefault(x => x.Product.Id == product.Id);
            if (existingCartItem.Amount > 1)
            {
                existingCartItem.Amount -= 1;
            }
            else
            {
                existingCart.Items.Remove(existingCartItem);
            }
            await databaseContext.SaveChangesAsync();
        }


        public async Task ClearAsync(string userId)
        {
            var cart = await TryGetByUserIdAsync(userId);
            databaseContext.Carts.Remove(cart);
            await databaseContext.SaveChangesAsync();
        }
    }
}
