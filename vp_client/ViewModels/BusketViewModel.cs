using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using vp_client.Models;
using vp_client.Views;

namespace vp_client.ViewModels
{
    public class BusketViewModel : INotifyPropertyChanged
    {
        #region Fields
        private INavigation navigation;
        private double sum;
        private ObservableCollection<DTOProductAndQuantity> productsInBasket;
#nullable enable
        private ObservableCollection<DTOProductAndQuantity>? productsFromHttp;
#nullable disable
        HttpClient httpClient = new HttpClient();

        private Command<object> changedQuantity;
        private Command<object> deleteProductCommand;
        private Command<object> makePurchaseCommand;
        private Command<object> minusProductCommand;
        private Command<object> plusProductCommand;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor
        public BusketViewModel()
        {
            productsFromHttp = httpClient.GetFromJsonAsync<ObservableCollection<DTOProductAndQuantity>>("http://10.0.2.2:5125/api/Busket").Result;
            deleteProductCommand = new Command<object>(DeleteProduct);
            makePurchaseCommand = new Command<object>(Purchase);
            minusProductCommand = new Command<object>(ProductMinus);
            plusProductCommand = new Command<object>(ProductPlus);

            ProductsInBasket = new ObservableCollection<DTOProductAndQuantity>(productsFromHttp);
            foreach (var i in ProductsInBasket)            
                Sum += i.product.Cost * i.QuantityInBusket;


        }
        private async void ProductMinus(object product)
        {
            var foundItem = ProductsInBasket.FirstOrDefault(i => i.product.Id == (product as DTOProductAndQuantity).product.Id);
            if (foundItem != null && foundItem.QuantityInBusket > 1)
            {                
                foundItem.QuantityInBusket--;
                Sum -= (product as DTOProductAndQuantity).product.Cost;
                using StringContent content = new(
                    JsonSerializer.Serialize(new ProductToB
                    {
                        ProductId = foundItem.product.Id,
                        isPlus = false
                    }),
                    Encoding.UTF8, "application/json");
                await httpClient.PutAsync("http://10.0.2.2:5125/api/Busket", content);

            }
            else if (foundItem.QuantityInBusket ==1)
            {
                DeleteProduct(foundItem);
            }                   
        }
        private async void ProductPlus(object product)
        {
            var foundItem = ProductsInBasket.FirstOrDefault(i=>i.product.Id == (product as DTOProductAndQuantity).product.Id);
            if (foundItem != null && foundItem.QuantityInBusket <= foundItem.quantityInWarehouse)
            {
                foundItem.QuantityInBusket++;
                Sum += (product as DTOProductAndQuantity).product.Cost;
                using StringContent content = new(
                    JsonSerializer.Serialize(new ProductToB
                    {
                        ProductId = foundItem.product.Id,
                        isPlus = true
                    }),
                    Encoding.UTF8, "application/json");
                await httpClient.PutAsync("http://10.0.2.2:5125/api/Busket", content);
            }
        }

        private async void Purchase(object obj)
        {
            if (sum >0 && productsInBasket is not null)
            {
                idProductsInBasketAndSum basketAndSum = new idProductsInBasketAndSum();
                basketAndSum.Sum = sum;
                foreach (var item in ProductsInBasket)
                {
                    ProductAndQuantity _product = new ProductAndQuantity();
                    _product.ProductID = item.product.Id;
                    _product.Quantity = item.QuantityInBusket;

                    basketAndSum.productQ.Add(_product);
                }
                using StringContent Content = new(
                    JsonSerializer.Serialize(basketAndSum),
                    Encoding.UTF8, "application/json");
                var responce = await httpClient.PutAsync("http://10.0.2.2:5125/api/Qr", Content);
                string idTransaction = await responce.Content.ReadAsStringAsync();
                int idt = Convert.ToInt32(idTransaction);
                ImageDto imageDto = new ImageDto();
                imageDto = await httpClient.GetFromJsonAsync<ImageDto>($"http://10.0.2.2:5125/api/Qr/{sum}");


                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    {"QRImage", imageDto.image },
                    {"TransactionID", idt }
                };

                await Shell.Current.GoToAsync("//QR", data);

            }
        }

        private async void DeleteProduct(object obj)
        {            
            Sum -= (obj as DTOProductAndQuantity).product.Cost * (obj as DTOProductAndQuantity).QuantityInBusket;
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
        public Command<object> PlusProductCommand
        {
            get { return plusProductCommand; }
            set { plusProductCommand = value; }
        }
        public Command<object> MinusProductCommand
        {
            get { return minusProductCommand; }
            set { minusProductCommand = value; }
        }
        public Command<object> MakePurchaseCommand
        {
            get { return makePurchaseCommand; }
            set { makePurchaseCommand = value; }
        }

        public Command<object> DeleteProductCommand
        {
            get { return deleteProductCommand; }
            set { deleteProductCommand = value; }
        }
        public INavigation Navigation
        {
            get { return navigation; }
            set { navigation = value; }
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
    public class ImageDto//класс для получения изображения QR-кода с сервера
    {
#nullable enable
        public byte[]? image { get; set; }
#nullable disable
    }
}
