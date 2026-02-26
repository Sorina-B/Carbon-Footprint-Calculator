using Microsoft.Maui.Controls;

namespace Proiect2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new CarbonViewModel();
        }

        private async void OnCalculateClicked(object sender, EventArgs e)
        {

            await Task.Delay(100);

            await PageScrollView.ScrollToAsync(ResultSection, ScrollToPosition.Center, true);
        }
    }
}
