using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.VisualBasic;

namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {
        public string connectionString = "server=localhost;port=3306;username=root;password=;";
        public MySqlConnection connection;

        public void readFromTable(MySqlDataAdapter adapter)
        {
            connection.Open();

            DataSet ds = new DataSet();
            adapter.Fill(ds, "Flights");
            dataGridView1.DataSource = ds.Tables["Flights"];

            connection.Close();
        }

        public Form1()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AboutUs aboutUs = new AboutUs(this);
            aboutUs.Show();

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "HH:mm:ss";

            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("select * from Flying.Flights", connection);
                readFromTable(adapter);

                connection.Open();

                MySqlCommand command = new MySqlCommand("select punkt_B from Flying.Flights", connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBox2.Items.Add(reader[0].ToString());
                }

                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string punkt_B = Convert.ToString(comboBox2.SelectedItem);
                string time_start = Convert.ToString(dateTimePicker1.Text);

                selectedRadioButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void selectedRadioButton()
        {
            string punkt_B = Convert.ToString(comboBox2.SelectedItem);
            string time_start = Convert.ToString(dateTimePicker1.Text);

            if (radioButton1.Checked == true)
            {
                try
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter($"select * from Flying.Flights where punkt_B = '{punkt_B}' and time_start = '{time_start}'", connection);
                    readFromTable(adapter);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            } else if(radioButton2.Checked == true)
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter($"select id_plane `Номер рейса`, time_start `Время вылета`, time_end `Время прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights where punkt_B = '{punkt_B}'", connection);
                readFromTable(adapter);
            }
            else
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter($"select id_plane `Номер рейса`, time_start `Время вылета`, time_end `Время прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights where time_start = '{time_start}'", connection);
                readFromTable(adapter);
            }
        }

        private void экспортToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                var wordApp = new Word.Application();
                wordApp.Visible = false;
                var wordDocument = wordApp.Documents.Add();

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    var paragraphone = wordDocument.Content.Paragraphs.Add();
                    var cellItem = dataGridView1.Rows[i].Cells;

                    string documentContent =
                        $"Номер рейса: {cellItem[0].Value} \n" +
                        $"Время вылета: {cellItem[1].Value} \n" +
                        $"Время прибытия: {cellItem[2].Value} \n" +
                        $"Свободные места эконом класса: {cellItem[3].Value} \n" +
                        $"Свободные места бизнес класса: {cellItem[4].Value} \n" +
                        $"Рейс в: {cellItem[5].Value} \n" +
                        $"{new string('*', 80)} \n";

                    paragraphone.Range.Text = documentContent;
                }

                wordApp.Visible = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        private void экспортToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Administration administration = new Administration(this);
            administration.Show();
            this.Hide();
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter("select id_plane `Номер рейса`, time_start `Время вылета`, time_end `Время прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights", connection);
            readFromTable(adapter);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(connectionString);
            MySqlDataAdapter adapter = new MySqlDataAdapter("select id_plane `Номер рейса`, time_start `Время вылета`, time_end `Время прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights", connection);
            readFromTable(adapter);
        }

        private void подключениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connection connection = new Connection(this);
            connection.Show();
        }
    }
}
