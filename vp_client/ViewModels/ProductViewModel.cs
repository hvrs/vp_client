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
    public class ProductViewModel
    {
        #region Fields
        private INavigation navigation;
        static HttpClient httpClient = new HttpClient();

        public ObservableCollection<Product> productFromHttp;
        public ObservableCollection<Product> product;//??

        private Command<object> textChanged;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructors
        public ProductViewModel(INavigation _navigation)
        {
            navigation = _navigation;
            textChanged = new Command<object>(OnChanged);
            productFromHttp = httpClient.GetFromJsonAsync<ObservableCollection<Product>>("http://10.0.2.2:5125/api/Product").Result;
            Products = new ObservableCollection<Product>(productFromHttp);
        }

        private void OnChanged(object obj)
        {
            List<Product> TempFiltered = productFromHttp.Where(p => p.NameProduct.Contains(obj.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (var item in productFromHttp.ToList())
            {
                if (!TempFiltered.Contains(item))
                {
                    Products.Remove(item);
                }
                else
                {
                    if (!Products.Contains(item))
                    {
                        Products.Add(item);
                    }
                }
            }
        }


        #endregion

        #region Properties

        public ObservableCollection<Product> Products
        {
            get
            {
                return product;
            }
            set
            {
                if (product != value)
                {
                product = value;
                NotifyPropertyChanged();
                }               
            }
        }

        public Command<object> TextChanged
        {
            get { return textChanged; }
            set { textChanged = value; }
        }

        #endregion
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
