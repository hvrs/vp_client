using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public bool isPlus { get; set; }
    }

    public class DTOProductAndQuantity : INotifyPropertyChanged //Объект продукта в корзине 
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Product product { get; set;}
        public int quantityInWarehouse { get; set; }//Количество продукции на скалде

        private int quantituInBusket { get; set; }
        public int QuantityInBusket
        {
            get { return quantituInBusket; }
            set { if(quantituInBusket != value)
                     quantituInBusket = value; RaisePropertyChanged();
                }
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
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
    public class IdTransaction
    {
        public int ID { get; set;}
    }
    public class ProductAndQuantity
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
