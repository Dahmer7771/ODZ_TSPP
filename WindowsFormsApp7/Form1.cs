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
        public string connectionString = ";;;";
        public MySqlConnection connection;

        public void ShowConnectionParams()
        {
            connection = new MySqlConnection(connectionString);
            string[] connectionParams = connectionString.Split(';');
            connectionParams[0] = connectionParams[0].Replace("=", " = ");
            connectionParams[1] = connectionParams[1].Replace("=", " = ");
            connectionParams[2] = connectionParams[2].Replace("=", " = ");
            label4.Text = $"{connectionParams[0]}";
            label5.Text = $"{connectionParams[1]}";
            label6.Text = $"{connectionParams[2]}";
        }

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
            try
            {
                ShowConnectionParams();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AboutUs aboutUs = new AboutUs(this);
            aboutUs.Show();

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "HH:mm";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";
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
            finally
            {
                connection.Close();
            }
        }

        private void selectedRadioButton()
        {
            string punkt_B = Convert.ToString(comboBox2.SelectedItem);
            string time_start = Convert.ToString(dateTimePicker1.Text);
            string flight_date = Convert.ToString(dateTimePicker2.Text);

            if (radioButton1.Checked == true)
            {
                try
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter($"select id_flight `Номер рейса`, id_plane `Номер самолета`, time_start `Время вылета`, time_end `Время прибытия`, flight_date `Дата вылета`, arrival_date `Дата прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights where punkt_B = '{punkt_B}' and time_start = '{time_start}' and flight_date = '{flight_date}'", connection);
                    readFromTable(adapter);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            } else if(radioButton2.Checked == true)
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter($"select id_flight `Номер рейса`, id_plane `Номер самолета`, time_start `Время вылета`, time_end `Время прибытия`, flight_date `Дата вылета`, arrival_date `Дата прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights where punkt_B = '{punkt_B}'", connection);
                readFromTable(adapter);
            }
            else
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter($"select id_flight `Номер рейса`, id_plane `Номер рейса`, time_start `Время самолета`, time_end `Время прибытия`, flight_date `Дата вылета`, arrival_date `Дата прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights where time_start = '{time_start}' and  flight_date = '{flight_date}'", connection);
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
                        $"Номер самолета: {cellItem[1].Value} \n" +
                        $"Время вылета: {cellItem[2].Value} \n" +
                        $"Время прибытия: {cellItem[3].Value} \n" +
                        $"Дата вылета: {cellItem[4].Value = Convert.ToString(cellItem[4].Value).Split(' ')[0]} \n" +
                        $"Дата прибытия: {cellItem[5].Value = Convert.ToString(cellItem[5].Value).Split(' ')[0]} \n" +
                        $"Свободные места эконом класса: {cellItem[6].Value} \n" +
                        $"Свободные места бизнес класса: {cellItem[7].Value} \n" +
                        $"Рейс в: {cellItem[8].Value} \n" +
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
            try
            {
                Administration administration = new Administration(this, connectionString);
                administration.Show();
                this.Hide();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Clear(DataGridView dataGridView)
        {
            while (dataGridView.Rows.Count > 1)
                for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
                    dataGridView.Rows.Remove(dataGridView.Rows[i]);
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
  
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("select id_flight `Номер рейса`, id_plane `Номер самолета`, time_start `Время вылета`, time_end `Время прибытия`, flight_date `Дата вылета`, arrival_date `Дата прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights", connection);
                readFromTable(adapter);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void подключениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connection connection = new Connection(this);
            connection.Show();
        }
    }
}
