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
    /// Join.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Join : Page
    {
        public Join()
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

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            controllers.TextHelper textHelper = new controllers.TextHelper();
            controllers.Encrypt encrypt = new controllers.Encrypt(); // 암호화 클래스

            List<string[]> userInfo = new List<string[]>();
            foreach (string line in System.IO.File.ReadLines(@"C:/HBT_Foot_Scanner/config/account.txt"))
            {
                userInfo.Add(textHelper.SplitString(line, ','));
            }

            string email = tb_email.Text;
            string password = tb_pw.Password;
            string password2 = tb_pw2.Password;
            string name = tb_name.Text;
            string phoneNumber = tb_phonenumber.Text;

            // 유효성 검사
            if (email == "" || password == "" || password2 == "" || name == "" || phoneNumber == "")
            {
                tb_Error.Text = "입력되지 않은 항목이 있습니다.";
                return;
            }
            if (!textHelper.IsValidMailAddress(email))
            {
                tb_Error.Text = "email 주소가 유효하지 않습니다.";
                return;
            }
            if (password != password2)
            {
                tb_Error.Text = "password가 일치하지 않습니다.";
                return;
            }
            foreach (string[] user in userInfo)
            {
                if(user[0] == email)
                {
                    tb_Error.Text = "해당 email 주소로 가입된 정보가 이미 존재합니다.";
                    return;
                }
            }
                        
            string encryptedPassword = encrypt.ConvertToSimpleEncoding(password); // password 암호화

            string temp = $"{email}, {encryptedPassword}, {name}, {phoneNumber}\n";
            try
            {
                textHelper.WriteStringToFile(@"C:/HBT_Foot_Scanner/config/account.txt", temp);
            }
            catch
            {
                Console.WriteLine("계정관리 파일이 없습니다.");
            }

            NavigationService.Navigate(new Uri("pages/Main.xaml", UriKind.Relative));
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
