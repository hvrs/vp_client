using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using vp_client.Views;


namespace vp_client.ViewModels
{
    [QueryProperty(nameof(ImageQR), "QRImage")]
    public class qrViewModel : INotifyPropertyChanged
    {
        #region Fields
        private byte[] qrCode;
        private Command<object> okCommand;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor
        public qrViewModel()
        {
            okCommand = new Command<object>(getImage);

            
            


        }

        private async void getImage(object obj)
        {
            

        }
        #endregion
        #region Properties
        public Command<object> OkCommand
        {
            get { return okCommand; }
            set { okCommand = value; }
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
