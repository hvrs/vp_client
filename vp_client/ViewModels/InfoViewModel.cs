using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using vp_client.Models;

namespace vp_client.ViewModels
{
    public class InfoViewModel
    {
        #region Fields
        private Product _product;
        public event PropertyChangedEventHandler PropertyChanged;
        //private INavigation navigation;
        #endregion
        #region Constructor
        public InfoViewModel(Product product)
        {
            Product = product;
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
        #endregion
        
    }
}
