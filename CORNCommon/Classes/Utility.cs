using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace  CORNCommon.Classes
{
    public static class Utility
    {
        public static string ShowAlert(bool IsSuccess,string Message)
        {
            string AlertMessage = "";

            if (IsSuccess)
            {
                AlertMessage = "<div style='padding:10px; margin:10px; background-color:#ecfaf0;border: Solid 1px #04551d; color:#04551d;	font-size:12px; font-family:Verdana;'>" + Message + "</div>";
            }
            else
            {
                AlertMessage = "<div style='padding:10px; margin:10px; background-color:#fbefef;	border: Solid 1px #bb0000; color:#bb0000;	font-size:12px; font-family:Verdana;'>" + Message + "</div>";
            }

            return AlertMessage;
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static List<T> ToCollection<T>(this DataTable dt)
        {
            List<T> lst = new System.Collections.Generic.List<T>();
            Type tClass = typeof(T);
            PropertyInfo[] pClass = tClass.GetProperties();
            List<DataColumn> dc = dt.Columns.Cast<DataColumn>().ToList();
            T cn;
            foreach (DataRow item in dt.Rows)
            {
                cn = (T)Activator.CreateInstance(tClass);
                foreach (PropertyInfo pc in pClass)
                {
                    // Can comment try catch block. 
                    try
                    {
                        DataColumn d = dc.Find(c => c.ColumnName.ToUpper() == pc.Name.ToUpper());
                        if (d != null)
                            pc.SetValue(cn, item[pc.Name], null);
                    }
                    catch
                    {
                    }
                }
                lst.Add(cn);
            }
            return lst;
        }
        public static T ToObject<T>(this DataTable dt)
        {
            Type tClass = typeof(T);
            PropertyInfo[] pClass = tClass.GetProperties();
            List<DataColumn> dc = dt.Columns.Cast<DataColumn>().ToList();
            T obj = (T)Activator.CreateInstance(tClass);

            if (dt.Rows.Count > 0)
            {
                DataRow item = dt.Rows[0];
                foreach (PropertyInfo pc in pClass)
                {
                    // Can comment try catch block. 
                    try
                    {
                        DataColumn d = dc.Find(c => c.ColumnName.ToUpper() == pc.Name.ToUpper());
                        if (d != null)
                            pc.SetValue(obj, item[pc.Name], null);
                    }
                    catch
                    {
                    }
                }
            }
            return obj;
        }


    }
}