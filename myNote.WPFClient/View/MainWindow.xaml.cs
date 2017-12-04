using System.Windows;
using MahApps.Metro.Controls;

namespace myNote.WPFClient.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = IoC.IoC.WindowViewModel;
        }
    }
}
