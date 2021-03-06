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
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.Windows.Threading;

using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.IO;
using System.Drawing;

namespace kiosk.pages
{
    /// <summary>
    /// Scan.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Scan : Page
    {
        private BackgroundWorker worker;
        private readonly VideoCapture capture;
        bool useCamera;

        public Scan()
        {
            InitializeComponent();

            useCamera = false;

            capture = new VideoCapture();

            EnableButton(btn_Reset_Scan, true);
            EnableButton(btn_See_Result, false);

            LoadIni();
            InitWorkerThread();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (useCamera == true)
            {
                // 페이지 Load 시 카메라 캡쳐 비동기 실행
                capture.Open(0, VideoCaptureAPIs.ANY);
                if (!capture.IsOpened())
                {
                    return;
                }
                else
                {
                    RunTask();
                }
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();

            // 페이지 Unload 시 카메라 캡쳐 종료
            if (useCamera == true)
            {
                if (!capture.IsDisposed)
                {
                    capture.Dispose();
                }
            }
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            EnableButton((Button)sender, false);
            NavigationService.Navigate(new Uri("pages/Main.xaml", UriKind.Relative));
        }

        private void Btn_Reset_Scan_Click(object sender, RoutedEventArgs e)
        {
           EnableButton((Button)sender, false);

            // 스캔 시작 시 카메라 연결 해제
            if (!capture.IsDisposed)
            {
                capture.Dispose();
            }

            // 스캔 프로그램 시작
            try
            {
                ProcessStart(@"C:\HBT_Foot_Scanner\Foot_Scan\HBT_Foot_Scanning.vbs");
                worker.RunWorkerAsync();
                tb_scanStart.Text = "스캔 중";
            }
            catch
            {
                Console.WriteLine("실행파일이 없습니다");
            }
        }

        private void Btn_close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void Btn_See_Result_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("pages/ScanResult.xaml", UriKind.Relative));
        }

        // 비동기 프로세스 생성
        private async void RunTask()
        {
            var task = Task.Run(() => Run_Camera());
            await task;
        }

        // 카메라 캡쳐 및 Display
        private bool Run_Camera()
        {
            while (true)
            {
                try
                {
                    using (var frameMat = capture.RetrieveMat())
                    {
                        var frameBitmap = BitmapConverter.ToBitmap(frameMat);
                        
                        // UI 쓰레드 접근을 위한 Invoke
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { VideoViewer.Source = ConvertBmpToImageSource(frameBitmap); }));
                    }
                }
                catch
                {
                    Console.WriteLine("예외 발생");
                }

                Thread.Sleep(100);
            }
        }

        // 스캔 진행상황 및 결과를 실시간으로 불러오고 표시해주기 위한 Background Worker Initialize
        public void InitWorkerThread()
        {
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true; //작업의 진행률이 바뀔때 ProgressChanged 이벤트 발생여부 체크
            worker.WorkerSupportsCancellation = true; //작업 취소 가능 여부 true 로 설정

            worker.DoWork += new DoWorkEventHandler(worker_DoWork); //해야할 작업을 실행할 메서드 정의
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged); //UI쪽에 진행사항을 보여줌, WorkerReportsProgress 속성값이 true 일때만 이벤트 발생
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);//작업이 완료되었을 때 실행할 콜백메서드 정의
        }

        // 검사 진행상황을 ini 파일에서 실시간으로 불러옴
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            IniFile ini = new IniFile();
            ini.Load(@"C:/HBT_Foot_Scanner/Result/status.ini");
            string status = ini["Progress"]["status"].ToString();
            string percent = ini["Progress"]["percent"].ToString();

            while (int.Parse(percent) < 100)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return; //about work, if it's cancelled;
                }
                try
                {
                    ini.Load(@"C:/HBT_Foot_Scanner/Result/status.ini");
                    status = ini["Progress"]["status"].ToString();
                    percent = ini["Progress"]["percent"].ToString();

                    worker.ReportProgress(Convert.ToInt32(percent));
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { tb_status.Text = status; }));
                }
                catch
                {
                    Debug.WriteLine("ini파일이 없습니다.");
                }

                Thread.Sleep(100);
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tb_progress.Text = e.ProgressPercentage.ToString() + "%";
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
            }
            EnableButton(btn_See_Result, true);
            EnableButton(btn_Reset_Scan, true);

            tb_scanStart.Text = "다시 스캔";
            return;
        }

        private void LoadIni()
        {
            IniFile ini = new IniFile();
            try
            {
                ini.Load(@"C:/HBT_Foot_Scanner/Result/Status.ini");
            }
            catch
            {
                Debug.WriteLine("ini파일이 없습니다.");
            }

            string temp = ini["Progress"]["percent"].ToString();

            if (int.Parse(temp) != 0)
            {
                ini["progress"]["percent"] = "0";
                ini["progress"]["status"] = "준비";
                ini.Save(@"C:/HBT_Foot_Scanner/Result/Status.ini");
                ini.Load(@"C:/HBT_Foot_Scanner/Result/Status.ini");
            }
            string status = ini["Progress"]["status"].ToString();
            string percent = ini["Progress"]["percent"].ToString();

            tb_status.Text = status;
            tb_progress.Text = percent + "%";
        }

        // BitmapImage를 Image Source로 변환
        public BitmapImage ConvertBmpToImageSource(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        // 외부 프로세스 시작 (exe, vbs .. UI 실행파일과 같은 경로 내에 있어야 함)
        private void ProcessStart(string fileName)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
                Process.Start(startInfo);
            }
            catch { }
        }

        private void ProcessKill(string fileName)
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.StartsWith(fileName))
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                        Console.WriteLine("프로세스 종료 실패");
                    }
                }
            }
        }

        private void EnableButton(Button btn, bool enable)
        {
            if(enable == true)
            {
                btn.IsEnabled = true;
                btn.Opacity = 1;
            }
            else
            {
                btn.IsEnabled = false;
                btn.Opacity = 0.3;
            }
        }
    }
}

