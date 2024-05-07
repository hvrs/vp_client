using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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


        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor
        public BusketViewModel()
        {
            productsFromHttp = httpClient.GetFromJsonAsync<ObservableCollection<DTOProductAndQuantity>>("http://10.0.2.2:5125/api/Busket").Result;



            ProductsInBasket = new ObservableCollection<DTOProductAndQuantity>(productsFromHttp);
            foreach (var i in ProductsInBasket)            
                Sum += i.product.Cost * i.quantityInBusket;


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
