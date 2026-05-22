using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using tasked_forms;

namespace tasked_forms
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            // Clear all fields when form loads
            textBox1.Clear();  // Username
            textBox2.Clear();  // Email
            textBox3.Clear();  // Password
            checkBox1.Checked = false;
        }

        // SIGN UP BUTTON (button2)
        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string email = textBox2.Text.Trim();
            string password = textBox3.Text.Trim();

            // Validation - Check if any field is empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all fields.", "Sign Up Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation - Check if terms accepted
            if (!checkBox1.Checked)
            {
                MessageBox.Show("Please accept the terms and conditions.", "Sign Up Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation - Check email format (simple check)
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Sign Up Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation - Password length (at least 4 characters)
            if (password.Length < 4)
            {
                MessageBox.Show("Password must be at least 4 characters long.", "Sign Up Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                // Check if username already exists
                string checkUserQuery = "SELECT COUNT(*) FROM accounts WHERE username = @username";
                MySqlCommand checkUserCmd = new MySqlCommand(checkUserQuery, db.GetConnection());
                checkUserCmd.Parameters.AddWithValue("@username", username);
                int userCount = Convert.ToInt32(checkUserCmd.ExecuteScalar());

                if (userCount > 0)
                {
                    MessageBox.Show("Username already exists. Please choose another.", "Sign Up Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    db.CloseConnection();
                    return;
                }

                // Check if email already exists
                string checkEmailQuery = "SELECT COUNT(*) FROM accounts WHERE email = @email";
                MySqlCommand checkEmailCmd = new MySqlCommand(checkEmailQuery, db.GetConnection());
                checkEmailCmd.Parameters.AddWithValue("@email", email);
                int emailCount = Convert.ToInt32(checkEmailCmd.ExecuteScalar());

                if (emailCount > 0)
                {
                    MessageBox.Show("Email already registered. Please use another email or login.", "Sign Up Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    db.CloseConnection();
                    return;
                }

                // Insert new account (status = active, role = user)
                string query = "INSERT INTO accounts (username, password, email, status, role) VALUES (@username, @password, @email, 'active', 'user')";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.ExecuteNonQuery();

                db.CloseConnection();

                MessageBox.Show("Account created successfully!\n\nYou can now login with your credentials.",
                                "Sign Up Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear fields and go back to login
                ClearFields();
                GoBackToLogin();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Sign Up Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // BACK TO LOGIN BUTTON (button1)
        private void button1_Click(object sender, EventArgs e)
        {
            GoBackToLogin();
        }

        // CHECKBOX - Terms and Conditions (checkBox1)
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Optional: Enable/disable Sign Up button based on checkbox
            button2.Enabled = checkBox1.Checked;
        }

        // Clear all fields
        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            checkBox1.Checked = false;
        }

        // Go back to Login Form (Form1)
        private void GoBackToLogin()
        {
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }

        // Email TextBox (textBox2)
        private void textBox2_TextChanged(object sender, EventArgs e) { }

        // Username TextBox (textBox1)
        private void textBox1_TextChanged(object sender, EventArgs e) { }

        // Password TextBox (textBox3)
        private void textBox3_TextChanged(object sender, EventArgs e) { }
    }
}