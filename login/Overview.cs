using System;
using System.Drawing;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;
using WpfAxis = LiveCharts.Wpf.Axis;
using WpfSeparator = LiveCharts.Wpf.Separator;
using WpfBrushes = System.Windows.Media.Brushes;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;

namespace login
{
    public partial class Overview : Form
    {
        private readonly HttpClient _http;
        public Overview(HttpClient httpClient)
        {
            InitializeComponent();
            //SetupChart();
            _http = httpClient;
            Loader();

        }

        private async void Loader()
        {
            var balance = await GetBalanceAsync();
            BalanceTxt.Text = $"{balance:C2}";
        }

        private void SetupChart()
        {
            var cartesianChart = new LiveCharts.WinForms.CartesianChart
            {
                Size = new Size(300, 150),
                Location = new Point(450, 50)
            };

            cartesianChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Income",
                    Values = new ChartValues<double> { 0, 90, 130, 160, 180, 200 },
                    Stroke = WpfBrushes.MediumSpringGreen,
                    Fill = WpfBrushes.Transparent,
                    PointGeometry = null,
                    StrokeThickness = 2
                },
                new LineSeries
                {
                    Title = "Expenses",
                    Values = new ChartValues<double> { 0, 30, 50, 80, 95, 110 },
                    Stroke = WpfBrushes.IndianRed,
                    Fill = WpfBrushes.Transparent,
                    PointGeometry = null,
                    StrokeThickness = 2
                }
            };

            cartesianChart.AxisX.Add(new WpfAxis
            {
                Labels = new[] { "Apr 1", "6", "12", "18", "24", "30" },
                Foreground = WpfBrushes.Gray,
                Separator = new WpfSeparator { Stroke = WpfBrushes.DimGray }
            });

            cartesianChart.AxisY.Add(new WpfAxis
            {
                LabelFormatter = value => $"${value}",
                Foreground = WpfBrushes.Gray,
                Separator = new WpfSeparator { Stroke = WpfBrushes.DimGray }
            });

            this.Controls.Add(cartesianChart);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            ExpensesBtn.FillColor = Color.FromArgb(100, 27, 43, 48);
            OvBtn.FillColor = Color.FromArgb(100, 14, 18, 18);
            budgetBtn.FillColor = Color.FromArgb(100, 14, 18, 18);
            GoalsBtn.FillColor = Color.FromArgb(100, 14, 18, 18);
        }

        private void OvBtn_Click(object sender, EventArgs e)
        {
            OvBtn.FillColor = Color.FromArgb(100, 27, 43, 48);
            ExpensesBtn.FillColor = Color.FromArgb(100, 14, 18, 18);
            budgetBtn.FillColor = Color.FromArgb(100, 14, 18, 18);
            GoalsBtn.FillColor = Color.FromArgb(100, 14, 18, 18);
        }

        private void budgetBtn_Click(object sender, EventArgs e)
        {
            budgetBtn.FillColor = Color.FromArgb(100, 27, 43, 48);
            ExpensesBtn.FillColor = Color.FromArgb(100, 14, 18, 18);
            OvBtn.FillColor = Color.FromArgb(100, 14, 18, 18);
            GoalsBtn.FillColor = Color.FromArgb(100, 14, 18, 18);
        }

        private void GoalsBtn_Click(object sender, EventArgs e)
        {
            GoalsBtn.FillColor = Color.FromArgb(100, 27, 43, 48);
            ExpensesBtn.FillColor = Color.FromArgb(100, 14, 18, 18);
            OvBtn.FillColor = Color.FromArgb(100, 14, 18, 18);
            budgetBtn.FillColor = Color.FromArgb(100, 14, 18, 18);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (File.Exists("auth.token"))
            {
                File.Delete("auth.token");
                Application.Exit();
            }
        }

        private void BalanceTxt_Click(object sender, EventArgs e)
        {

        }


        public async Task<decimal> GetBalanceAsync()
        {
            var token = LoadToken();
            if (string.IsNullOrEmpty(token))
            {
                MessageBox.Show("No saved token. Please log in.");
                return 0;
            }

            try
            {
                var response = await _http.GetAsync("api/balance");
                if (response.IsSuccessStatusCode)
                {
                    string body = await response.Content.ReadAsStringAsync();

                    // Optionally log to debug or MessageBox
                    Console.WriteLine($"Raw balance response: {body}");

                    if (decimal.TryParse(body, out var balance))
                        return balance;

                    MessageBox.Show("Failed to parse balance value.");
                    return 0;
                }

                MessageBox.Show("Failed to fetch balance.");
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return 0;
            }
        }




        public static string LoadToken()
        {
            string tokenPath = "auth.token";

            if (File.Exists(tokenPath))
                return File.ReadAllText(tokenPath);

            return null;
        }

        bool menuExpand = true;
        private void menuTransition_Tick_1(object sender, EventArgs e)
        {
            if (menuExpand)
            {
                sidebar.Width -= 10;
                OvBtn.Width = sidebar.Width - 20;
                budgetBtn.Width = sidebar.Width - 20;
                ExpensesBtn.Width = sidebar.Width - 20;
                GoalsBtn.Width = sidebar.Width - 20;

                //for (int i = 100; i > 0; i--)
                //{
                //    OvBtn.ForeColor = Color.FromArgb(i, 255, 255, 255);
                //    budgetBtn.ForeColor = Color.FromArgb(i, 255, 255, 255);
                //    ExpensesBtn.ForeColor = Color.FromArgb(i, 255, 255, 255);
                //    GoalsBtn.ForeColor = Color.FromArgb(i, 255, 255, 255);
                //}

                if (sidebar.Width <= 65)
                {
                    menuExpand = false;
                    menuTransition.Stop();
                }
            }
            else
            {
                sidebar.Width += 10;
                OvBtn.Width = sidebar.Width - 20;
                budgetBtn.Width = sidebar.Width - 20;
                ExpensesBtn.Width = sidebar.Width - 20;
                GoalsBtn.Width = sidebar.Width - 20;

                //for (int i = 0; i <= 100; i++)
                //{
                //    OvBtn.ForeColor = Color.FromArgb(i, 255, 255, 255);
                //    budgetBtn.ForeColor = Color.FromArgb(i, 255, 255, 255);
                //    ExpensesBtn.ForeColor = Color.FromArgb(i, 255, 255, 255);
                //    GoalsBtn.ForeColor = Color.FromArgb(i, 255, 255, 255);
                //}

                if (sidebar.Width >= 200)
                {
                    menuExpand = true;
                    menuTransition.Stop();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            menuTransition.Start();
        }
    }
}