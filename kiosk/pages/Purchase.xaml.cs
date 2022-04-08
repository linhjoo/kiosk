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

namespace kiosk.pages
{
    /// <summary>
    /// Purchase.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Purchase : Page
    {
        public Purchase()
        {
            InitializeComponent();
        }

        private void Btn_close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void BtnGoMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("pages/Main.xaml", UriKind.Relative));
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
