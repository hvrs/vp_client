using vp_client.ViewModels;
namespace vp_client.Views;

public partial class ProductBasket : ContentPage
{
    public BusketViewModel busketViewModel { get; set; }
    public ProductBasket()
	{
        busketViewModel = new BusketViewModel();
		InitializeComponent();
        BindingContext = busketViewModel;

    }

    private void btn_back_Clicked(object sender, EventArgs e)
    {
        var newp = new ProductPage();
        Navigation.PushAsync(newp);
    }

}