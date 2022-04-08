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
using System.Windows.Media.Media3D;

namespace kiosk.pages
{
    /// <summary>
    /// Disease.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public enum FOOT_SIDES { LEFT, RIGHT };
    public partial class Disease : Page
    {
        public Disease()
        {
            InitializeComponent();

            LoadResultData();

            GetMeshFromPly(FOOT_SIDES.LEFT, @"C:\HBT_Foot_Scanner\Result\Demo_Left.ply");
            GetMeshFromPly(FOOT_SIDES.RIGHT, @"C:\HBT_Foot_Scanner\Result\Demo_Right.ply");

            // C:\HBT_Foot_Scanner\Data\insole_sample\180_L.ply

        }

        private void LoadResultData()
        {
            IniFile ini = new IniFile();
            ini.Load(@"C:\HBT_Foot_Scanner\Result\measurements.ini");

            double footLengthL = double.Parse(ini["FOOT_SIZE"]["L_foot length"].ToString().Replace(" mm", ""));
            double footWidthL = double.Parse(ini["FOOT_SIZE"]["L_foot width"].ToString().Replace(" mm", ""));
            double footLengthR = double.Parse(ini["FOOT_SIZE"]["R_foot length"].ToString().Replace(" mm", ""));
            double footWidthR = double.Parse(ini["FOOT_SIZE"]["R_foot width"].ToString().Replace(" mm", ""));

            FootLengthL.Text = string.Format("{0:0.0}mm", footLengthL);
            FootWidthL.Text = string.Format("{0:0.0}mm", footWidthL);
            FootLengthR.Text = string.Format("{0:0.0}mm", footLengthR);
            FootWidthR.Text = string.Format("{0:0.0}mm", footWidthR);
        }

        private void GetMeshFromPly(FOOT_SIDES foot_side, string _path_model)
        {
            if (File.Exists(_path_model))
            {
                MeshGeometry3D _geometry = getGeometryFromPLY(_path_model, false);
                if (foot_side == FOOT_SIDES.LEFT)
                {
                    model1.Points = _geometry.Positions;
                    model1.Color = Colors.Beige;
                }
                else
                {
                    model2.Points = _geometry.Positions;
                    model2.Color = Colors.Beige;
                }
            }
        }

        private MeshGeometry3D getGeometryFromPLY(string path_ply, bool flip)
        {
            MeshGeometry3D geometry = new MeshGeometry3D();
            geometry.TriangleIndices = new Int32Collection();
            geometry.TextureCoordinates = new PointCollection();
            geometry.Positions = new Point3DCollection();

            int idx = 0;
            string line;
            StreamReader reader = new StreamReader(path_ply);
            while ((line = reader.ReadLine()) != null)
            {
                if (idx++ < 10) { continue; }
                try
                {
                    if (idx % 2 == 0) { continue; }
                    string[] _str_points = line.Trim().Split(' ');
                    geometry.Positions.Add(
                        new Point3D(
                            Double.Parse(_str_points[0]) * (flip ? -1 : 1),
                            Double.Parse(_str_points[1]),
                            Double.Parse(_str_points[2])
                        ));
                }
                catch (Exception) { }
            }
            return geometry;
        }

        private void Cancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("pages/Purchase.xaml", UriKind.Relative));
        }

        private void Rescan_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
