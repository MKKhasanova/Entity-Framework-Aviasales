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
    public partial class Form4 : Form
    {
        Database1Entities db = new Database1Entities();
        public Form4()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form4_FormClosing);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            Hide();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            var korzinas = from korzina in db.History
                           join genre in db.Client on korzina.Client_ID equals genre.Client_ID
                           select new
                           {
                               Id = korzina.Id,
                               Client_ID = genre.Client_ID,
                               Flight_number = korzina.Flight_number,
                               Otkuda = korzina.Otkuda,
                               Kuda = korzina.Kuda,
                               Vremiya_otpravleniya = korzina.Vremya_otpravlenya

                           };
            dataGridView1.DataSource = korzinas.ToList();
            dataGridView2.DataSource = db.Client.ToList<Client>();
            dataGridView3.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)// скрыть историю читат
        {
            dataGridView3.Visible = false;
            textBox1.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /* 
             DataGridViewRow curRow = dataGridView1.CurrentRow;

             // Получение значений ячеек выбранной строки
             string flightNumber = curRow.Cells["Flight_number"].Value.ToString();
             string otkuda = curRow.Cells["Otkuda"].Value.ToString();
             string kuda = curRow.Cells["Kuda"].Value.ToString();
             //TimeSpan? vremyaOtpravlenya = curRow.Cells["Vremya_otpravlenya"].Value as TimeSpan?;
             int clientID = Convert.ToInt32(curRow.Cells["Client_ID"].Value);

             // Создание объекта для добавления в таблицу Istory
             Istory istory = new Istory
             {
                 Client_ID = clientID,
                 Flight_number = flightNumber,
                 Otkuda = otkuda,
                 Kuda = kuda
             };

             // Добавление объекта в таблицу Istory
             db.Istorys.Add(istory);

             // Сохранение изменений в базе данных
             db.SaveChanges();
             db.Istorys.Remove(istory);
             db.SaveChanges();
             MessageBox.Show("Заказ успешно оформлен!");*/
            // dataGridView1.DataSource = db.Istorys.ToList<Istory>();
            DataGridViewRow curRow = dataGridView2.CurrentRow;
            DataGridViewRow curRows = dataGridView1.CurrentRow;
            string Client_Name = curRow.Cells["Name"].Value.ToString();
            Client selectedClient = db.Client.Where(x => x.Name == Client_Name).FirstOrDefault();
            int Client_ID = selectedClient.Client_ID;
            string Otk = curRows.Cells["Otkuda"].Value.ToString();
            string kuda = curRows.Cells["Kuda"].Value.ToString();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string Flight_number = row.Cells["Flight_number"].Value.ToString();
               /* History selectedBook = db.History.Where(x => x.Flight_number == Flight_number).FirstOrDefault();
                int Ticket_Id = (int)selectedBook.Tic;*/
                Istory newLoan = new Istory();
                //newLoan.Istori_ID = Client_ID;
                newLoan.Client_ID = Client_ID;
                //newLoan.Loan_Date = DateTime.Today;
                //newLoan.Return_Date = DateTime.Today.AddDays(15);
                newLoan.Flight_number = Flight_number;
                newLoan.Otkuda = Otk;
                newLoan.Kuda = kuda;
                int newId = db.Istorys.Max(x => x.Istori_ID) + 1;
                newLoan.Istori_ID = newId;
                db.Istorys.Add(newLoan);
                db.SaveChanges();
            }
            MessageBox.Show($"Заказ успешно оформлен на пользователя {Client_Name}");
            //очищение корзины
            List<History> korzinaToDelete = db.History.ToList();
            db.History.RemoveRange(korzinaToDelete);
            db.SaveChanges();
            var korzinas = from korzina in db.History
                           join genre in db.Client on korzina.Client_ID equals genre.Client_ID
                           select new
                           {
                               Id = korzina.Id,
                               Client_ID = genre.Client_ID,
                               Flight_number = korzina.Flight_number,
                               Otkuda = korzina.Otkuda,
                               Kuda = korzina.Kuda,
                               Vremiya_otpravleniya = korzina.Vremya_otpravlenya

                           };
            dataGridView1.DataSource = korzinas.ToList();



            //обновление списка выдач
            var ist = from istor in db.Istorys
                           join genre in db.Client on istor.Client_ID equals genre.Client_ID
                           select new
                           {
                               Id = istor.Istori_ID,
                               
                               Flight_number = istor.Flight_number,
                               Otkuda = istor.Otkuda,
                               Kuda = istor.Kuda
                           };
            dataGridView3.DataSource = ist.ToList();
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            db.SaveChanges();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                int clientID = Convert.ToInt32(row.Cells["Client_ID"].Value);
                textBox1.Text = dataGridView2.CurrentRow.Cells["Name"].Value.ToString();
                // Выбор данных из таблицы Istory по Client_ID
                var historyData = from history in db.Istorys
                                  where history.Client_ID == clientID
                                  select new
                                  {
                                      history.Istori_ID,
                                      history.Flight_number,
                                      history.Otkuda,
                                      history.Kuda
                                  };
                dataGridView3.DataSource = historyData.ToList();
            }
        }

        private void button4_Click(object sender, EventArgs e)//показать историю читателя
        {

            if (dataGridView2.CurrentRow != null)
            {
                int clientID = Convert.ToInt32(dataGridView2.CurrentRow.Cells["Client_ID"].Value);
                textBox1.Text = dataGridView2.CurrentRow.Cells["Name"].Value.ToString();
               
                var historyData = from history in db.Istorys
                                  where history.Client_ID == clientID
                                  select new
                                  {
                                      history.Istori_ID,
                                      history.Flight_number,
                                      history.Otkuda,
                                      history.Kuda
                                  };

                // Установка результатов выборки в качестве источника данных для dataGridView3
                dataGridView3.DataSource = historyData.ToList();
               
            }

           dataGridView3.Visible = true;
        }
    }
}
