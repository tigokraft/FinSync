using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class LoginForm : Form
    {
        private readonly HttpClient httpClient;

        private readonly HttpClient _http;

        public LoginForm(HttpClient httpClient)
        {
            InitializeComponent();
            _http = httpClient;
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

            var response = await _http.PostAsync("api/auth/login", content);
            if (response.IsSuccessStatusCode)
            {
                var tokenJson = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(tokenJson);
                var token = doc.RootElement.GetProperty("token").GetString();

                File.WriteAllText("auth.token", token);
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var mainForm = new Landing(httpClient);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid credentials.");
            }
        }
    }
}
