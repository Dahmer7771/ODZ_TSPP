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

        public Connection(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = $"server={textBox1.Text};" +
                $"port={textBox2.Text};" +
                $"username={textBox3.Text};" +
                $"password={textBox4.Text};";
                this.form1.connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                MySqlDataAdapter adapter = new MySqlDataAdapter("select id_plane `Номер рейса`, time_start `Время вылета`, time_end `Время прибытия`, free_count_econom `Билеты эконом класса`, free_count_business `Билеты бизнесс класса`, punkt_B `Место назначение` from Flying.Flights", this.form1.connection);
                this.form1.readFromTable(adapter);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
