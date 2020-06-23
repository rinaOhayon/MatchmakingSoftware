using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Schiduch
{

    public class Labels
    {
        public string Label;
        public string Catg;
        public int ID;
        public Labels(string lbl, string ctg, int idef)
        {
            Label = lbl;
            Catg = ctg;
            ID = idef;
        }
        public Labels() { }
    }
    public static class GLOBALLABELS
    {
        public static Labels[] AllLabels;
        private static void LoadLabelsToAllLabels()
        {
            SqlDataReader reader = DBFunction.ExecuteReader("select * from Labels");
            AllLabels=new Labels[65];
            int index=0;
            while (reader.Read())
            {
                AllLabels[index] = new Labels(reader["label"].ToString(),reader["cat"].ToString(),int.Parse(reader["ID"].ToString()));
                index++;
            }
            reader.Close();
        }
        public static void LoadLabels()
        {
            LoadLabelsToAllLabels();
        }

        public static void LoadGeneralReports(ComboBox cmb)
        {
            
            cmb.Items.Add(new KeyValueClass("פגישות", (int)MakeReport.ReportType.Dates));
            //cmb.Items.Add(new KeyValueClass("שדכנים פעילים", (int)MakeReport.ReportType.ActiveUsers));
        }
    }
}
