using System;
using System.IO;
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
using HelixToolkit.Wpf;

namespace kiosk.pages
{
    /// <summary>
    /// ScanResult.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ScanResult : Page
    {
        enum ViewMode
        {
            VIEWMODE_3D,
            VIEWMODE_2D
        }

        controllers.TextHelper textHelper = new controllers.TextHelper();

        public ScanResult()
        {
            InitializeComponent();

            LoadResultData();

            Init3DViewer();
            Init2DViewer();

            SetViewerVisiblity(ViewMode.VIEWMODE_3D);
        }

        private void Init3DViewer()
        {
            ModelImporter importer1 = new ModelImporter();

            System.Windows.Media.Media3D.Model3DGroup m3DGroup1 = importer1.Load(@"C:\HBT_Foot_Scanner\Result\L_foot_mesh.ply");
            System.Windows.Media.Media3D.Model3DGroup m3DGroup2 = importer1.Load(@"C:\HBT_Foot_Scanner\Result\R_foot_mesh.ply");

            System.Windows.Media.Media3D.Material mat = MaterialHelper.CreateMaterial(
            new SolidColorBrush(Color.FromRgb(238, 230, 196)));

            foreach (System.Windows.Media.Media3D.GeometryModel3D geometryModel in m3DGroup1.Children)
            {
                geometryModel.Material = mat;
                geometryModel.BackMaterial = mat;
            }
            foreach (System.Windows.Media.Media3D.GeometryModel3D geometryModel in m3DGroup2.Children)
            {
                geometryModel.Material = mat;
                geometryModel.BackMaterial = mat;
            }

            model1.Content = m3DGroup1;
            model2.Content = m3DGroup2;
        }

        private void Init2DViewer()
        {
            var bitmapL = new BitmapImage();
            var stream = File.OpenRead(@"C:\HBT_Foot_Scanner\Result\Result_RGB_L_foot.png");

            bitmapL.BeginInit();
            bitmapL.CacheOption = BitmapCacheOption.OnLoad;
            bitmapL.StreamSource = stream;
            bitmapL.EndInit();
            stream.Close();
            stream.Dispose();

            FootViewerImgL.Source = bitmapL;

            var bitmapR = new BitmapImage();
            stream = File.OpenRead(@"C:\HBT_Foot_Scanner\Result\Result_RGB_R_foot.png");

            bitmapR.BeginInit();
            bitmapR.CacheOption = BitmapCacheOption.OnLoad;
            bitmapR.StreamSource = stream;
            bitmapR.EndInit();
            stream.Close();
            stream.Dispose();

            FootViewerImgL.Source = bitmapL;
            FootViewerImgR.Source = bitmapR;
        }


        private void SetViewerVisiblity(ViewMode mode)
        {
            if (mode == ViewMode.VIEWMODE_3D)
            {
                FootViewer3DL.Visibility = Visibility.Visible;
                FootViewer3DR.Visibility = Visibility.Visible;
                FootViewerImgL.Visibility = Visibility.Hidden;
                FootViewerImgR.Visibility = Visibility.Hidden;
            }
            else
            {
                FootViewer3DL.Visibility = Visibility.Hidden;
                FootViewer3DR.Visibility = Visibility.Hidden;
                FootViewerImgL.Visibility = Visibility.Visible;
                FootViewerImgR.Visibility = Visibility.Visible;
            }
        }

        private void Rescan_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
                FootViewerImgL.Source = null;
                FootViewerImgR.Source = null;
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("pages/Disease.xaml", UriKind.Relative));
        }

        private void Cancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
                FootViewerImgL.Source = null;
                FootViewerImgR.Source = null;
            }
        }

        private void LoadResultData()
        {
            IniFile ini = new IniFile();
            ini.Load(@"C:\HBT_Foot_Scanner\Result\measurements.ini");
            
            L_foot_length.Text = string.Format("{0:0.0}mm", double.Parse(ini["FOOT_SIZE"]["L_foot length"].ToString().Replace(" mm", "")));
            R_foot_length.Text = string.Format("{0:0.0}mm", double.Parse(ini["FOOT_SIZE"]["R_foot length"].ToString().Replace(" mm", "")));

            L_foot_width.Text = string.Format("{0:0.0}mm", double.Parse(ini["FOOT_SIZE"]["L_foot width"].ToString().Replace(" mm", "")));
            R_foot_width.Text = string.Format("{0:0.0}mm", double.Parse(ini["FOOT_SIZE"]["R_foot width"].ToString().Replace(" mm", "")));

            L_foot_waist_Girth.Text = string.Format("{0:0.0}mm", double.Parse(ini["FOOT_SIZE"]["L_foot waist Girth"].ToString().Replace(" mm", "")));
            R_foot_waist_Girth.Text = string.Format("{0:0.0}mm", double.Parse(ini["FOOT_SIZE"]["R_foot waist Girth"].ToString().Replace(" mm", "")));
            
            L_foot_waist_height.Text = string.Format("{0:0.0}mm", double.Parse(ini["FOOT_SIZE"]["L_foot waist height"].ToString().Replace(" mm", "")));
            R_foot_waist_height.Text = string.Format("{0:0.0}mm", double.Parse(ini["FOOT_SIZE"]["R_foot waist height"].ToString().Replace(" mm", "")));
            
            L_foot_heel_height.Text = string.Format("{0:0.0}mm", double.Parse(ini["FOOT_SIZE"]["L_foot heel height"].ToString().Replace(" mm", "")));
            R_foot_heel_height.Text = string.Format("{0:0.0}mm", double.Parse(ini["FOOT_SIZE"]["R_foot heel height"].ToString().Replace(" mm", "")));
            
            L_foot_center_to_toe_length.Text = string.Format("{0:0.0}mm", double.Parse(ini["FOOT_SIZE"]["L_foot center to toe length"].ToString().Replace(" mm", "")));
            R_foot_center_to_toe_length.Text = string.Format("{0:0.0}mm", double.Parse(ini["FOOT_SIZE"]["R_foot center to toe length"].ToString().Replace(" mm", "")));
            
            L_foot_center_to_heel_length.Text = string.Format("{0:0.0}mm", double.Parse(ini["FOOT_SIZE"]["L_foot center to heel length"].ToString().Replace(" mm", "")));
            R_foot_center_to_heel_length.Text = string.Format("{0:0.0}mm", double.Parse(ini["FOOT_SIZE"]["R_foot center to heel length"].ToString().Replace(" mm", "")));
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            if(FootViewer3DL.Visibility==Visibility.Visible)
            {
                SetViewerVisiblity(ViewMode.VIEWMODE_2D);
            }
            else
            {
                SetViewerVisiblity(ViewMode.VIEWMODE_3D);
            }
        }
    }
}
