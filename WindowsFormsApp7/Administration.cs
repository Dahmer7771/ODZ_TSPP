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

namespace WindowsFormsApp7
{
    public partial class Administration : Form
    {
        private Form1 refForm1;
        private MySqlConnection connection;

        public Administration(Form1 refForm1, string connectionString)
        {
            InitializeComponent();
            this.refForm1 = refForm1;
            try
            {
                connection = new MySqlConnection(connectionString);
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

        private void readFromTable(MySqlDataAdapter adapter)
        {
            connection.Open();

            DataSet ds = new DataSet();
            adapter.Fill(ds, "Flights");
            dataGridView1.DataSource = ds.Tables["Flights"];

            connection.Close();
        }

        private void Administration_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("select id_plane `Номер самолета`, time_start `Время вылета`, time_end `Время прибытия`, flight_date `Дата вылета`, arrival_date `Дата прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights", connection);
                readFromTable(adapter);
                dataGridView1.Columns[3].DefaultCellStyle.Format = "yyyy-MM-dd";
                dataGridView1.Columns[4].DefaultCellStyle.Format = "yyyy-MM-dd";
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

        private void Administration_FormClosed(object sender, FormClosedEventArgs e)
        {
            refForm1.Show();
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("select id_flight `Номер рейса`, id_plane `Номер самолета`, time_start `Время вылета`, time_end `Время прибытия`, flight_date `Дата вылета`, arrival_date `Дата прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights", connection);
                this.refForm1.readFromTable(adapter);

                this.refForm1.comboBox2.Items.Clear();

                this.refForm1.connection.Open();

                MySqlCommand command = new MySqlCommand("select distinct punkt_B from Flying.Flights", this.refForm1.connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    this.refForm1.comboBox2.Items.Add(reader[0].ToString());
                }

                this.refForm1.connection.Close();
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string[] date_massive = Convert.ToString(dataGridView1.CurrentRow.Cells[3].Value).Split(' ')[0].Split('.');
                string date_format = $"{date_massive[2]}-{date_massive[1]}-{date_massive[0]}";
                string[] date_massive2 = Convert.ToString(dataGridView1.CurrentRow.Cells[4].Value).Split(' ')[0].Split('.');
                string date_format2 = $"{date_massive2[2]}-{date_massive2[1]}-{date_massive2[0]}";

                MessageBox.Show(date_format);
                MySqlDataAdapter adapter = new MySqlDataAdapter($"delete from Flying.Flights where id_plane = {dataGridView1.CurrentRow.Cells[0].Value} " +
                    $"and time_start = '{dataGridView1.CurrentRow.Cells[1].Value}' " +
                    $"and time_end = '{dataGridView1.CurrentRow.Cells[2].Value}' " +
                    $"and flight_date = '{date_format}'" +
                    $" and arrival_date = '{date_format2}'" +
                    $"and free_count_econom = {dataGridView1.CurrentRow.Cells[4].Value} " +
                    $"and free_count_business = {dataGridView1.CurrentRow.Cells[5].Value} " +
                    $"and punkt_B = '{dataGridView1.CurrentRow.Cells[6].Value}';", 
                    connection);
                readFromTable(adapter);

                MySqlDataAdapter adapter2 = new MySqlDataAdapter("select id_plane `Номер самолета`, time_start `Время вылета`, time_end `Время прибытия`, flight_date `Дата вылета`, arrival_date `Дата прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights;", connection);
                readFromTable(adapter2);
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Начальное количество записей в БД
                MySqlCommand command = new MySqlCommand("select count(id_flight) from Flying.Flights", connection);
                connection.Open();
                int rowsInTable = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();

                string[,] massiv = new string[rowsInTable, dataGridView1.ColumnCount];

                for (int i = 0; i < rowsInTable; i++)
                {
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        massiv[i, j] = dataGridView1[j, i].Value.ToString();
                    }

                    string[] date_massive = Convert.ToString(massiv[i, 3]).Split(' ')[0].Split('.');
                    string date_format = $"{date_massive[2]}-{date_massive[1]}-{date_massive[0]}";
                    string[] date_massive2 = Convert.ToString(massiv[i, 4]).Split(' ')[0].Split('.');
                    string date_format2 = $"{date_massive2[2]}-{date_massive2[1]}-{date_massive2[0]}";

                    MySqlCommand command2 = new MySqlCommand($"update Flying.Flights set `id_plane` = {massiv[i, 0]}, `time_start` = '{massiv[i, 1]}', `time_end` = '{massiv[i, 2]}', `flight_date` = '{date_format}', `arrival_date` = '{date_format2}', `free_count_econom` = {massiv[i, 4]}, `free_count_business` = {massiv[i, 5]}, `punkt_B` = '{massiv[i, 6]}' where id_plane = {dataGridView1[0, i].Value.ToString()}", connection);
                    connection.Open();
                    command2.ExecuteNonQuery();
                    connection.Close();
                }

                for (int i = rowsInTable; i < dataGridView1.RowCount-1; i++)
                {
                    bool normalRow = true;
                    for (int j = 0; j < dataGridView1.Rows[0].Cells.Count; j++)
                    {

                        if (Convert.ToString(dataGridView1.Rows[i].Cells[j].Value).Length < 1)
                        {
                            DialogResult dialogResult = MessageBox.Show(text: $"Не все поля заполнены!", buttons: MessageBoxButtons.RetryCancel, icon: MessageBoxIcon.Error, caption: "Ошибка");
                            if(dialogResult == DialogResult.Cancel)
                            {
                                this.Clear(dataGridView1);
                                MySqlDataAdapter adapter = new MySqlDataAdapter("select id_plane `Номер самолета`, time_start `Время вылета`, time_end `Время прибытия`, flight_date `Дата вылета`, arrival_date `Дата прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights", connection);
                                readFromTable(adapter);
                                normalRow = false;
                                break;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }

                    if (normalRow)
                    {
                        var cellItem = dataGridView1.Rows[i];
                        string[] date_massive = Convert.ToString(cellItem.Cells[3].Value).Split(' ')[0].Split('.');
                        string date_format = $"{date_massive[2]}-{date_massive[1]}-{date_massive[0]}";
                        string[] date_massive2 = Convert.ToString(cellItem.Cells[4].Value).Split(' ')[0].Split('.');
                        string date_format2 = $"{date_massive2[2]}-{date_massive2[1]}-{date_massive2[0]}";

                        MySqlCommand command2 = new MySqlCommand($"INSERT INTO Flying.Flights" +
                            $"(`id_plane`, `time_start`, `time_end`, `flight_date`, `arrival_date`, `free_count_econom`, `free_count_business`, `punkt_B`) " +
                            $"VALUES (" +
                            $"{cellItem.Cells[0].Value}, " +
                            $"'{cellItem.Cells[1].Value.ToString()}', " +
                            $"'{cellItem.Cells[2].Value.ToString()}', " +
                            $"'{date_format}', " +
                            $"'{date_format2}'" +
                            $"{cellItem.Cells[5].Value}, " +
                            $"{cellItem.Cells[6].Value}, " +
                            $"'{cellItem.Cells[7].Value.ToString()}')", connection);

                        connection.Open();
                        command2.ExecuteNonQuery();
                        connection.Close();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(text: "Неверный тип введенных данных!", buttons: MessageBoxButtons.RetryCancel, icon: MessageBoxIcon.Error, caption: "Ошибка");
            }
            finally
            {
                connection.Close();
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show(text: "Неверно введены данные!", buttons: MessageBoxButtons.RetryCancel, icon: MessageBoxIcon.Error, caption: "Ошибка");
                if (dialogResult == DialogResult.Cancel)
                {
                    this.Clear(dataGridView1);
                    MySqlDataAdapter adapter = new MySqlDataAdapter("select id_plane `Номер самолета`, time_start `Время вылета`, time_end `Время прибытия`, flight_date `Дата вылета`, arrival_date `Дата прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights", connection);
                    readFromTable(adapter);
                    dataGridView1.Columns[3].DefaultCellStyle.Format = "yyyy-MM-dd";
                    dataGridView1.Columns[4].DefaultCellStyle.Format = "yyyy-MM-dd";
                }
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
    }
}
