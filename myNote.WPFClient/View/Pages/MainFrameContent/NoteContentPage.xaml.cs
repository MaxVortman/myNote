using myNote.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace myNote.WPFClient.View.Pages.MainFrameContent
{
    /// <summary>
    /// Логика взаимодействия для NoteContentPage.xaml
    /// </summary>
    public partial class NoteContentPage : Page
    {
        public NoteContentPage(Note note)
        {
            InitializeComponent();

            DataContext = new NoteContentViewModel(note);
        }
    }
}
