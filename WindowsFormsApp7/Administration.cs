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
        private MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;");

        public Administration(Form1 refForm1)
        {
            InitializeComponent();
            this.refForm1 = refForm1;
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
            MySqlDataAdapter adapter = new MySqlDataAdapter("select id_plane `Номер рейса`, time_start `Время вылета`, time_end `Время прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights", connection);
            readFromTable(adapter);
        }

        private void Administration_FormClosed(object sender, FormClosedEventArgs e)
        {
            refForm1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter($"delete from Flying.Flights where id_plane = {dataGridView1.CurrentRow.Cells[0].Value} and time_start = '{dataGridView1.CurrentRow.Cells[1].Value}'", connection);
            readFromTable(adapter);

            MySqlDataAdapter adapter2 = new MySqlDataAdapter("select id_plane `Номер рейса`, time_start `Время вылета`, time_end `Время прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights", connection);
            readFromTable(adapter2);
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
                    MySqlCommand command2 = new MySqlCommand($"update Flying.Flights set `id_plane` = {massiv[i, 0]}, `time_start` = '{massiv[i, 1]}', `time_end` = '{massiv[i, 2]}', `free_count_econom` = {massiv[i, 3]}, `free_count_business` = {massiv[i, 4]}, `punkt_B` = '{massiv[i, 5]}' where id_plane = {dataGridView1[0, i].Value.ToString()}", connection);
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
                                MySqlDataAdapter adapter = new MySqlDataAdapter("select id_plane `Номер рейса`, time_start `Время вылета`, time_end `Время прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights", connection);
                                readFromTable(adapter);
                            }
                            normalRow = false;
                            break;
                        }
                    }

                    if (normalRow)
                    {
                        var cellItem = dataGridView1.Rows[i];
                        MySqlCommand command2 = new MySqlCommand($"INSERT INTO Flying.Flights" +
                            $"(`id_plane`, `time_start`, `time_end`, `free_count_econom`, `free_count_business`, `punkt_B`) " +
                            $"VALUES (" +
                            $"{cellItem.Cells[0].Value}, " +
                            $"'{cellItem.Cells[1].Value.ToString()}', " +
                            $"'{cellItem.Cells[2].Value.ToString()}', " +
                            $"{cellItem.Cells[3].Value}, " +
                            $"{cellItem.Cells[4].Value}, " +
                            $"'{cellItem.Cells[5].Value.ToString()}')", connection);

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

        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(text: "Неверно введены данные!", buttons: MessageBoxButtons.RetryCancel, icon: MessageBoxIcon.Error, caption: "Ошибка");
            if (dialogResult == DialogResult.Cancel)
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("select id_plane `Номер рейса`, time_start `Время вылета`, time_end `Время прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights", connection);
                readFromTable(adapter);
            }
        }
    }
}
