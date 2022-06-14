using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Reflection;

namespace Winform_Final.Business_Layers
{
    internal class BSProduct
    {
        PizzaManageLinQDatabaseDataContext pizzaManager;
        string err = "nooo";
        public BSProduct()
        {
            pizzaManager = new PizzaManageLinQDatabaseDataContext();
        }
        public Table<PRODUCT> getProducts()
        {
            return pizzaManager.PRODUCTs;
        }
        public DataTable getProduct_baseOnCategory(string category)
        {
            var productLst = from product in pizzaManager.PRODUCTs
                           where product.Category == category
                           select product;
            DataTable dt = LINQResultToDataTable(productLst);

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
        public int countProducts(ref string err)
        {
            int productAmount = (from product in pizzaManager.PRODUCTs
                                select product.ID).Count();
            return productAmount;
        }
        public int countProductTypes(ref string err, string category)
        {
            int userCount = (from product in pizzaManager.PRODUCTs
                             where product.Category == category
                             select product.ID).Count();
            return userCount;
        }
        public bool updateProduct(string foodID, string name, string price, ref string err)
        {
            var tpQuery = (from product in pizzaManager.PRODUCTs
                           where product.ID == Convert.ToInt32(foodID)
                           select product).SingleOrDefault();
            if (tpQuery != null)
            {
                tpQuery.ProductName = name;
                tpQuery.price = Convert.ToInt32(price);
                pizzaManager.SubmitChanges();
            }
            return true;
        }
        public bool addingProduct(string name, string category, string price, ref string err)
        {
            PRODUCT newProduct = new PRODUCT();
            newProduct.ProductName = name;
            newProduct.Category = category;
            newProduct.price = Convert.ToInt32(price);

            pizzaManager.PRODUCTs.InsertOnSubmit(newProduct);
            pizzaManager.PRODUCTs.Context.SubmitChanges();
            return true;
        }
        public bool deletingProduct(ref string err, string productID)
        {
            var tpQuery = from product in pizzaManager.PRODUCTs
                          where product.ID == Convert.ToInt32(productID)
                          select product;
            pizzaManager.PRODUCTs.DeleteAllOnSubmit(tpQuery);
            pizzaManager.SubmitChanges();
            return true;
        }
    }
}
