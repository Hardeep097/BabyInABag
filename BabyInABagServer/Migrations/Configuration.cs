namespace BabyInABagServer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using BabyInABagServer.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<BabyInABagServer.Services.Context>
    {
        DateTime value = new DateTime(2017, 1, 18);
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BabyInABagServer.Services.Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.

            context.Admins.AddOrUpdate(
                a => a.Admin_Id,
                new Admin { Admin_Id = 3, Full_Name = "Mathew Glinka", Username = "glinka", Password = "B1132C41BB7AED679775E29F1B419AE63A245099605511B0C6568AED9D682E9D", Salt = "glinka", Enabled = true }
                );

            context.Customers.AddOrUpdate(
                c => c.Customer_Id,
                new Customer { Customer_Id = 1, First_Name = "Hardeep", Last_Name = "Singh", Customer_Phone = "9876543234", Customer_Email = "Hardeep@gmail.com", Username = "hsingh", Password = "8D8191421DA7E3E10A3D43F5060E27FC91BB43501D915EBA88D99D57C25C7BAA", Salt = "hsingh" },
                new Customer { Customer_Id = 2, First_Name = "Jordan", Last_Name = "Grace", Customer_Phone = "6471172941", Customer_Email = "Grace@gmail.com", Username = "grace", Password = "BFA5F558D6520C49F781454D334E5782D14E35CBEAB4003EF843A3BFAA31442C", Salt = "grace" }
                );

            context.Orders.AddOrUpdate(
                o => o.Order_Id,
               new Order { Order_Id = 1, Order_Date_Placed = value, Order_Date_Paid = value, Order_Details = "Blankets", Order_Status = order_status.InProgress, Customer_Id = 1 },
               new Order { Order_Id = 2, Order_Date_Placed = value, Order_Date_Paid = value, Order_Details = "Bag blanket", Order_Status = order_status.InProgress, Customer_Id = 2 }
               );

            context.Products.AddOrUpdate(
                p => p.Product_Id,
                new Product { Product_Id = 1, Product_Description = "Blanket Bag white strip", Product_Name = "Blanket Bag", Product_Price = 55, Size = "Small", Active = true, Product_Image = "root", Product_Category_Id = 1 }
                );

            context.ProductCategories.AddOrUpdate(
                pc => pc.Product_Category_Id,
                new ProductCategory { Product_Category_Id = 1, Product_Category = "Blanket",Default_Image= "/images/blueb180237469.jpg", Default_Price=50},
                new ProductCategory { Product_Category_Id = 2, Product_Category = "Boots", Default_Image = "/images/bubblesprite180347444.jpg", Default_Price = 40 },
                new ProductCategory { Product_Category_Id = 3, Product_Category = "Sleeping Bag", Default_Image = "/images/bluesky180530225.jpg", Default_Price = 60 }
                );
        }
    }
}
