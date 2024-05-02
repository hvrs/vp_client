using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using vp_client.Models;
using vp_client.Views;

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
        private Command<object> tapCommand;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructors
        public ProductViewModel(INavigation _navigation)
        {
            navigation = _navigation;
            textChanged = new Command<object>(OnChanged);
            tapCommand = new Command<object>(toInfoPage);
            productFromHttp = httpClient.GetFromJsonAsync<ObservableCollection<Product>>("http://10.0.2.2:5125/api/Product").Result;
            Products = new ObservableCollection<Product>(productFromHttp);
        }

        private async void toInfoPage(object obj)//На страницу подробной информации о товаре
        {
            if (obj != null)
            {         
                /*var p = obj as Product;
                using StringContent jsonContent = new(
                    JsonSerializer.Serialize(new
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now),
                        Time = TimeOnly.FromDateTime(DateTime.Now),
                        ProductId = p.Id
                    }),
                    Encoding.UTF8, "application/json");
                await httpClient.PostAsJsonAsync("http://10.0.2.2:5125/api/View",jsonContent);*/

               var newPage = new ProductInfo(obj as Product);
               await Navigation.PushAsync(newPage);
            }
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

        public INavigation Navigation
        {
            get { return navigation; }
            set { navigation = value; }
        }

        public Command<object> TextChanged
        {
            get { return textChanged; }
            set { textChanged = value; }
        }
        public Command<object> TapCommand
        {
            get { return tapCommand; }
            set { tapCommand = value; }
        }

        #endregion
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
