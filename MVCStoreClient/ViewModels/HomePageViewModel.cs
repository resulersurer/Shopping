using CommunityToolkit.Mvvm.ComponentModel;
using MVCStoreClient.Models;
using MVCStoreClient.Services;
using System.Collections.ObjectModel;

namespace MVCStoreClient.ViewModels
{
    public partial class HomePageViewModel : ViewModel
    {

        public HomePageViewModel(
            IClientService clientService
            )
        {
            this.clientService = clientService;
        }

        [ObservableProperty]
        private ObservableCollection<Rayon> rayons = new();
        private readonly IClientService clientService;

        public override async Task Initialize()
        {
            Rayons = new(await clientService.GetRayons());
            base.Initialize();
        }
    }
}
