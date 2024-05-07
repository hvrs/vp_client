using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using vp_client.Models;
using vp_client.Views;

namespace vp_client.ViewModels
{
    public class InfoViewModel
    {
        #region Fields
        static HttpClient httpClient = new HttpClient();
        private Product _product;
        public event PropertyChangedEventHandler PropertyChanged;

        private Command<object> addToBusketCommand;


        //private INavigation navigation;
        #endregion
        #region Constructor
        public InfoViewModel(Product product)
        {
            Product = product;
            addToBusketCommand = new Command<object>(addProductToBusket);
            //backCommand = new Command<object>(toMainPage);
        }

        private async void addProductToBusket(object obj)//Добавление товара в корзину на сервере
        {
            if (_product != null)
            {              
                using StringContent Content = new(
                    JsonSerializer.Serialize(new ProductToB
                    {
                        ProductId = Convert.ToInt32(_product.Id)
                    }),
                    Encoding.UTF8, "application/json");
                await httpClient.PutAsync("http://10.0.2.2:5125/api/Busket", Content);
            }
        }
        #endregion
        #region Properties
        public Product Product
        {
            get { return _product; }
            set
            {
                 _product = value;  
            }
        }   

        public Command<object> AddToBusketCommand
        {
            get { return addToBusketCommand; }
            set { addToBusketCommand = value; }
        }
        #endregion

    }
}
