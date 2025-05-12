using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly HttpClient httpClient;
        public Form1()
        {
            InitializeComponent();

            httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5034")
            };

            httpClient.DefaultRequestHeaders.Add("x-api-key", "12345-abcdef-67890");
        }

        private async void btnlogin_Click(object sender, EventArgs e)
        {
            var credentials = new
            {
                username = txtuser.Text,
                password = txtpass.Text
            };

            var json = JsonSerializer.Serialize(credentials);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync("api/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var doc = JsonDocument.Parse(responseContent);
                    var token = doc.RootElement.GetProperty("token").GetString();
                    doc.Dispose();

                    MessageBox.Show("Login successful!");

                    // Store the token for future use
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Example: Proceed to next form
                    //var mainForm = new MainForm(httpClient);
                    //mainForm.Show();
                    //this.Hide();
                }
                else
                {
                    MessageBox.Show($"Login failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error{ex.Message}");
            }
        }
    }
}
