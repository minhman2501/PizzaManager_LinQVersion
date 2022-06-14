using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

using System.Data;
using System.Data.SqlClient;

namespace Winform_Final.Business_Layers
{
    internal class BSUser
    {
        PizzaManageLinQDatabaseDataContext pizzaManager;
        string err = "nooo";
        
        public BSUser()
        {
            pizzaManager = new PizzaManageLinQDatabaseDataContext();
        }
        public Table<ACCOUNT> getUserDetails()
        {
            return pizzaManager.ACCOUNTs;
        }
        public int countUsers_username(ref string err, string username)
        {
            int userCount = (from account in pizzaManager.ACCOUNTs
                             where account.username == username
                             select account.userID).Count();
            return userCount;
        }
        public int countUsers_login(ref string err, string username, string password)
        {
            int userCount = (from account in pizzaManager.ACCOUNTs
                             where account.username == username
                             && account.password == password
                             select account.userID).Count();
            return userCount;
        }
        public bool addingUser(string username, string password, string fullName, string address, ref string err)
        {
            ACCOUNT account = new ACCOUNT();
            account.username = username;
            account.password = password;
            account.FullName = fullName;
            account.Address = address;

            pizzaManager.ACCOUNTs.InsertOnSubmit(account);
            pizzaManager.ACCOUNTs.Context.SubmitChanges();
            return true;
        }
        public ACCOUNT getUser_withUsername(string username)    
        {
            return (ACCOUNT) pizzaManager.ACCOUNTs.Where(account => account.username == username).Single();
        }
        public bool deletingUser(string clientID)
        {
            var tpQuery = from account in pizzaManager.ACCOUNTs
                          where account.userID == Convert.ToInt32(clientID)
                          select account;
            pizzaManager.ACCOUNTs.DeleteAllOnSubmit(tpQuery);
            pizzaManager.SubmitChanges();
            return true;
        }
    }
}
