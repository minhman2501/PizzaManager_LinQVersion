using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

using Winform_Final.Business_Layers;

namespace Winform_Final.Class_Folder.Class_Object
{
    public class Client
    {
        private int id;
        private string username;
        private string fullName;
        private string address;

        BSUser userDatabase;


        public Client(string username)
        {
            this.username = username;
        }
        public int getID()
        {
            return this.id;
        }
        public string getFullName()
        {
            return fullName;
        }
        public string getAddress()
        {
            return address;
        }
        public void getClientDetails()
        {
            ACCOUNT account = new ACCOUNT();
            userDatabase = new BSUser();
            account = userDatabase.getUser_withUsername(this.username);
            this.id = account.userID;
            this.fullName = account.FullName;
            this.address = account.Address;
        }

    }
}
