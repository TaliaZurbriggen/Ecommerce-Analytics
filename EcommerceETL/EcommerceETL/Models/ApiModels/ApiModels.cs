using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceETL.Models.ApiModels
{
    public class ProductsResponse
    {
        public List<ApiProduct> Products { get; set; } = new();
    }

    public class ApiProduct
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal Rating { get; set; }
    }

    public class CartsResponse
    {
        public List<ApiCart> Carts { get; set; } = new();
    }

    public class ApiCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<ApiCartProduct> Products { get; set; } = new();
        public decimal Total { get; set; }
    }

    public class ApiCartProduct
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
    }

    public class UsersResponse
    {
        public List<ApiUser> Users { get; set; } = new();
    }

    public class ApiUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public ApiAddress Address { get; set; } = new();
    }

    public class ApiAddress
    {
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}
