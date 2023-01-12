﻿using System.Collections.Generic;

namespace Basket.Api.Entities
{
    public class ShoppingCart
    {
        public string _userName { get; set; }
        public ShoppingCart()
        {

        }
        public ShoppingCart(string userName)
        {
            _userName = userName; 
        }

        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
    
        public decimal TotalPrice { get
            { 
                decimal totalPrice = 0;
                foreach(var item in Items)
                {
                    totalPrice += item.Price * item.Quantity;
                }
                return totalPrice;
            } 
        }
    }
}
