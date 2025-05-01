using Avalonia.Controls;
using Avalonia.Interactivity;

namespace RestaurantExpertSystem
{
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
        }

        private void OnStartClick(object? sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close(); // Close welcome window
        }
    }
}
