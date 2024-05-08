using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using vp_client.Models;

namespace vp_client.ViewModels
{
    class BusketViewModel
    {
        #region Fields
        private double sum;
        private ObservableCollection<DTOProductAndQuantity> productsInBasket;
        private ObservableCollection<DTOProductAndQuantity> productsFromHttp;
        HttpClient httpClient = new HttpClient();

        private Command<object> changedQuantity;
        private Command<object> deleteProductCommand;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor
        public BusketViewModel()
        {
            productsFromHttp = httpClient.GetFromJsonAsync<ObservableCollection<DTOProductAndQuantity>>("http://10.0.2.2:5125/api/Busket").Result;
            deleteProductCommand = new Command<object>(DeleteProduct);


            ProductsInBasket = new ObservableCollection<DTOProductAndQuantity>(productsFromHttp);
            foreach (var i in ProductsInBasket)            
                Sum += i.product.Cost * i.quantityInBusket;


        }
        
        private async void DeleteProduct(object obj)
        {            
            Sum -= (obj as DTOProductAndQuantity).product.Cost * (obj as DTOProductAndQuantity).quantityInBusket;
            ProductsInBasket.Remove(obj as  DTOProductAndQuantity);
            await httpClient.DeleteAsync($"http://10.0.2.2:5125/api/Busket/{(obj as DTOProductAndQuantity).product.Id}");

            //NotifyPropertyChanged();
        }
        #endregion

        #region Properties
        public ObservableCollection<DTOProductAndQuantity> ProductsInBasket
        {
            get { return productsInBasket; }
            set
            {
                if (productsInBasket != value)
                {
                    productsInBasket = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public Command<object> DeleteProductCommand
        {
            get { return deleteProductCommand; }
            set { deleteProductCommand = value; }
        }

        public Command<object> ChangedQuantity
        {
            get { return changedQuantity;}
            set { changedQuantity = value;}
        }
       
        public double Sum
        {
            get { return sum; }
            set
            {
                if (sum != value)
                {
                    sum = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion


        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
