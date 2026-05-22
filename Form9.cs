using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using tasked_forms;

namespace tasked_forms
{
    public partial class Form9 : Form
    {
        private int selectedAccountId = -1;

        public Form9()
        {
            InitializeComponent();

            button6.Text = "@" + Form1.loggedInUsername;

            // Set password textbox to hide characters by default
            textBox3.PasswordChar = '*';
            textBox3.Text = "";

            // Setup Status ComboBox
            SetupStatusComboBox();

            // Force close any existing connections before loading
            try
            {
                taskisdbConnection db = taskisdbConnection.GetInstance();
                if (db.GetConnection().State == ConnectionState.Open)
                {
                    db.CloseConnection();
                }
            }
            catch { }

            LoadAccounts();
        }

        // SETUP STATUS COMBOBOX
        private void SetupStatusComboBox()
        {
            if (comboBox1 != null)
            {
                comboBox1.Items.Clear();
                comboBox1.Items.Add("active");
                comboBox1.Items.Add("inactive");
                comboBox1.SelectedIndex = 0;
            }
        }

        // LOAD ALL ACCOUNTS (without password by default)
        private void LoadAccounts()
        {
            try
            {
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                string query = "SELECT account_id, username, email, status FROM accounts";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, db.GetConnection());
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;

                // Hide account_id column
                if (dataGridView1.Columns["account_id"] != null)
                {
                    dataGridView1.Columns["account_id"].Visible = false;
                }

                // Set column widths
                if (dataGridView1.Columns["username"] != null)
                {
                    dataGridView1.Columns["username"].Width = 150;
                }
                if (dataGridView1.Columns["email"] != null)
                {
                    dataGridView1.Columns["email"].Width = 250;
                }
                if (dataGridView1.Columns["status"] != null)
                {
                    dataGridView1.Columns["status"].Width = 80;
                }

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading accounts: " + ex.Message);
            }
        }

        // LOAD PASSWORD FOR SELECTED ACCOUNT
        private void LoadPasswordForSelectedAccount()
        {
            try
            {
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                string query = "SELECT password FROM accounts WHERE account_id = @id";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@id", selectedAccountId);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    textBox3.Text = result.ToString();
                }

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading password: " + ex.Message);
            }
        }

        // SHOW/HIDE PASSWORD IN ACCOUNT DETAILS (checkbox)
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // Show actual password when checkbox is checked
                if (selectedAccountId != -1)
                {
                    LoadPasswordForSelectedAccount();
                    textBox3.PasswordChar = '\0';  // Show characters
                }
            }
            else
            {
                // Hide password when checkbox is unchecked
                textBox3.Text = "";
                textBox3.PasswordChar = '*';  // Hide with dots
            }
        }

        // SEARCH BUTTON (button7 - Go)
        private void button7_Click(object sender, EventArgs e)
        {
            string search = textBox1.Text.Trim();

            try
            {
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                string query = "SELECT account_id, username, email, status FROM accounts WHERE username LIKE @search OR email LIKE @search";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search error: " + ex.Message);
            }
        }

        // ADD ACCOUNT BUTTON (button10)
        private void button10_Click(object sender, EventArgs e)
        {
            string username = textBox2.Text.Trim();
            string password = textBox3.Text.Trim();
            string email = textBox4.Text.Trim();
            string status = comboBox1?.SelectedItem?.ToString() ?? "active";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username and Password are required.");
                return;
            }

            try
            {
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                string query = "INSERT INTO accounts (username, password, email, status) VALUES (@username, @password, @email, @status)";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.ExecuteNonQuery();

                db.CloseConnection();

                MessageBox.Show("Account added successfully!");
                ClearFields();
                LoadAccounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding account: " + ex.Message);
            }
        }

        // EDIT ACCOUNT BUTTON (button4)
        private void button4_Click(object sender, EventArgs e)
        {
            if (selectedAccountId == -1)
            {
                MessageBox.Show("Please select an account from the list first.");
                return;
            }

            string username = textBox2.Text.Trim();
            string email = textBox4.Text.Trim();
            string status = comboBox1?.SelectedItem?.ToString() ?? "active";

            try
            {
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                string query = "UPDATE accounts SET username = @username, email = @email, status = @status WHERE account_id = @id";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@id", selectedAccountId);
                cmd.ExecuteNonQuery();

                // Update password if provided and not empty
                string password = textBox3.Text.Trim();
                if (!string.IsNullOrEmpty(password))
                {
                    string pwdQuery = "UPDATE accounts SET password = @password WHERE account_id = @id";
                    MySqlCommand pwdCmd = new MySqlCommand(pwdQuery, db.GetConnection());
                    pwdCmd.Parameters.AddWithValue("@password", password);
                    pwdCmd.Parameters.AddWithValue("@id", selectedAccountId);
                    pwdCmd.ExecuteNonQuery();
                }

                db.CloseConnection();

                MessageBox.Show("Account updated successfully!");
                ClearFields();
                LoadAccounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating account: " + ex.Message);
            }
        }

        // DELETE/DEACTIVATE ACCOUNT BUTTON (button11)
        private void button11_Click(object sender, EventArgs e)
        {
            if (selectedAccountId == -1)
            {
                MessageBox.Show("Please select an account from the list first.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to DEACTIVATE this account?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    taskisdbConnection db = taskisdbConnection.GetInstance();
                    db.OpenConnection();

                    string query = "UPDATE accounts SET status = 'inactive' WHERE account_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                    cmd.Parameters.AddWithValue("@id", selectedAccountId);
                    cmd.ExecuteNonQuery();

                    db.CloseConnection();

                    MessageBox.Show("Account deactivated!");
                    ClearFields();
                    LoadAccounts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // DATA GRID VIEW - SELECT ROW
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedAccountId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["account_id"].Value);
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["username"].Value.ToString();

                // Clear password field (for security)
                textBox3.Text = "";
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["email"].Value.ToString();

                // Load status into comboBox1
                if (dataGridView1.Rows[e.RowIndex].Cells["status"].Value != null)
                {
                    string status = dataGridView1.Rows[e.RowIndex].Cells["status"].Value.ToString();
                    if (comboBox1 != null)
                    {
                        comboBox1.Text = status;
                    }
                }

                // If checkbox is checked, load password
                if (checkBox1.Checked)
                {
                    LoadPasswordForSelectedAccount();
                    textBox3.PasswordChar = '\0';  // Show characters
                }
                else
                {
                    textBox3.PasswordChar = '*';  // Hide with dots
                }
            }
        }

        // COMBOBOX - Status changed (Active/Inactive)
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // This is for manual status change if needed
            // The status is saved when clicking Update button
        }

        // CLEAR FIELDS
        private void ClearFields()
        {
            selectedAccountId = -1;
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox1.Text = "";
            checkBox1.Checked = false;  // Uncheck show password
            textBox3.PasswordChar = '*';
            if (comboBox1 != null) comboBox1.SelectedIndex = 0;  // Reset to "active"
        }

        // LOG OUT BUTTON (button6)
        private void button6_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Close();
        }

        // EMPTY EVENTS (keep these as is)
        private void button2_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void panel4_Paint(object sender, PaintEventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void label12_Click(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label11_Click(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
        
            // Direct to Form5 Reports
            Form5 reportsForm = new Form5();
            reportsForm.Show();
            this.Hide();
        }
    }
}