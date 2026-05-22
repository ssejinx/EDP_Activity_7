using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using tasked_forms;

namespace tasked_forms
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // Optional: Clear textbox when form loads
            textBox1.Clear();
        }

        // SEND RESET PASSWORD BUTTON (button2)
        private void button2_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text.Trim();

            // Check if email is empty
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please enter your email address.", "Recovery Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Get database connection
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                // Check if email exists in accounts table
                string query = "SELECT username FROM accounts WHERE email = @email";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@email", email);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string username = result.ToString();

                    // Ask if they want to reset password
                    DialogResult dialogResult = MessageBox.Show($"Email found! Username: {username}\n\nDo you want to reset your password to 'default123'?",
                                                    "Reset Password", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dialogResult == DialogResult.Yes)
                    {
                        // Update password to default
                        string updateQuery = "UPDATE accounts SET password = 'default123' WHERE email = @email";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, db.GetConnection());
                        updateCmd.Parameters.AddWithValue("@email", email);
                        updateCmd.ExecuteNonQuery();

                        MessageBox.Show("Password has been reset to: default123\n\nPlease change it after logging in.",
                                        "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // Email not found
                    MessageBox.Show("Email address not found in our records. Please check and try again.",
                                    "Recovery Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // BACK TO LOGIN LINK (linkLabel1)
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Go back to Login Form (Form1)
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }

        // Email TextBox (textBox1)
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Optional: Add email validation here if needed
            // This event fires every time text changes
        }

        // Keep these empty events - they don't affect functionality
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
    }
}