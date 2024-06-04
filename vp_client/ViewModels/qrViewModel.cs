using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using vp_client.Views;


namespace vp_client.ViewModels
{
    [QueryProperty(nameof(ImageQR), "QRImage")]
    [QueryProperty(nameof(TransactionID), "TransactionID")]
    public class qrViewModel : INotifyPropertyChanged
    {
        #region Fields
        private byte[] qrCode;
        private int transactionID;
        private Command<object> okCommand;
        private Command<object> cancelCommand;

        public event PropertyChangedEventHandler PropertyChanged;
        HttpClient httpClient = new HttpClient();
        #endregion

        #region Constructor
        public qrViewModel()
        {
            okCommand = new Command<object>(CompletedPurchase);
            cancelCommand = new Command<object>(CancelPurchase);
        }
        private async void CancelPurchase(object obj)//Отмена покупки полностью
        {
            using StringContent Content = new(
                   JsonSerializer.Serialize(new PurchaseStatus
                   {
                       transactionID = transactionID,
                       isCompleted = false
                   }),
                   Encoding.UTF8, "application/json");
            await httpClient.PostAsync("http://10.0.2.2:5125/api/Busket", Content);
            await Shell.Current.GoToAsync("//Basket");
        }
        
        private async void CompletedPurchase(object obj)
        {
            using StringContent Content = new(
                   JsonSerializer.Serialize(new PurchaseStatus
                   {
                       transactionID = transactionID,
                       isCompleted = true
                   }),
                   Encoding.UTF8, "application/json");
            await httpClient.PostAsync("http://10.0.2.2:5125/api/Busket", Content);

            await Shell.Current.GoToAsync("//Basket");
        }
        #endregion
        #region Properties
        public Command<object> OkCommand
        {
            get { return okCommand; }
            set { okCommand = value; }
        }
        public Command<object> CancelCommand
        {
            get { return cancelCommand; }
            set { cancelCommand = value; }
        }
        public int TransactionID
        {
            get { return transactionID; }
            set { transactionID = value; NotifyPropertyChanged(); }
        }

        public byte[] ImageQR
        {
            get { return qrCode; }
            set 
            { 
                qrCode = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class PurchaseStatus
    {
        public int transactionID { get; set; }
        public bool isCompleted { get; set; }
    }
}
