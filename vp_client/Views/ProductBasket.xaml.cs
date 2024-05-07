using vp_client.ViewModels;
namespace vp_client.Views;

public partial class ProductBasket : ContentPage
{
	public ProductBasket()
	{
		InitializeComponent();
		BindingContext = new BusketViewModel();
	}

    private void btn_back_Clicked(object sender, EventArgs e)
    {
        var newp = new ProductPage();
        Navigation.PushAsync(newp);
    }

}