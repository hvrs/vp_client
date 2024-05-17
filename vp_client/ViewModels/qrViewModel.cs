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
                   JsonSerializer.Serialize(transactionID),
                   Encoding.UTF8, "application/json");
            await httpClient.PostAsync("http://10.0.2.2:5125/api/Busket", Content);
        }
        
        private async void CompletedPurchase(object obj)
        {
            await httpClient.DeleteAsync("http://10.0.2.2:5125/api/Busket");
            await Shell.Current.GoToAsync("//MainPage");
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
}
