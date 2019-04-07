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
    public partial class Connection : Form
    {
        private Form1 form1;
        public string connectionString;

        public Connection(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
            textBox4.PasswordChar = ('*');
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                connectionString = $"server={textBox1.Text};" +
                $"port={textBox2.Text};" +
                $"username={textBox3.Text};" +
                $"password={textBox4.Text};";

                this.form1.comboBox2.Items.Clear();

                this.form1.connection = new MySqlConnection(connectionString);
                this.form1.connectionString = connectionString;
                MySqlDataAdapter adapter = new MySqlDataAdapter("select id_flight `Номер рейса`, id_plane `Номер самолета`, time_start `Время вылета`, time_end `Время прибытия`, flight_date `Дата вылета`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights", this.form1.connection);
                this.form1.readFromTable(adapter);
                this.form1.connection.Open();

                MySqlCommand command = new MySqlCommand("select distinct punkt_B from Flying.Flights", this.form1.connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    this.form1.comboBox2.Items.Add(reader[0].ToString());
                }

                this.form1.connection.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                this.form1.comboBox2.Items.Clear();
                this.form1.Clear(this.form1.dataGridView1);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.form1.connectionString = connectionString;
                this.form1.ShowConnectionParams();
                this.form1.Show();
                this.Close();
            }
        }

        private void Connection_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.form1.Show();
        }
    }
}
