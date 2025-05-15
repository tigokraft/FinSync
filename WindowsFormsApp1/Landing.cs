using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace WindowsFormsApp1
{
    public partial class Landing : Form
    {
        private readonly HttpClient _http;
        private Guna2Panel overviewPanel;

        public Landing(HttpClient httpClient)
        {
            InitializeComponent();
            _http = httpClient;

            this.Text = "FinSync - Overview";
            this.Size = new Size(1100, 600);
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;

            LoadOverviewUI();
        }

        private void LoadOverviewUI()
        {
            Guna2Elipse ellipse = new Guna2Elipse();
            ellipse.BorderRadius = 20;
            ellipse.TargetControl = this;

            // Sidebar
            Guna2Panel sidebar = new Guna2Panel();
            sidebar.Size = new Size(200, this.Height);
            sidebar.Location = new Point(0, 0);
            sidebar.FillColor = Color.FromArgb(24, 24, 29);
            this.Controls.Add(sidebar);

            // App Title
            Guna2HtmlLabel appTitle = new Guna2HtmlLabel();
            appTitle.Text = "FinSync";
            appTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            appTitle.ForeColor = Color.White;
            appTitle.Location = new Point(25, 20);
            appTitle.AutoSize = true;
            sidebar.Controls.Add(appTitle);

            // Sidebar Buttons
            string[] navItems = { "Overview", "Budget", "Expenses", "Goals", "Settings" };
            int topOffset = 80;

            foreach (string name in navItems)
            {
                Guna2Button btn = new Guna2Button();
                btn.Text = name;
                btn.Size = new Size(180, 40);
                btn.Location = new Point(10, topOffset);
                btn.FillColor = Color.FromArgb(36, 36, 42);
                btn.ForeColor = Color.White;
                btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btn.BorderRadius = 6;
                btn.HoverState.FillColor = Color.FromArgb(46, 46, 56);
                btn.Cursor = Cursors.Hand;
                sidebar.Controls.Add(btn);

                topOffset += 50;
            }

            // Overview Panel
            overviewPanel = new Guna2Panel();
            overviewPanel.Size = new Size(this.Width - 200, this.Height);
            overviewPanel.Location = new Point(200, 0);
            overviewPanel.FillColor = Color.FromArgb(26, 26, 31);
            overviewPanel.Padding = new Padding(20);
            this.Controls.Add(overviewPanel);

            // Header
            Guna2HtmlLabel headerLabel = new Guna2HtmlLabel();
            headerLabel.Text = "Overview";
            headerLabel.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            headerLabel.ForeColor = Color.White;
            headerLabel.Location = new Point(30, 20);
            overviewPanel.Controls.Add(headerLabel);

            // Summary Cards
            CreateSummaryCard("Total Balance", "€2,340.00", new Point(30, 80));
            CreateSummaryCard("Monthly Spend", "€420.00", new Point(260, 80));
            CreateSummaryCard("Active Budget", "€800.00", new Point(490, 80));

            // Recent Expenses Label
            Guna2HtmlLabel recentExpensesLabel = new Guna2HtmlLabel();
            recentExpensesLabel.Text = "Recent Expenses";
            recentExpensesLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            recentExpensesLabel.ForeColor = Color.White;
            recentExpensesLabel.Location = new Point(30, 200);
            overviewPanel.Controls.Add(recentExpensesLabel);

            // Expense List
            FlowLayoutPanel expensesList = new FlowLayoutPanel();
            expensesList.Location = new Point(30, 230);
            expensesList.Size = new Size(400, 300);
            expensesList.AutoScroll = true;
            expensesList.BackColor = Color.Transparent;
            expensesList.FlowDirection = FlowDirection.TopDown;
            expensesList.WrapContents = false;
            overviewPanel.Controls.Add(expensesList);

            List<KeyValuePair<string, double>> recentExpenses = new List<KeyValuePair<string, double>>();
            recentExpenses.Add(new KeyValuePair<string, double>("Food", 15.75));
            recentExpenses.Add(new KeyValuePair<string, double>("Transport", 9.60));
            recentExpenses.Add(new KeyValuePair<string, double>("Rent", 420.00));
            recentExpenses.Add(new KeyValuePair<string, double>("Entertainment", 22.00));
            recentExpenses.Add(new KeyValuePair<string, double>("Utilities", 75.00));

            foreach (KeyValuePair<string, double> exp in recentExpenses)
            {
                Guna2Panel itemPanel = new Guna2Panel();
                itemPanel.Size = new Size(380, 50);
                itemPanel.FillColor = Color.FromArgb(31, 31, 36);
                itemPanel.BorderRadius = 8;
                itemPanel.Margin = new Padding(0, 0, 0, 10);

                Guna2HtmlLabel label = new Guna2HtmlLabel();
                label.Text = string.Format("{0} — €{1:F2}", exp.Key, exp.Value);
                label.ForeColor = Color.White;
                label.Font = new Font("Segoe UI", 10);
                label.Location = new Point(10, 15);
                label.AutoSize = true;

                itemPanel.Controls.Add(label);
                expensesList.Controls.Add(itemPanel);
            }

            // Budget Usage
            Guna2HtmlLabel budgetLabel = new Guna2HtmlLabel();
            budgetLabel.Text = "Budget Usage";
            budgetLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            budgetLabel.ForeColor = Color.White;
            budgetLabel.Location = new Point(460, 230);
            overviewPanel.Controls.Add(budgetLabel);

            Guna2ProgressBar budgetBar = new Guna2ProgressBar();
            budgetBar.Location = new Point(460, 260);
            budgetBar.Size = new Size(380, 25);
            budgetBar.Value = 70;
            budgetBar.ProgressColor = Color.FromArgb(122, 95, 255);
            budgetBar.ProgressColor2 = Color.FromArgb(58, 141, 255);
            budgetBar.FillColor = Color.FromArgb(42, 42, 46);
            budgetBar.BorderRadius = 10;
            budgetBar.BackColor = Color.Transparent;
            overviewPanel.Controls.Add(budgetBar);

            // Goals
            Guna2HtmlLabel goalsLabel = new Guna2HtmlLabel();
            goalsLabel.Text = "Savings Goals";
            goalsLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            goalsLabel.ForeColor = Color.White;
            goalsLabel.Location = new Point(460, 310);
            overviewPanel.Controls.Add(goalsLabel);

            List<Tuple<string, double, double>> goals = new List<Tuple<string, double, double>>();
            goals.Add(new Tuple<string, double, double>("Vacation Fund", 200, 500));
            goals.Add(new Tuple<string, double, double>("Emergency", 350, 1000));

            int goalTop = 340;
            foreach (Tuple<string, double, double> goal in goals)
            {
                Guna2HtmlLabel goalLabel = new Guna2HtmlLabel();
                goalLabel.Text = string.Format("{0} — €{1} / €{2}", goal.Item1, goal.Item2, goal.Item3);
                goalLabel.ForeColor = Color.White;
                goalLabel.Font = new Font("Segoe UI", 9);
                goalLabel.Location = new Point(460, goalTop);
                overviewPanel.Controls.Add(goalLabel);

                Guna2ProgressBar goalBar = new Guna2ProgressBar();
                goalBar.Location = new Point(460, goalTop + 20);
                goalBar.Size = new Size(380, 20);
                goalBar.Value = (int)((goal.Item2 / goal.Item3) * 100);
                goalBar.ProgressColor = Color.FromArgb(74, 222, 128);
                goalBar.FillColor = Color.FromArgb(42, 42, 46);
                goalBar.BorderRadius = 10;
                goalBar.BackColor = Color.Transparent;
                overviewPanel.Controls.Add(goalBar);

                goalTop += 50;
            }
        }


        private void CreateSummaryCard(string title, string value, Point location)
        {
            Guna2Panel card = new Guna2Panel();
            card.Size = new Size(220, 100);
            card.BorderRadius = 10;
            card.FillColor = Color.FromArgb(31, 31, 36);
            card.BackColor = Color.Transparent;
            card.Location = location;

            Guna2HtmlLabel titleLabel = new Guna2HtmlLabel();
            titleLabel.Text = title;
            titleLabel.ForeColor = Color.FromArgb(160, 160, 160);
            titleLabel.Font = new Font("Segoe UI", 10);
            titleLabel.Location = new Point(10, 10);
            titleLabel.AutoSize = true;

            Guna2HtmlLabel valueLabel = new Guna2HtmlLabel();
            valueLabel.Text = value;
            valueLabel.ForeColor = Color.White;
            valueLabel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            valueLabel.Location = new Point(10, 40);
            valueLabel.AutoSize = true;

            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);
            overviewPanel.Controls.Add(card);
        }
    }
}
