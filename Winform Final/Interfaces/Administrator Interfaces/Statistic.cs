using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winform_Final.Business_Layers;

namespace Winform_Final.Interfaces.Administrator_Interfaces
{
    public partial class Statistic : Form
    {
        BSUser clientDatabase;
        BSBill billDatabase = new BSBill();
        string err = "Error";
        int totalIncome = 0;
        public Statistic()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            clientDatabase = new BSUser();
            billDatabase = new BSBill();
            orderGridView.DataSource = billDatabase.getBills_Full();
            userGridView.DataSource = clientDatabase.getUserDetails();
            orderGridView.AutoResizeColumns();
            userGridView.AutoResizeColumns();

            totalIncome = calculateTotalIncome();
            totalIIncomeLB.Text = totalIncome.ToString();
        }

        private void Statistic_Load(object sender, EventArgs e)
        {
            LoadData();
            
        }

        private void deleteUserBtn_Click(object sender, EventArgs e)
        {
            int selectedIndex = userGridView.CurrentCell.RowIndex;
            string clientID = userGridView.Rows[selectedIndex].Cells["userID"].Value.ToString();

            DialogResult traloi;
            // Hiện hộp thoại hỏi đáp 
            traloi = MessageBox.Show("Confirm Deleting User Account !", "Trả lời",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (traloi == DialogResult.Yes)
            {
                
                clientDatabase.deletingUser(clientID);
                billDatabase.deletingBill_basedOnUser(ref err, clientID);
                // Cập nhật lại DataGridView 
                LoadData();
                // Thông báo 
                MessageBox.Show("Product has been deleted!");
            }
            else
            {
                // Thông báo 
                MessageBox.Show("Cancel deleting..!");
            }
        }

        private int calculateTotalIncome()
        {
            int total = 0;
            foreach(DataGridViewRow orderRow in orderGridView.Rows)
            {
                total += Convert.ToInt32(orderRow.Cells["totalPrice"].Value);
            }
            return total;
        }
    }
}
