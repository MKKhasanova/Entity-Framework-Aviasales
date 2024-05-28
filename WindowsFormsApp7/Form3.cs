using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp7
{
    public partial class Form3 : Form
    {
        Database1Entities db = new Database1Entities();
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получить максимальное значение идентификатора из существующих записей
            int maxId = db.Client.Max(c => c.Client_ID);

            Client newClient = new Client()
            {
                Client_ID = maxId + 1,
                Name = textBox1.Text,
                Address = textBox2.Text,
                Passport = textBox3.Text,
                Year_Of_Birth = DateTime.Parse(textBox4.Text)
            };

            // Получить экземпляр Form1 из контекста приложения
            Form1 form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (form1 != null)
            {
                // Добавить нового клиента в таблицу на Form1
                form1.AddClient(newClient);
            }

            // Закрыть текущую форму (Form3)
            Close();
        }
    }
}
