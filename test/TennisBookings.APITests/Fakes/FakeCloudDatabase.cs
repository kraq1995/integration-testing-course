using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisBookings.Merchandise.Api.Data.Dto;
using TennisBookings.Merchandise.Api.External.Database;

namespace TennisBookings.APITests.Fakes
{
    public class FakeCloudDatabase : ICloudDatabase
    {
        private IReadOnlyCollection<ProductDto> _customDefaultProducts;

        public List<ProductDto> Products { get; set; }


        public FakeCloudDatabase(IReadOnlyCollection<ProductDto> products = null)
        {
            ReplaceCustomProducts(products);
            ResetDefaultProducts();
        }

        public Task<ProductDto> GetAsync(string id)
        {
            return Task.FromResult(Products.SingleOrDefault(p => p.Id.ToString() == id));
        }

        public Task InsertAsync(string id, ProductDto product)
        {
            Products.Add(product);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<ProductDto>> ScanAsync()
        {
            return Task.FromResult(Products as IReadOnlyCollection<ProductDto>);
        }

        public void ReplaceCustomProducts(IReadOnlyCollection<ProductDto> products)
        {
            _customDefaultProducts = products;
        }

        public void ResetDefaultProducts(bool useCustomIfAvailable = true)
        {
            Products = _customDefaultProducts is object && useCustomIfAvailable
                ? _customDefaultProducts.ToList()
                : GetDefaultProducts();
        }

        public static FakeCloudDatabase WithDefaultProducts()
        {
            var db = new FakeCloudDatabase();
            db.ResetDefaultProducts();
            return db;
        }

        private List<ProductDto> GetDefaultProducts() => new List<ProductDto>
        {
            new ProductDto
                {
                    Id = new Guid("6de90054-242a-434d-86cc-0d3cbc3cb01a"),
                    Name = "Ace T-Shirt (Mens)",
                    ShortDescription = "Always serve an ace with this amazing mens tennis T-shirt made from keep-cool fibres. Size: Small",
                    InternalReference = "SKU100",
                    Category = "Clothing",
                    Price = 29.90m,
                    Ratings = new [] { 4, 5, 4, 4, 5 },
                    IsEnabled = true,
                    StockCount = 1210
                },

                new ProductDto
                {
                    Id=new Guid("9b7a1993-481d-4877-a48e-a1565cd134ab"),
                    Name = "Ace T-Shirt (Ladies)",
                    ShortDescription = "Always serve an ace with this amazing ladies tennis T-shirt made from keep-cool fibres. Size: Small",
                    InternalReference = "SKU104",
                    Category = "Clothing",
                    Price = 29.90m,
                    Ratings = new [] { 4, 5, 4, 4, 5, 4, 4 },
                    IsEnabled = true,
                    StockCount = 576
                },

                new ProductDto
                {
                    Id = new Guid("a5cfffa4-47ed-4b6b-8eab-bf89de5b8fa0"),
                    Name = "Smashit Pro Racket",
                    ShortDescription = "Never miss a short with the patented, strike-perfect design used for this pro racket.",
                    InternalReference = "SKU200",
                    Category = "Rackets",
                    Price = 210.50m,
                    Ratings = new [] { 5, 5, 5, 5, 4, 5, 5, 4 },
                    IsEnabled = true,
                    StockCount = 19
                },

                new ProductDto
                {
                    Id = new Guid("1ef1ee8a-89f4-460f-8149-657149f07a79"),
                    Name = "Smashit Pro Racket (Super Advanced)",
                    ShortDescription = "Never miss a short with the patented, strike-perfect design used for this pro racket. This super advanced version also features max-speed strings.",
                    InternalReference = "SKU310",
                    Category = "Rackets",
                    Price = 399.99m,
                    IsEnabled = false
                },

               new ProductDto
                {
                    Id =new Guid("0e577d09-3659-4153-b8e3-24a5447a2c0e"),
                    Name = "Lightning Tennis Balls",
                    ShortDescription = "The long-lasting construction of these profession tennis balls means they are built to last. Pack of 4.",
                    InternalReference = "SKU050",
                    Category = "Equipment",
                    Price = 25.00m,
                    IsEnabled = true,
                    StockCount = 65
                }
        };
    }
}
