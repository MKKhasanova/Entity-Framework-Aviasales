using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp7
{
    public partial class Form2 : Form
    {
        public Form1 form1;
        private Client selectedClient;
        Database1Entities db = new Database1Entities();
        public Form2(Client client)
        {
            InitializeComponent();
            selectedClient = client;

            // Использовать данные клиента для инициализации элементов управления в Form2
            textBox1.Text = selectedClient.Name;
            textBox2.Text = selectedClient.Address;
            textBox3.Text = selectedClient.Passport;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получите отредактированные данные из TextBox
            selectedClient.Name = textBox1.Text;
            selectedClient.Address = textBox2.Text;
            selectedClient.Passport = textBox3.Text;
            db.SaveChanges();
            Close();
        }

    }
}
