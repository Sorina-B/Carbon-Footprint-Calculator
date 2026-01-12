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

        // --- NEW: Scroll Logic ---
        private async void OnCalculateClicked(object sender, EventArgs e)
        {
            // 1. Wait a small fraction of a second to ensure the ViewModel
            // has processed the calculation and set ResultVisible = true.
            await Task.Delay(100);

            // 2. Scroll the view to the ResultSection frame
            await PageScrollView.ScrollToAsync(ResultSection, ScrollToPosition.Center, true);
        }
    }
}