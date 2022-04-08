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
    /// Main.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Main : Page
    {
        public Main()
        {
            InitializeComponent();

            BtnLogOut.Visibility = Visibility.Collapsed;

            ///** MediaPlayer */
            //media_player.Visibility = Visibility.Collapsed;
            //media_player.Source = new Uri(PATH_MAIN_VIDEO, UriKind.RelativeOrAbsolute);
            //media_player.MediaFailed += media_player_MediaFailed;
            //media_player.MediaEnded += media_player_MediaEnded;
            //media_player.MediaOpened += media_player_MediaOpened;
            //media_player.Play();

            if(App.loginStatus == false)
            {
                SetLogout();
            }
            else
            {
                SetLogin();
            }
        }

        void media_player_MediaOpened(object sender, RoutedEventArgs e)
        {
            //ControlHelper.FadeIn(media_player, 1500);
        }

        private void media_player_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            //Console.WriteLine("Media Player Error: " + e.ErrorException.Message + "\nPath: " + PATH_MAIN_VIDEO);
        }

        private void media_player_MediaEnded(object sender, RoutedEventArgs e)
        {
            //ControlHelper.FadeOut(media_player, 1000);
            //ControlHelper.FadeIn(media_player, 1000);
            media_player.Position = TimeSpan.FromSeconds(0);
            media_player.Play();
        }

        private void btn_3DScan_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("pages/Scan.xaml", UriKind.Relative));
        }

        private void btn_Cancel_Click(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("pages/LogIn.xaml", UriKind.Relative));
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            SetLogout();
        }

        private void BtnJoin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("pages/Join.xaml", UriKind.Relative));
        }

        private void SetLogout()
        {
            BtnLogIn.Visibility = Visibility.Visible;
            BtnJoin.Visibility = Visibility.Visible;
            BtnLogOut.Visibility = Visibility.Collapsed;
            BtnFootScan.Visibility = Visibility.Collapsed;
            tb_welcome.Text = $"회원등록 후 사용 가능합니다!\n아이디가 있다면 로그인해주세요!";

            App.loginStatus = false;
            App.userName = "";
        }

        private void SetLogin()
        {
            BtnLogIn.Visibility = Visibility.Collapsed;
            BtnJoin.Visibility = Visibility.Collapsed;
            BtnLogOut.Visibility = Visibility.Visible;
            BtnFootScan.Visibility = Visibility.Visible;
            tb_welcome.Text = $"{App.userName}님 환영합니다!";
        }
    }
}
