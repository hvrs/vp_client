using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vp_client.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string NameProduct { get; set; } = "";
        public byte[]? Photo { get; set; }
        public string Category { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string? Nicotine { get; set; }
        public string? Strength { get; set; }
        public double Cost { get; set; }

    }
    public class ProductToB
    {
        public int ProductId { get; set; }
    }

    public class DTOProductAndQuantity//Объект продукта в корзине
    {
        public Product product { get; set;}
        public int quantityInBusket { get; set; }
    }


    
    //Классы для передачи данных о производимой транзакции на сервер
    public class idProductsInBasketAndSum
    {
        public List<ProductAndQuantity> productQ { get; set; }
        public double Sum { get; set; }
        public idProductsInBasketAndSum()
        {
            productQ = new List<ProductAndQuantity>();
        }
    }
    public class ProductAndQuantity
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
