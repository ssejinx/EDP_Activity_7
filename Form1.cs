using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using tasked_forms;

namespace tasked_forms
{
    public partial class Form1 : Form
    {
        public static string loggedInUsername = "";
        public static string loggedInRole = "";

        public Form1()
        {
            InitializeComponent();
        }

        // LOGIN BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            // Get username and password from textboxes
            string username = usernametxtbox.Text.Trim();
            string password = passwordtxtbox.Text.Trim();

            // Check if fields are empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password.", "Login Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Get database connection
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                // Query to check username and password
                string query = "SELECT account_id, username, role, status FROM accounts WHERE username = @username AND password = @password";

                using (MySqlCommand cmd = new MySqlCommand(query, db.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string status = reader["status"].ToString();
                            string role = reader["role"].ToString();

                            // Check if account is active
                            if (status != "active")
                            {
                                MessageBox.Show("Your account is inactive. Please contact administrator.",
                                                "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            // Login successful
                            MessageBox.Show("Login successful! Welcome " + reader["username"].ToString(),
                                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Save Username and Role
                            loggedInUsername = reader["username"].ToString();
                            loggedInRole = role;

                            // REDIRECT BASED ON ROLE
                            if (role == "admin")
                            {
                                // Admin goes to Form9 (User Management)
                                Form9 adminForm = new Form9();
                                adminForm.Show();
                            }
                            else
                            {
                                // Member goes to Form4 (Dashboard)
                                Form4 dashboard = new Form4();
                                dashboard.Show();
                                dashboard.Activate();  // ← ADD THIS ONE LINE
                                this.Hide();
                            }

                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Login Failed",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // FORGOT PASSWORD LINK - Opens Form3 (Forgot Password Form)
        private void forgotpassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form3 forgotPasswordForm = new Form3();
            forgotPasswordForm.Show();
            this.Hide();
        }

        // SIGN UP BUTTON - Opens Form6 (SignUp Form)
        private void signUpBtn_Click(object sender, EventArgs e)
        {
            Form6 signUpForm = new Form6();
            signUpForm.Show();
            this.Hide();
        }

        // ===============================================
        // KEEP THESE EMPTY EVENTS (they don't affect anything)
        // ===============================================
        private void Form1_Load(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void usernameLabel_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e) { }
        private void dashedDivider_Click(object sender, EventArgs e) { }
        private void agreementCheckBox_CheckedChanged(object sender, EventArgs e) { }

        // USERNAME TEXTBOX - Set height
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (usernametxtbox != null)
            {
                usernametxtbox.AutoSize = false;
                usernametxtbox.Height = 40;
            }
        }

        // PASSWORD TEXTBOX - Set height
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (passwordtxtbox != null)
            {
                passwordtxtbox.AutoSize = false;
                passwordtxtbox.Height = 40;
            }
        }
    }
}