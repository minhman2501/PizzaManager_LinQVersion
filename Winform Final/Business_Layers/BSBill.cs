﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Reflection;

namespace Winform_Final.Business_Layers
{
    internal class BSBill
    {
        PizzaManageLinQDatabaseDataContext pizzaManager;
        string err = "nooo";
        public BSBill()
        {
            pizzaManager = new PizzaManageLinQDatabaseDataContext();
        }
        public Table<BILLDETAIL> getBills_Full()
        {
            return pizzaManager.BILLDETAILs;
        }
        public DataTable getBills_baseOnUser(int userId)
        {
            var billList = from bill in pizzaManager.BILLDETAILs
                           where bill.clientID == Convert.ToInt32(userId)
                           select bill;
            DataTable dt = LINQResultToDataTable(billList);

            return dt;
        }
        public static DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();
            PropertyInfo[] columns = null;
            if (Linqlist == null) return dt;
            foreach (T Record in Linqlist)
            {
                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                               == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                           (Record, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public bool addingBill(string userID, string totalPrice, string content, string address, string date, ref string err)
        {
            BILLDETAIL newBill = new BILLDETAIL();
            newBill.clientID = Convert.ToInt32(userID);
            newBill.totalPrice = Convert.ToInt32(totalPrice);
            newBill.content = content;
            newBill.deliveryAddress = address;
            newBill.deliveryDate = Convert.ToDateTime(date);

            pizzaManager.BILLDETAILs.InsertOnSubmit(newBill);
            pizzaManager.BILLDETAILs.Context.SubmitChanges();
            return true;
        }
        public bool deletingBill_basedOnUser(ref string err, string clientID)
        {
            var tpQuery = from bill in pizzaManager.BILLDETAILs
                          where bill.clientID == Convert.ToInt32(clientID)
                          select bill;
            pizzaManager.BILLDETAILs.DeleteAllOnSubmit(tpQuery);
            pizzaManager.SubmitChanges();
            return true;
        }
    }
}
