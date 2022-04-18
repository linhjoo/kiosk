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
        double gFootLengthL, gFootLengthR;

        public Disease()
        {
            InitializeComponent();
            LoadResultData();
            SetInsole3DModel();
        }

        private void SetInsole3DModel()
        {
            int footLengthL, footLengthR;

            // 인솔 사이즈 최소 단위인 5 단위 반올림을 위해 *2, 반올림 후 /2
            footLengthL = (int)IntRound(gFootLengthL * 2, -1) / 2;
            footLengthR = (int)IntRound(gFootLengthR * 2, -1) / 2;

            // 에러 방지를 위한 최대 최소 사이즈 지정
            if (footLengthL < 180) footLengthL = 180;
            else if (footLengthL > 290) footLengthL = 290;

            if (footLengthR < 180) footLengthR = 180;
            else if (footLengthR > 290) footLengthR = 290;

            ModelImporter importer1 = new ModelImporter();

            System.Windows.Media.Media3D.Model3DGroup m3DGroup1 = importer1.Load($"C:/HBT_Foot_Scanner/Data/insole_sample/{footLengthL}_L.ply");
            System.Windows.Media.Media3D.Model3DGroup m3DGroup2 = importer1.Load($"C:/HBT_Foot_Scanner/Data/insole_sample/{footLengthR}_R.ply");

            System.Windows.Media.Media3D.Material mat1 = MaterialHelper.CreateMaterial(
            new SolidColorBrush(Color.FromRgb(194, 197, 204)));
            
            foreach (System.Windows.Media.Media3D.GeometryModel3D geometryModel in m3DGroup1.Children)
            {
                geometryModel.Material = mat1;
                geometryModel.BackMaterial = mat1;
            }
            foreach (System.Windows.Media.Media3D.GeometryModel3D geometryModel in m3DGroup2.Children)
            {
                geometryModel.Material = mat1;
                geometryModel.BackMaterial = mat1;
            }

            model1.Content = m3DGroup1;
            model2.Content = m3DGroup2;
        }

        // 1의 자리 반올림
        public double IntRound(double Value, int Digit)
        {
            double Temp = Math.Pow(10.0, Digit);
            return Math.Round(Value * Temp) / Temp;
        }

        // 사이즈 측정 결과 불러오기
        private void LoadResultData()
        {
            IniFile ini = new IniFile();
            ini.Load(@"C:\HBT_Foot_Scanner\Result\measurements.ini");

            //ini 파일에서 수치를 불러와 " mm" 문자 제거 후 double로 캐스팅
            double footLengthL = double.Parse(ini["FOOT_SIZE"]["L_foot length"].ToString().Replace(" mm", ""));
            double footWidthL = double.Parse(ini["FOOT_SIZE"]["L_foot width"].ToString().Replace(" mm", ""));
            double footLengthR = double.Parse(ini["FOOT_SIZE"]["R_foot length"].ToString().Replace(" mm", ""));
            double footWidthR = double.Parse(ini["FOOT_SIZE"]["R_foot width"].ToString().Replace(" mm", ""));

            gFootLengthL = footLengthL;
            gFootLengthR = footLengthR;

            FootLengthL.Text = string.Format("{0:0.0}mm", footLengthL);
            FootWidthL.Text = string.Format("{0:0.0}mm", footWidthL);
            FootLengthR.Text = string.Format("{0:0.0}mm", footLengthR);
            FootWidthR.Text = string.Format("{0:0.0}mm", footWidthR);
        }


        /*
        // 도트 데이터를 뷰어에 표시할 경우 사용
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
        */

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
