using System;
using System.Collections.Generic;
using System.Linq;
using ContosoPets.Api.Models;

namespace ContosoPets.Api.Data
{
    public static class SeedData
    {

        public static void Init(ContosoPetsContext context)
        {
            if (!context.Products.Any())
            {
                context.Products.AddRange(Products);
                context.SaveChanges();
            }
        }  
        static IList<Product> Products
        {
            get
            {
                return new List<Product>
                {
                    new Product
                    {
                        Name = "Squeaky Bone",
                        Price = 20.99m
                    },
                     new Product
                    {
                        Name = "Knotted Rope",
                        Price = 12.99m
                    }
                };
            }
        }
    }
}
