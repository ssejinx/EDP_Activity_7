using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using tasked_forms;

namespace tasked_forms
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            button6.Text = "@" + Form1.loggedInUsername;
            label7.Text = "HELLO, " + Form1.loggedInUsername.ToUpper() + "!";

            // Setup filter combo box
            SetupFilterComboBox();

            // Test connection first
            TestConnection();

            // Then load data
            LoadSimpleStats();
            LoadSimpleTasks();
        }

        private void SetupFilterComboBox()
        {
            if (comboBox1 != null)
            {
                comboBox1.Items.Clear();
                comboBox1.Items.Add("ALL TASKS");
                comboBox1.Items.Add("PENDING");
                comboBox1.Items.Add("IN PROGRESS");
                comboBox1.Items.Add("COMPLETED");
                comboBox1.SelectedIndex = 0;
            }
        }

        private void TestConnection()
        {
            try
            {
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();
                MessageBox.Show("Database connection SUCCESSFUL!", "Test");
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection FAILED: " + ex.Message, "Error");
            }
        }

        private void LoadSimpleStats()
        {
            try
            {
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                MySqlCommand tasksCmd = new MySqlCommand("SELECT COUNT(*) FROM task", db.GetConnection());
                int totalTasks = Convert.ToInt32(tasksCmd.ExecuteScalar());
                db.CloseConnection();

                if (label10 != null) label10.Text = totalTasks.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Stats Error: " + ex.Message);
            }
        }

        private void LoadSimpleTasks()
        {
            try
            {
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                // Get selected filter from comboBox1
                string selectedFilter = comboBox1?.SelectedItem?.ToString() ?? "ALL TASKS";

                string query = "";

                switch (selectedFilter)
                {
                    case "PENDING":
                        query = "SELECT task_id, title, status, due_date FROM task WHERE LOWER(status) = 'pending' ORDER BY due_date ASC";
                        break;
                    case "IN PROGRESS":
                        query = "SELECT task_id, title, status, due_date FROM task WHERE LOWER(status) = 'in progress' ORDER BY due_date ASC";
                        break;
                    case "COMPLETED":
                        query = "SELECT task_id, title, status, due_date FROM task WHERE LOWER(status) = 'completed' ORDER BY due_date ASC";
                        break;
                    default:
                        query = "SELECT task_id, title, status, due_date FROM task ORDER BY due_date ASC";
                        break;
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, db.GetConnection());
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                db.CloseConnection();

                // Format the DataGridView
                if (dataGridView1.Columns["task_id"] != null)
                    dataGridView1.Columns["task_id"].Visible = false;
                if (dataGridView1.Columns["title"] != null)
                    dataGridView1.Columns["title"].HeaderText = "TASK NAME".ToUpper();
                if (dataGridView1.Columns["status"] != null)
                    dataGridView1.Columns["status"].HeaderText = "STATUS".ToUpper();
                if (dataGridView1.Columns["due_date"] != null)
                    dataGridView1.Columns["due_date"].HeaderText = "DUE DATE".ToUpper();

                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Inter", 8, System.Drawing.FontStyle.Bold);

                // Color code status column
                ColorCodeStatusColumn();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tasks Error: " + ex.Message);
            }
        }

        private void ColorCodeStatusColumn()
        {
            if (dataGridView1.Columns["status"] == null) return;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["status"].Value != null)
                {
                    string status = row.Cells["status"].Value.ToString().ToLower();

                    if (status == "pending")
                    {
                        row.Cells["status"].Style.ForeColor = System.Drawing.Color.Orange;
                        row.Cells["status"].Style.Font = new System.Drawing.Font("Inter", 8, System.Drawing.FontStyle.Bold);
                    }
                    else if (status == "in progress")
                    {
                        row.Cells["status"].Style.ForeColor = System.Drawing.Color.Blue;
                        row.Cells["status"].Style.Font = new System.Drawing.Font("Inter", 8, System.Drawing.FontStyle.Bold);
                    }
                    else if (status == "completed")
                    {
                        row.Cells["status"].Style.ForeColor = System.Drawing.Color.Green;
                        row.Cells["status"].Style.Font = new System.Drawing.Font("Inter", 8, System.Drawing.FontStyle.Bold);
                    }
                }
            }
        }

        // FILTER CHANGED - Reload tasks when comboBox1 selection changes
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSimpleTasks();
        }

        // BUTTON EVENTS
        private void button1_Click(object sender, EventArgs e)
        {
            LoadSimpleStats();
            LoadSimpleTasks();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Logged in as: " + Form1.loggedInUsername, "Profile");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form8 peopleForm = new Form8();
            peopleForm.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form8 peopleForm = new Form8();
            peopleForm.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Form8 peopleForm = new Form8();
            peopleForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Task Management - Coming Soon");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reports - Coming Soon");
        }

        // Empty events
        private void button7_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void button11_Click(object sender, EventArgs e) { }
        private void panel5_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox7_Click(object sender, EventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void label11_Click(object sender, EventArgs e) { }
        private void label12_Click(object sender, EventArgs e) { }
        private void label13_Click(object sender, EventArgs e) { }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}