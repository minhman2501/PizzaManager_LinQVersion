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
using Winform_Final.Class_Folder.Class_Object;

namespace Winform_Final.Interfaces.Client_Interface
{
    public partial class Client_HistoryOrdering : Form
    {
        Client user;
        BSBill billDatabase = new BSBill();
        public Client_HistoryOrdering(string username)
        {
            InitializeComponent();
            user = new Client(username);

            user.getClientDetails();
        }

        private void Client_HistoryOrdering_Load(object sender, EventArgs e)
        {
            loadData();  
        }
        public void loadData()
        {
            orderGridView.DataSource = billDatabase.getBills_baseOnUser(user.getID());
            orderGridView.AutoResizeColumns();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            loadData();
        }
    }
}
