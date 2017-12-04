using myNote.WPFClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace myNote.WPFClient.View
{
    /// <summary>
    /// Логика взаимодействия для InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        public InputWindow(string property = "")
        {
            InitializeComponent();

            ViewModel = new InputWindowViewModel(property, this);
            DataContext = ViewModel;
        }
        /// <summary>
        /// Result of this dialog window
        /// </summary>
        public string Result
        {
            get
            {
                return this.DialogResult == true ? ViewModel.InputText : string.Empty;
            }
        }

        public InputWindowViewModel ViewModel { get; }
    }
}
