using MVCStoreClient.ViewModels;

namespace MVCStoreClient.Views;

public partial class HomePage
{
    public HomePage(
        HomePageViewModel homePageViewModel
        )
    {
        BindingContext = homePageViewModel;
        InitializeComponent();
    }
}