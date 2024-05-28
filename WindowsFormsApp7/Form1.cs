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
    public partial class Form1 : Form
    {
        Database1Entities db = new Database1Entities();
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }
        private void UpdateDataGridView()//обновление dataGridView1
        {
            var ticketsData = from ticket in db.Tickets
                              join runway in db.Runways on ticket.Runway_ID equals runway.Runway_ID
                              join transplantation in db.Transplantations on ticket.Transplantation_ID equals transplantation.Transplantation_ID
                              select new
                              {
                                  Ticket_Id = ticket.Ticket_Id,
                                  Flight_number = ticket.Flight_number,
                                  Otkuda_countr = ticket.Otk_Count,
                                  Otkuda = ticket.Otkuda,
                                  Kuda_countr = ticket.Kuda,
                                  Kuda = ticket.Kuda,
                                  Vremya_otpravlenya = ticket.Vremya_otpravlenya,
                                  Kol_svobod_biletov = ticket.Kol_svobod_biletov,
                                  Price = ticket.Price,

                              };
            dataGridView1.DataSource = ticketsData.ToList();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            UpdateDataGridView();

            // Заполнение ComboBox1 данными из столбца Otkuda
            var distinctOtkuda = db.Tickets.Select(t => t.Otkuda).Distinct().ToList();
            comboBox1.Items.Add("Города");
            comboBox1.Items.AddRange(distinctOtkuda.ToArray());
            comboBox1.SelectedItem = "Города";

            // Заполнение ComboBox2 данными из столбца Kuda
            var distinctKuda = db.Tickets.Select(t => t.Kuda).Distinct().ToList();
            comboBox2.Items.Add("Города");
            comboBox2.Items.AddRange(distinctKuda.ToArray());
            comboBox2.SelectedItem = "Города";
            LoadComboBox3Data();
            dataGridView2.DataSource = db.Client.ToList<Client>();
            dataGridView3.DataSource = db.History.ToList<History>();
            // Заполнение ComboBox4 данными из столбца откуда
            var distinctStrOtkuda = db.Tickets.Select(t => t.Otk_Count).Distinct().ToList();
            comboBox4.Items.Add("Страны");
            comboBox4.Items.AddRange(distinctStrOtkuda.ToArray());
            comboBox4.SelectedItem = ("Страны");
            //Заполнение ComboBox5 куда
            var distinctStrKuda = db.Tickets.Select(t => t.Kud_Count).Distinct().ToList();
            comboBox5.Items.Add("Страны");
            comboBox5.Items.AddRange(distinctStrKuda.ToArray());
            comboBox5.SelectedItem = "Страны";
            LoadComboBox3Data();
            dataGridView2.DataSource = db.Client.ToList<Client>();
            dataGridView3.DataSource = db.History.ToList<History>();


        }
        private void LoadComboBox3Data()
        {
            List<int> ids = db.Client.Select(item => item.Client_ID).ToList();

           
            ids.Insert(0, 0);

            comboBox3.DataSource = ids;
            comboBox3.SelectedIndex = 0;  

        }

        private void ApplyFilters()
        {
            string selectedValue1 = comboBox1.SelectedItem?.ToString() ?? "Города";
            string selectedValue2 = comboBox2.SelectedItem?.ToString() ?? "Города";
            string selectedValue4 = comboBox4.SelectedItem?.ToString() ?? "Страны";
            string selectedValue5 = comboBox5.SelectedItem?.ToString() ?? "Страны";

            var filteredData = from ticket in db.Tickets
                               join runway in db.Runways on ticket.Runway_ID equals runway.Runway_ID
                               join transplantation in db.Transplantations on ticket.Transplantation_ID equals transplantation.Transplantation_ID
                               select new
                               {
                                   Ticket_Id = ticket.Ticket_Id,
                                   Flight_number = ticket.Flight_number,
                                   Otkuda_countr = ticket.Otk_Count,
                                   Otkuda = ticket.Otkuda,
                                   Kuda_countr = ticket.Kud_Count,
                                   Kuda = ticket.Kuda,
                                   Vremya_otpravlenya = ticket.Vremya_otpravlenya,
                                   Kol_svobod_biletov = ticket.Kol_svobod_biletov,
                                   Price = ticket.Price,
                                  
                               };

            if (selectedValue1 != "Города" && !string.IsNullOrEmpty(selectedValue1))
            {
                filteredData = filteredData.Where(p => p.Otkuda == selectedValue1);
            }

            if (selectedValue2 != "Города" && !string.IsNullOrEmpty(selectedValue2))
            {
                filteredData = filteredData.Where(p => p.Kuda == selectedValue2);
            }
            if (selectedValue4 != "Страны" && !string.IsNullOrEmpty(selectedValue4))
            {
                filteredData = filteredData.Where(p => p.Otkuda_countr == selectedValue4);
            }

            if (selectedValue5 != "Страны" && !string.IsNullOrEmpty(selectedValue5))
            {
                filteredData = filteredData.Where(p => p.Kuda_countr == selectedValue5);
            }

            dataGridView1.DataSource = filteredData.ToList();
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedClientID = (int)comboBox3.SelectedItem;

            if (selectedClientID == 0)
            {
                // Отобразить все элементы
                dataGridView2.DataSource = db.Client.ToList();
            }
            else
            {
                // Фильтровать по выбранному Client_ID
                dataGridView2.DataSource = db.Client.Where(history => history.Client_ID == selectedClientID).ToList();
            }
        }

        private void button1_Click(object sender, EventArgs e) //редактировать 
        {
            int clientId = (int)comboBox3.SelectedValue;
            Client selectedClient = db.Client.FirstOrDefault(c => c.Client_ID == clientId);

            // Создать экземпляр Form2 и передать выбранный клиент для отображения и редактирования
            Form2 form2 = new Form2(selectedClient);
            //form2.ShowDialog();

            form2.ShowDialog();
            dataGridView2.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)//выбор
        {
            DataGridViewRow curRow = dataGridView1.CurrentRow;
            DataGridViewRow curRow1 = dataGridView2.CurrentRow;
            int Ticket_Id = (int)curRow.Cells["Ticket_Id"].Value;
            string flightNumber = curRow.Cells["Flight_number"].Value.ToString();
            string otkuda = curRow.Cells["Otkuda"].Value.ToString();
            string kuda = curRow.Cells["Kuda"].Value.ToString();
            TimeSpan? vremyaOtpravlenya = curRow.Cells["Vremya_otpravlenya"].Value as TimeSpan?;
            int clientID = Convert.ToInt32(curRow1.Cells["Client_ID"].Value);

            History history = new History
            {
                Client_ID = clientID,
                Flight_number = flightNumber,
                Otkuda = otkuda,
                Kuda = kuda,
                Vremya_otpravlenya = vremyaOtpravlenya
            };

            db.History.Add(history);

            // Уменьшение значения столбца Kol_svobod_biletov на 1
            Tickets kolli = db.Tickets.FirstOrDefault(x => x.Ticket_Id == Ticket_Id);
            kolli.Kol_svobod_biletov -= 1;

            db.SaveChanges();
            UpdateDataGridView();

           

           // dataGridView3.ClearSelection();
            dataGridView3.DataSource = db.History.ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataGridViewRow curRow = dataGridView3.CurrentRow;
            if (curRow != null)
            {
                string flightNumber = curRow.Cells["Flight_number"].Value.ToString();

                // Удаление строки из dataGridView3 и таблицы History
                History selectedHistory = db.History.FirstOrDefault(h => h.Flight_number == flightNumber);
                if (selectedHistory != null)
                {
                    db.History.Remove(selectedHistory);
                }

                Tickets kolli = db.Tickets.FirstOrDefault(x => x.Flight_number == flightNumber);
                kolli.Kol_svobod_biletov += 1;

                db.SaveChanges();
                UpdateDataGridView();

                // Обновление данных в dataGridView3
                dataGridView3.DataSource = db.History.ToList();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            Hide();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            db.SaveChanges();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           
                db.SaveChanges();
            
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)//Удаление клиента
        {
            int clientId = (int)comboBox3.SelectedValue;
            Client selectedClient = db.Client.FirstOrDefault(c => c.Client_ID == clientId);


            if (selectedClient != null)
            {
                db.Client.Remove(selectedClient);
                dataGridView2.Refresh();
                db.SaveChanges();
            }

            // Вывод измененных данных
            dataGridView2.DataSource = db.Client.ToList<Client>();
            LoadComboBox3Data();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();

            DialogResult result = form3.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Обновите DataGridView или другой элемент управления для отображения добавленных данных
                dataGridView2.DataSource = db.Client.ToList();
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
            }
            dataGridView2.Refresh();
            LoadComboBox3Data();
        }
        public void AddClient(Client client)
        {
            db.Client.Add(client);
            db.SaveChanges();

            // Обновление отображения данных в DataGridView
            dataGridView2.DataSource = db.Client.ToList();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
    }
}
