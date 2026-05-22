using Microsoft.Office.Core;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using tasked_forms;
using tasked_forms.Properties;
using Excel = Microsoft.Office.Interop.Excel;

namespace tasked_forms
{
    public partial class Form5 : Form
    {
        private DataTable currentDataTable = null;
        private string currentReportType = "";

        public Form5()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Set default dates
            dateTimePicker1.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dateTimePicker2.Value = DateTime.Now;

            // Setup report type combo box
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Tasks Report");
            comboBox1.Items.Add("Members Report");
            comboBox1.Items.Add("Task Assignments Report");
            comboBox1.Items.Add("Course Progress Report");
            comboBox1.Items.Add("Member Task Summary");
            comboBox1.SelectedIndex = 0;

            // Set default title
            textBox1.Text = "TASK MANAGEMENT REPORT";
        }

        // GENERATE REPORT BUTTON (button7)
        private void button7_Click(object sender, EventArgs e)
        {
            currentReportType = comboBox1.SelectedItem.ToString();
            DateTime fromDate = dateTimePicker1.Value;
            DateTime toDate = dateTimePicker2.Value;
            bool includeCompleted = checkBox2.Checked;
            bool includeSubtasks = checkBox1.Checked;

            switch (currentReportType)
            {
                case "Tasks Report":
                    LoadTasksReport(fromDate, toDate, includeCompleted);
                    break;
                case "Members Report":
                    LoadMembersReport();
                    break;
                case "Task Assignments Report":
                    LoadAssignmentsReport(fromDate, toDate);
                    break;
                case "Course Progress Report":
                    LoadCourseProgressReport(fromDate, toDate);
                    break;
                case "Member Task Summary":
                    LoadMemberTaskSummary(fromDate, toDate);
                    break;
            }
        }

        private void LoadTasksReport(DateTime fromDate, DateTime toDate, bool includeCompleted)
        {
            try
            {
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                string statusFilter = "";
                if (!includeCompleted)
                {
                    statusFilter = " AND LOWER(t.status) != 'completed' ";
                }

                string query = $@"SELECT t.task_id, 
                                         t.title as 'TASK NAME',
                                         CASE t.status
                                           WHEN 'pending' THEN 'PENDING'
                                           WHEN 'in progress' THEN 'IN PROGRESS'
                                           WHEN 'completed' THEN 'COMPLETED'
                                           ELSE UPPER(t.status)
                                         END as 'STATUS',
                                         c.course_code as 'COURSE',
                                         cat.category_type as 'DIFFICULTY',
                                         DATE_FORMAT(t.due_date, '%b %d, %Y') as 'DUE DATE'
                                  FROM task t
                                  LEFT JOIN course c ON t.course_id = c.course_id
                                  LEFT JOIN category cat ON t.category_id = cat.category_id
                                  WHERE t.due_date BETWEEN @fromDate AND @toDate
                                  {statusFilter}
                                  ORDER BY t.due_date ASC";

                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@fromDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@toDate", toDate.ToString("yyyy-MM-dd"));

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                currentDataTable = new DataTable();
                adapter.Fill(currentDataTable);
                dataGridView1.DataSource = currentDataTable;

                db.CloseConnection();

                if (dataGridView1.Columns["task_id"] != null)
                    dataGridView1.Columns["task_id"].Visible = false;

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.RowHeadersVisible = false;

                if (dataGridView1.Columns["STATUS"] != null)
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["STATUS"].Value != null)
                        {
                            string status = row.Cells["STATUS"].Value.ToString();
                            if (status == "PENDING")
                                row.Cells["STATUS"].Style.ForeColor = Color.Orange;
                            else if (status == "IN PROGRESS")
                                row.Cells["STATUS"].Style.ForeColor = Color.Blue;
                            else if (status == "COMPLETED")
                                row.Cells["STATUS"].Style.ForeColor = Color.Green;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading tasks report: " + ex.Message);
            }
        }

        private void LoadMembersReport()
        {
            try
            {
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                string query = @"SELECT CONCAT(m.member_fname, ' ', m.member_lname) as 'MEMBER NAME',
                                        m.member_email as 'EMAIL'
                                 FROM member m
                                 ORDER BY m.member_lname";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, db.GetConnection());
                currentDataTable = new DataTable();
                adapter.Fill(currentDataTable);
                dataGridView1.DataSource = currentDataTable;

                db.CloseConnection();

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.RowHeadersVisible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading members report: " + ex.Message);
            }
        }

        private void LoadAssignmentsReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                string query = @"SELECT CONCAT(m.member_fname, ' ', m.member_lname) as 'MEMBER',
                                       t.title as 'TASK',
                                       c.course_code as 'COURSE',
                                       t.status as 'STATUS',
                                       DATE_FORMAT(t.due_date, '%b %d, %Y') as 'DUE DATE'
                                 FROM task_assignment ta
                                 JOIN member m ON ta.member_id = m.member_id
                                 JOIN task t ON ta.task_id = t.task_id
                                 LEFT JOIN course c ON t.course_id = c.course_id
                                 WHERE t.due_date BETWEEN @fromDate AND @toDate
                                 ORDER BY t.due_date ASC";

                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@fromDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@toDate", toDate.ToString("yyyy-MM-dd"));

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                currentDataTable = new DataTable();
                adapter.Fill(currentDataTable);
                dataGridView1.DataSource = currentDataTable;

                db.CloseConnection();

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.RowHeadersVisible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading assignments report: " + ex.Message);
            }
        }

        private void LoadCourseProgressReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                string query = @"SELECT 
                                    c.course_code as 'COURSE',
                                    SUM(CASE WHEN LOWER(t.status) = 'pending' THEN 1 ELSE 0 END) as 'PENDING',
                                    SUM(CASE WHEN LOWER(t.status) = 'in progress' THEN 1 ELSE 0 END) as 'IN PROGRESS',
                                    SUM(CASE WHEN LOWER(t.status) = 'completed' THEN 1 ELSE 0 END) as 'COMPLETED',
                                    COUNT(*) as 'TOTAL'
                                 FROM task t
                                 LEFT JOIN course c ON t.course_id = c.course_id
                                 WHERE t.due_date BETWEEN @fromDate AND @toDate
                                 GROUP BY c.course_code
                                 ORDER BY c.course_code";

                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@fromDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@toDate", toDate.ToString("yyyy-MM-dd"));

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                currentDataTable = new DataTable();
                adapter.Fill(currentDataTable);
                dataGridView1.DataSource = currentDataTable;

                db.CloseConnection();

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.RowHeadersVisible = false;

                if (dataGridView1.Columns["PENDING"] != null)
                    dataGridView1.Columns["PENDING"].DefaultCellStyle.ForeColor = Color.Orange;
                if (dataGridView1.Columns["IN PROGRESS"] != null)
                    dataGridView1.Columns["IN PROGRESS"].DefaultCellStyle.ForeColor = Color.Blue;
                if (dataGridView1.Columns["COMPLETED"] != null)
                    dataGridView1.Columns["COMPLETED"].DefaultCellStyle.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading course progress report: " + ex.Message);
            }
        }

        private void LoadMemberTaskSummary(DateTime fromDate, DateTime toDate)
        {
            try
            {
                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                string query = @"SELECT 
                                    CONCAT(m.member_fname, ' ', m.member_lname) as 'MEMBER NAME',
                                    COUNT(CASE WHEN LOWER(t.status) = 'pending' THEN 1 END) as 'PENDING',
                                    COUNT(CASE WHEN LOWER(t.status) = 'in progress' THEN 1 END) as 'IN PROGRESS',
                                    COUNT(CASE WHEN LOWER(t.status) = 'completed' THEN 1 END) as 'COMPLETED',
                                    COUNT(*) as 'TOTAL TASKS'
                                 FROM task_assignment ta
                                 JOIN member m ON ta.member_id = m.member_id
                                 JOIN task t ON ta.task_id = t.task_id
                                 WHERE t.due_date BETWEEN @fromDate AND @toDate
                                 GROUP BY m.member_id, m.member_fname, m.member_lname
                                 ORDER BY COUNT(*) DESC";

                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@fromDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@toDate", toDate.ToString("yyyy-MM-dd"));

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                currentDataTable = new DataTable();
                adapter.Fill(currentDataTable);
                dataGridView1.DataSource = currentDataTable;

                db.CloseConnection();

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.RowHeadersVisible = false;

                if (dataGridView1.Columns["TOTAL TASKS"] != null)
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["TOTAL TASKS"].Value != null)
                        {
                            int total = Convert.ToInt32(row.Cells["TOTAL TASKS"].Value);
                            if (total >= 5)
                                row.Cells["TOTAL TASKS"].Style.ForeColor = Color.Red;
                            else if (total >= 3)
                                row.Cells["TOTAL TASKS"].Style.ForeColor = Color.Orange;
                            else
                                row.Cells["TOTAL TASKS"].Style.ForeColor = Color.Green;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading member task summary: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (currentDataTable == null || currentDataTable.Rows.Count == 0)
            {
                MessageBox.Show("No data to export. Please generate a report first.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files|*.xlsx";
            saveFileDialog.Title = "Save Report as Excel";
            saveFileDialog.FileName = comboBox1.SelectedItem.ToString().Replace(" ", "_") + "_" + DateTime.Now.ToString("yyyyMMdd");

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToExcel(saveFileDialog.FileName);
            }
        }

        private void ExportToExcel(string filePath)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet worksheet = workbook.ActiveSheet;
                Excel.Worksheet chartSheet = workbook.Worksheets.Add();
                chartSheet.Name = "Chart";

                worksheet.Name = "Report Data";

                worksheet.Cells.Font.Name = "Arial";
                chartSheet.Cells.Font.Name = "Arial";

                // HEADER WITH LOGO
                string logoPath = @"C:\Users\Acer\source\repos\tasked_forms\Resources\Union.png";
                if (System.IO.File.Exists(logoPath))
                {
                    try
                    {
                        Excel.Range logoCell = worksheet.Range["A1"];
                        Excel.Shape logo = worksheet.Shapes.AddPicture(logoPath,
                            MsoTriState.msoFalse, MsoTriState.msoCTrue,
                            (float)logoCell.Left + 200, (float)logoCell.Top, 45, 45);
                        logo.Width = 45;
                        logo.Height = 45;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Logo placement error: " + ex.Message);
                    }
                }

                worksheet.Cells[2, 1] = "TASKED";
                worksheet.Cells[2, 1].Font.Bold = true;
                worksheet.Cells[2, 1].Font.Size = 28;
                worksheet.Cells[2, 1].Font.Color = System.Drawing.Color.Blue;
                worksheet.Range["A2", "G2"].Merge();
                worksheet.Range["A2", "G2"].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                worksheet.Cells[3, 1] = "Task Management System";
                worksheet.Cells[3, 1].Font.Size = 12;
                worksheet.Cells[3, 1].Font.Italic = true;
                worksheet.Range["A3", "G3"].Merge();
                worksheet.Range["A3", "G3"].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                worksheet.Cells[4, 1] = "EST. 2026";
                worksheet.Cells[4, 1].Font.Size = 10;
                worksheet.Range["A4", "G4"].Merge();
                worksheet.Range["A4", "G4"].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                worksheet.Cells[6, 1] = textBox1.Text;
                worksheet.Cells[6, 1].Font.Bold = true;
                worksheet.Cells[6, 1].Font.Size = 14;
                worksheet.Range["A6", "G6"].Merge();
                worksheet.Range["A6", "G6"].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                worksheet.Cells[7, 1] = $"Report Period: {dateTimePicker1.Value.ToString("MMMM dd, yyyy")} - {dateTimePicker2.Value.ToString("MMMM dd, yyyy")}";
                worksheet.Range["A7", "G7"].Merge();
                worksheet.Range["A7", "G7"].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                worksheet.Cells[8, 1] = $"Generated on: {DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt")}";
                worksheet.Cells[8, 1].Font.Size = 9;
                worksheet.Range["A8", "G8"].Merge();
                worksheet.Range["A8", "G8"].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                worksheet.Rows[1].RowHeight = 55;
                worksheet.Rows[2].RowHeight = 35;

                // DATA GRID HEADERS
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    worksheet.Cells[10, i + 1] = dataGridView1.Columns[i].HeaderText;
                    worksheet.Cells[10, i + 1].Font.Bold = true;
                    worksheet.Cells[10, i + 1].Interior.Color = System.Drawing.Color.LightGray;
                    worksheet.Cells[10, i + 1].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }

                // DATA ROWS
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 11, j + 1] = dataGridView1.Rows[i].Cells[j].Value?.ToString();
                        worksheet.Cells[i + 11, j + 1].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    }
                }

                worksheet.Columns.AutoFit();

                // SIGNATURE
                int lastRow = dataGridView1.Rows.Count + 14;
                worksheet.Cells[lastRow + 2, 1] = "Prepared by:";
                worksheet.Cells[lastRow + 2, 1].Font.Bold = true;
                worksheet.Cells[lastRow + 2, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                worksheet.Cells[lastRow + 3, 1] = "_________________________";
                worksheet.Cells[lastRow + 4, 1] = Form1.loggedInUsername.ToUpper();
                worksheet.Cells[lastRow + 5, 1] = "System Administrator";
                worksheet.Cells[lastRow + 6, 1] = DateTime.Now.ToString("MMMM dd, yyyy");

                // ==========================================
                // GRAPH ON SHEET 2 - BASED ON REPORT TYPE
                // ==========================================

                taskisdbConnection db = taskisdbConnection.GetInstance();
                db.OpenConnection();

                int rowCount = 0;

                if (currentReportType == "Tasks Report")
                {
                    // Graph: Task Status Summary
                    int pendingCount = 0, inProgressCount = 0, completedCount = 0;
                    MySqlCommand pendingCmd = new MySqlCommand("SELECT COUNT(*) FROM task WHERE LOWER(status) = 'pending'", db.GetConnection());
                    pendingCount = Convert.ToInt32(pendingCmd.ExecuteScalar());
                    MySqlCommand progressCmd = new MySqlCommand("SELECT COUNT(*) FROM task WHERE LOWER(status) = 'in progress'", db.GetConnection());
                    inProgressCount = Convert.ToInt32(progressCmd.ExecuteScalar());
                    MySqlCommand completedCmd = new MySqlCommand("SELECT COUNT(*) FROM task WHERE LOWER(status) = 'completed'", db.GetConnection());
                    completedCount = Convert.ToInt32(completedCmd.ExecuteScalar());

                    chartSheet.Cells[1, 1] = "Status";
                    chartSheet.Cells[1, 2] = "Count";
                    chartSheet.Cells[2, 1] = "Pending";
                    chartSheet.Cells[2, 2] = pendingCount;
                    chartSheet.Cells[3, 1] = "In Progress";
                    chartSheet.Cells[3, 2] = inProgressCount;
                    chartSheet.Cells[4, 1] = "Completed";
                    chartSheet.Cells[4, 2] = completedCount;
                    rowCount = 4;
                }
                else if (currentReportType == "Members Report")
                {
                    // Graph: Active vs Inactive Members
                    int activeCount = 0, inactiveCount = 0;
                    MySqlCommand activeCmd = new MySqlCommand("SELECT COUNT(*) FROM accounts WHERE status = 'active'", db.GetConnection());
                    activeCount = Convert.ToInt32(activeCmd.ExecuteScalar());
                    MySqlCommand inactiveCmd = new MySqlCommand("SELECT COUNT(*) FROM accounts WHERE status = 'inactive'", db.GetConnection());
                    inactiveCount = Convert.ToInt32(inactiveCmd.ExecuteScalar());

                    chartSheet.Cells[1, 1] = "Status";
                    chartSheet.Cells[1, 2] = "Count";
                    chartSheet.Cells[2, 1] = "Active";
                    chartSheet.Cells[2, 2] = activeCount;
                    chartSheet.Cells[3, 1] = "Inactive";
                    chartSheet.Cells[3, 2] = inactiveCount;
                    rowCount = 3;
                }
                else if (currentReportType == "Task Assignments Report")
                {
                    // Graph: Course vs Number of Members Assigned
                    string query = @"SELECT 
                        c.course_code as Course,
                        COUNT(DISTINCT ta.member_id) as 'Members Assigned'
                    FROM task_assignment ta
                    JOIN task t ON ta.task_id = t.task_id
                    LEFT JOIN course c ON t.course_id = c.course_id
                    WHERE c.course_code IS NOT NULL
                    GROUP BY c.course_id, c.course_code
                    ORDER BY COUNT(DISTINCT ta.member_id) DESC";

                    MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                    MySqlDataReader reader = cmd.ExecuteReader();

                    chartSheet.Cells[1, 1] = "Course";
                    chartSheet.Cells[1, 2] = "Members Assigned";

                    int currentRow = 2;
                    while (reader.Read())
                    {
                        chartSheet.Cells[currentRow, 1] = reader["Course"].ToString();
                        chartSheet.Cells[currentRow, 2] = reader["Members Assigned"];
                        currentRow++;
                    }
                    reader.Close();
                    rowCount = currentRow - 1;
                }
                else if (currentReportType == "Course Progress Report")
                {
                    // Graph: Courses and Number of Tasks
                    string query = @"SELECT c.course_code as Course,
                                   COUNT(CASE WHEN LOWER(t.status) = 'pending' THEN 1 END) as Pending,
                                   COUNT(CASE WHEN LOWER(t.status) = 'in progress' THEN 1 END) as InProgress,
                                   COUNT(CASE WHEN LOWER(t.status) = 'completed' THEN 1 END) as Completed
                            FROM task t
                            LEFT JOIN course c ON t.course_id = c.course_id
                            GROUP BY c.course_id";
                    MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                    MySqlDataReader reader = cmd.ExecuteReader();

                    chartSheet.Cells[1, 1] = "Course";
                    chartSheet.Cells[1, 2] = "Pending";
                    chartSheet.Cells[1, 3] = "In Progress";
                    chartSheet.Cells[1, 4] = "Completed";

                    int currentRow = 2;
                    while (reader.Read())
                    {
                        string course = reader["Course"].ToString();
                        if (!string.IsNullOrEmpty(course))
                        {
                            chartSheet.Cells[currentRow, 1] = course;
                            chartSheet.Cells[currentRow, 2] = reader["Pending"];
                            chartSheet.Cells[currentRow, 3] = reader["InProgress"];
                            chartSheet.Cells[currentRow, 4] = reader["Completed"];
                            currentRow++;
                        }
                    }
                    reader.Close();
                    rowCount = currentRow - 1;
                }
                else if (currentReportType == "Member Task Summary")
                {
                    // Graph: Members and Number of Tasks Assigned
                    string query = @"SELECT CONCAT(m.member_fname, ' ', m.member_lname) as Member,
                                   COUNT(CASE WHEN LOWER(t.status) = 'pending' THEN 1 END) as Pending,
                                   COUNT(CASE WHEN LOWER(t.status) = 'in progress' THEN 1 END) as InProgress,
                                   COUNT(CASE WHEN LOWER(t.status) = 'completed' THEN 1 END) as Completed
                            FROM task_assignment ta
                            JOIN member m ON ta.member_id = m.member_id
                            JOIN task t ON ta.task_id = t.task_id
                            GROUP BY m.member_id
                            ORDER BY COUNT(*) DESC
                            LIMIT 10";
                    MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                    MySqlDataReader reader = cmd.ExecuteReader();

                    chartSheet.Cells[1, 1] = "Member";
                    chartSheet.Cells[1, 2] = "Pending";
                    chartSheet.Cells[1, 3] = "In Progress";
                    chartSheet.Cells[1, 4] = "Completed";

                    int currentRow = 2;
                    while (reader.Read())
                    {
                        chartSheet.Cells[currentRow, 1] = reader["Member"].ToString();
                        chartSheet.Cells[currentRow, 2] = reader["Pending"];
                        chartSheet.Cells[currentRow, 3] = reader["InProgress"];
                        chartSheet.Cells[currentRow, 4] = reader["Completed"];
                        currentRow++;
                    }
                    reader.Close();
                    rowCount = currentRow - 1;
                }

                db.CloseConnection();

                // CREATE THE CHART
                if (rowCount > 1)
                {
                    Excel.ChartObjects chartObjects = (Excel.ChartObjects)chartSheet.ChartObjects();
                    Excel.ChartObject chartObject = chartObjects.Add(100, 50, 600, 400);
                    Excel.Chart chart = chartObject.Chart;

                    // Determine chart range based on report type
                    string chartRangeAddress = "";
                    if (currentReportType == "Tasks Report" || currentReportType == "Members Report" || currentReportType == "Task Assignments Report")
                    {
                        chartRangeAddress = $"A1:B{rowCount}";
                    }
                    else
                    {
                        chartRangeAddress = $"A1:D{rowCount}";
                    }

                    Excel.Range chartRange = chartSheet.Range[chartRangeAddress];
                    chart.SetSourceData(chartRange);
                    chart.ChartType = Excel.XlChartType.xlColumnClustered;
                    chart.HasTitle = true;

                    if (currentReportType == "Tasks Report")
                        chart.ChartTitle.Text = "Task Status Summary";
                    else if (currentReportType == "Members Report")
                        chart.ChartTitle.Text = "Active vs Inactive Members";
                    else if (currentReportType == "Task Assignments Report")
                        chart.ChartTitle.Text = "Number of Members Assigned per Course";
                    else if (currentReportType == "Course Progress Report")
                        chart.ChartTitle.Text = "Course Task Progress (by Status)";
                    else if (currentReportType == "Member Task Summary")
                        chart.ChartTitle.Text = "Member Workload Summary (by Status)";

                    chart.ChartTitle.Font.Size = 14;
                    chart.ChartTitle.Font.Bold = true;

                    if (chart.SeriesCollection().Count > 0)
                    {
                        chart.SeriesCollection(1).HasDataLabels = true;
                        chart.SeriesCollection(1).DataLabels.Font.Size = 10;
                    }
                }

                workbook.SaveAs(filePath);
                workbook.Close();
                excelApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(chartSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                MessageBox.Show($"Report exported successfully!\n\nFile saved to:\n{filePath}",
                                "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export error: " + ex.Message);
            }
        }

        // EMPTY EVENTS
        private void Form5_Load(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void panel4_Paint(object sender, PaintEventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e) { }
        private void checkBox2_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}