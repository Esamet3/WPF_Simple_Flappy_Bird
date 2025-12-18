using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;

namespace WPFKus
{
    
    public partial class MainWindow : Window
    {
        Random random = new Random();
        DispatcherTimer oyuntimer = new DispatcherTimer();
        double kusY = 100;
        double hiz = 0;
        double yerCekimi = 0.5;
        double boruX = 800;

        int puan = 0;
        int rekor = 0;

        bool puanEklendi = false;

        public MainWindow()
        {
            InitializeComponent();
            oyuntimer.Interval = TimeSpan.FromMilliseconds(20);
            oyuntimer.Tick += OyunDongusu;
            oyuntimer.Start();
            this.KeyDown += MainWindow_KeyDown;
            RekoruYukle();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                hiz = -10;
            }
        }

        private void OyunDongusu(object? sender, EventArgs e)
        {
            Rect kusAlani = new Rect(Canvas.GetLeft(flappyKus), kusY, flappyKus.Width, flappyKus.Height);
            Rect engelAlani = new Rect(boruX, Canvas.GetTop(boru), boru.Width, boru.Height);
            if (kusAlani.IntersectsWith(engelAlani))
            {
                oyuntimer.Stop();
                gameOverPanel.Visibility = Visibility.Visible;
                txtSonPuan.Text = "Puan :" + puan.ToString();
                if (puan > rekor)
                {
                    rekor = puan;
                    RekoruKaydet(rekor);
                    txtRekor.Text = rekor.ToString();
                    yeniRekor.Text = "YENİ REKOR!!!";
                }
                
                return;
            }
            hiz += yerCekimi;
            kusY += hiz;
            Canvas.SetTop(flappyKus, kusY);
            boruX -= 5;
            Canvas.SetLeft(boru, boruX);
            if (boruX < 100 && puanEklendi == false)
            {
                puanEklendi = true;
                puan++;
                txtPuan.Text = "Puan :" + puan.ToString();
            }
            if (boruX < -50)
            {
                puanEklendi = false;
                boruX = 800;
                Canvas.SetTop(boru, random.Next(100, 300));
            }
            kusRotasyon.Angle = hiz * 3;
        }
        private void OyunuSifirla()
        {
            kusY = 100;
            boruX = 800;
            hiz = 0;
            puan = 0;
            puanEklendi = false;
            txtPuan.Text = "Puan :0";
            Canvas.SetTop(flappyKus, kusY);
            Canvas.SetTop(boru, 250);
            Canvas.SetLeft(boru, boruX);
            oyuntimer.Start();
            gameOverPanel.Visibility = Visibility.Hidden;
        }
        private void RekoruYukle()
        {
            if (File.Exists("rekor.txt")){
                string okunanSayi = File.ReadAllText("rekor.txt");
                int.TryParse(okunanSayi, out rekor);
                txtRekor.Text = "Rekor :" + rekor.ToString();

            }

        }
        private void RekoruKaydet(int yeniSkor)
        {
            File.WriteAllText("rekor.txt", yeniSkor.ToString());
        }

        private void btnTekrar_Click(object sender, RoutedEventArgs e)
        {
            OyunuSifirla();
        }

        private void btnCıkıs_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}