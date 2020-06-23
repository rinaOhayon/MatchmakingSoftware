using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace Schiduch
{
    class FreeSearch
    {
        /*
        private string[] Sexs;
        //private string[] Tall;
        private string[] HeadCover;
        private string[] FaceColor;
        private string[] Eda;
        public static ArrayList FirstNames;
        public static ArrayList Lastnames;
        public enum accuracy {Normal,High,Low}
        public accuracy acc_general;
        //private string[] Status;
        private string[] Weight;
        private string[] Bread;
        private string[] LearnStatus;
        private string[] Background;
        private string[] ClientPropeties;
        private string[] Zerem;
        public FreeSearch()
        {
            SqlDataReader reader= DBFunction.ExecuteReader("select top 1 * from freesearch");
            reader.Read();
            Sexs = reader["sexs"].ToString().Split(',');
            Zerem = reader["Zerem"].ToString().Split(',');
            Eda = reader["Eda"].ToString().Split(',');
            reader.Close();
        }

        public SqlDataReader Search(string txt, accuracy accval)
        {
            new Log(Log.ActionType.FreeSearchLog, txt).Add();
            string sql= "select show,chadchan,Subscription,[peopledetails].[relatedid],lastupdate,[Peoples].[ID],FirstName,sexs,Lastname,tall,schools,age,city,show,notes from peoples inner join peopledetails on ID=relatedid  inner join " +
                    "registerinfo r on peopledetails.relatedid=r.relatedid where ";
            string show = " AND (show <2 or (show=5 and chadchan like '%{" + GLOBALVARS.MyUser.ID + "}%') or (show=4 and chadchan like '%{" + GLOBALVARS.MyUser.ID + "}%'))";
            string temp ="";
            if (GLOBALVARS.MyUser.Control == User.TypeControl.Manger || GLOBALVARS.MyUser.Control == User.TypeControl.Admin)
                show = "";
            acc_general = accval;

            txt = txt.Replace(",", " ");

            temp=SexsFilter(ref txt);
            if (!string.IsNullOrEmpty(temp))
                sql += temp + " AND ";

            temp = AgeFilter(ref txt);
            if (!string.IsNullOrEmpty(temp))
                sql +=  temp + " AND ";

            temp = ZeremFilter(ref txt);
            if (!string.IsNullOrEmpty(temp))
                sql += temp + " AND ";

            temp = EdaFilter(ref txt);
            if (!string.IsNullOrEmpty(temp))
                sql += temp + " AND ";

            temp = StatusFilter(ref txt);
            if (!string.IsNullOrEmpty(temp))
                sql += temp + " AND ";

            

            temp = EasyFaceColor(ref txt);
            if (!string.IsNullOrEmpty(temp))
                sql += temp + " AND ";

            temp = EasyFat(ref txt);
            if (!string.IsNullOrEmpty(temp))
                sql += temp + " OR "; // or for the lastone before names


            temp = CheckNames(ref txt);
           
               

            RemoveLast_ORANDWHERE(ref sql);
            sql += show;
            sql += " ORDER BY ID DESC";
          //  System.Windows.Forms.MessageBox.Show(sql);
            return  DBFunction.ExecuteReader(sql);
        }

        private string SexsFilter(ref string txt) {
            string sql = "(";
            foreach (string temp in Sexs)
            {
                if (Regex.IsMatch(txt, "\\b" + temp + "\\b"))
                {
                    switch (temp)
                    {
                        case "גבר":
                        case "הגבר":
                        case "וגבר":
                        case "בחור":
                        case "הבחור":
                        case "רווק":
                        case "ובחור":
                        case "גרוש":
                        case "בחורים":
                        case "הבחורים":
                        case "ובחורים":
                        case "בן":
                        case "ובן":
                            sql += " sexs=N'זכר' or";
                            break;
                        case "אשה":
                        case "אישה":
                        case "נשים":
                        case "הנשים":
                        case "בחורה":
                        case "הבחורה":
                        case "בחורות":
                        case "ובחורות":
                        case "הבחורות":
                        case "נקבה":
                        case "גרושה":
                        case "רווקה":
                        case "הנקבות":
                        case "בת":
                        case "ובת":
                            sql += " sexs=N'נקבה' or";
                            break;
                    }
                }
            }
            
            RemoveLast_ORANDWHERE(ref sql);
            sql += ")";
            if(sql !="()")
                return sql;
            return "";
        }
        private string StatusFilter(ref string txt)
        {
            string sql = "(";
            if (txt.Contains("רווק"))
                sql += " status=N'רווק/ה' or";
            if (txt.Contains("גרוש"))
                sql += " status=N'גרוש/ה' or";
            RemoveLast_ORANDWHERE(ref sql);
            sql += ")";
            if (sql != "()")
                return sql;
            return "";
        }
        private string ZeremFilter(ref string txt)
        {
            string sql = "(";
            string or_and = "or";
            if (acc_general==accuracy.High)
                or_and = "and";
            foreach (string temp in Zerem)
            {
                if (txt.Contains(temp))
                {
                    sql += " zerem like N'%" + temp + "%' " + or_and;
                }
            }
                    RemoveLast_ORANDWHERE(ref sql);
            sql += ")";
            if (sql != "()")
                return sql;
            return "";
        }
        private string EdaFilter(ref string txt)
        {
            string sql = "(";
            foreach (string temp in Eda)
            {
                if (txt.Contains(temp))
                {
                    sql += " eda like N'%" + temp + "%' or";
                }
            }
            if(Regex.Matches(txt, "חצי").Count == 2) { 
                //sql += " eda=N'חצי חצי' or";
                if (txt.Contains("תימני"))
                    sql += " eda=N'חצי תימני' or";
            }
            if(txt.Contains("שכנזי") || txt.Contains("אשכנז"))
                sql += " eda=N'ליטאי' or eda=N'חסידי' or";
            RemoveLast_ORANDWHERE(ref sql);
            sql += ")";
            if (sql != "()")
                return sql;
            return "";
        }
        private string CheckNames(ref string txt)
        {
            string sql = "(";
            foreach (string temp in FirstNames)
            {
                if (string.IsNullOrEmpty(temp))
                    continue;
                if (txt.Contains(temp))
                {
                    sql += " firstname like N'%" + temp + "%' or";
                }
            }
           

            foreach (string temp in Lastnames)
            {
                if (string.IsNullOrEmpty(temp))
                    continue;
                if (txt.Contains(temp))
                {
                    sql += " lastname like N'%" + temp + "%' or";
                }
            }
            RemoveLast_ORANDWHERE(ref sql);

            sql += ")";


            if (sql != "()")
                return sql;
            return "";
        }      
        private string EasyFaceColor(ref string txt)
        {
            string sql = "(";
            if (txt.Contains("כהה"))
                sql += " facecolor=N'כהה' or";
            if (txt.Contains("בהיר"))
                sql += " facecolor like N'%בהיר%' or";
            if (txt.Contains("שזוף") || txt.Contains("שזופה"))
                sql += " facecolor like N'%שזו%' or";
            RemoveLast_ORANDWHERE(ref sql);
            sql += ")";
            if (sql != "()")
                return sql;
            return "";
        }

        private string EasyFat(ref string txt)
        {
            string sql = "(";
            if (txt.Contains("רזה"))
                sql += " fat=N'%רזה%' or";
            if (txt.Contains("קצת מלא"))
                sql += " fat like N'%קצת מלא%' or";
            if (txt.Contains("מלא") && !txt.Contains("קצת מלא"))
                sql += " fat like N'מלא%' or";
            

            RemoveLast_ORANDWHERE(ref sql);
            sql += ")";
            if (sql != "()")
                return sql;
            return "";
        }
        public static string AgeFilter(ref string txt)
        {
            int n;
            string num1="";
            bool continenum = true;
            foreach(char temp in txt.ToCharArray())
            {
                
                bool isNumeric = int.TryParse(temp.ToString(), out n);
                if (isNumeric)
                {
                   
                    if (continenum)
                        num1 += temp;
                    else
                        num1 += "," + temp;
                    continenum = true;
                    continue;
                }
                if(temp !='.')
                    continenum = false;
                
            }
            if (string.IsNullOrEmpty(num1))
                return "";
            num1=num1.Remove(0, 1); // remove the first ,
            string[] numbers = num1.Split(',');
            if (numbers.Length == 1)
            {
                float number1 = float.Parse(numbers[0]);
                if(number1 > 18 && number1 < 90) { // we in age filter 
                if (txt.Contains("עד"))
                    return "AGE < " + number1.ToString();
                return "AGE > " + number1.ToString();
                }
                else // we in the height filter
                {
                    if (txt.Contains("עד"))
                        return "TALL < " + number1.ToString();
                    return "TALL > " + number1.ToString();
                }
            }

            if (numbers.Length == 2)
            {
                float number1 = float.Parse(numbers[0]);
                float number2 = float.Parse(numbers[1]);
                if(number1 > number2)
                {
                    number1 = number2;
                    number2= float.Parse(numbers[0]);
                }
                if (number1 > 18 && number2 < 90)
                {
                    return "AGE > " + number1.ToString() + " AND AGE < " + number2.ToString();
                }
            }
            return "";
        }
        public static void RemoveLast_ORANDWHERE(ref string str)
        {
            
            if (str.ToLower().EndsWith("or"))
                str = str.Remove(str.Length - 2);
            if (str.ToLower().EndsWith("where "))
                str = str.Remove(str.Length - 6);
            if (str.ToLower().EndsWith("and"))
                str = str.Remove(str.Length - 3);
            if (str.ToLower().EndsWith("and "))
                str = str.Remove(str.Length - 4);
           

        }*/
    }
}
