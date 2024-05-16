using vp_client.ViewModels;
namespace vp_client.Views;

public partial class QRPaymentPage : ContentPage
{
	public QRPaymentPage()
	{
		InitializeComponent();
		BindingContext = new qrViewModel();
	}
}