using vp_client.ViewModels;
namespace vp_client.Views;

public partial class ProductPage : ContentPage
{
	public ProductPage()
	{
		InitializeComponent();
		BindingContext = new ProductViewModel(this.Navigation);
	}
}