using vp_client.Models;
using vp_client.ViewModels;
namespace vp_client.Views;

public partial class ProductInfo : ContentPage
{
	public ProductInfo(Product product)
	{
		InitializeComponent();
		BindingContext = new InfoViewModel(product);
	}

    private void btn_back_Clicked(object sender, EventArgs e)
    {
		var newp = new ProductPage();
		Navigation.PushAsync(newp);
    }
}