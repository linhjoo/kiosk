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
    /// LogIn.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogIn : Page
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void btn_Cancel_Click(object sender, MouseButtonEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            controllers.TextHelper textHelper = new controllers.TextHelper();
            controllers.Encrypt encrypt = new controllers.Encrypt();

            string email = tb_email.Text;
            string password = pb_pwd.Password;
            List<string[]> userInfo = new List<string[]>();


            try
            {
                foreach (string line in System.IO.File.ReadLines(@"C:/HBT_Foot_Scanner/config/account.txt"))
                {
                    userInfo.Add(textHelper.SplitString(line, ','));
                }
            }
            catch
            {
                Console.WriteLine("계정정보 파일이 존재하지 않습니다.");
            }

            foreach (string[] user in userInfo)
            {
                if (user[0].Equals(email))
                {
                    if (encrypt.ConvertToSimpleDecoding(user[1]) == password)
                    {
                        App.loginStatus = true;
                        App.userName = user[2];
                        NavigationService.Navigate(new Uri("pages/Main.xaml", UriKind.Relative));
                    }
                    else
                    { 
                        tb_error.Text = "비밀번호가 일치하지 않습니다.";
                    }
                    break;
                }
                else
                {
                    tb_error.Text = "해당 이메일에 대한 가입 정보가 존재하지 않습니다.";
                }
            }

        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
