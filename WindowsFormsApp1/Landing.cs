using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Landing : Form
    {
        private readonly HttpClient _http;

        public Landing(HttpClient httpClient)
        {
            InitializeComponent();
            _http = httpClient;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists("auth.token"))
                File.Delete("auth.token");

            var loginForm = new LoginForm(_http);
            loginForm.Show();
            this.Close();
        }
    }
}
