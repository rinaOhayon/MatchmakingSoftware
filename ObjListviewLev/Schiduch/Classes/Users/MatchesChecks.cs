using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using Schiduch.Classes.Program;

namespace Schiduch
{
    class MatchesChecks
    {
        static List<DictinorayRow> mydic_whoami = new List<DictinorayRow>();
        static List<DictinorayRow> mydic_whoiwant = new List<DictinorayRow>();
        static List<DictinorayRow> mydic_not_type = new List<DictinorayRow>();

        public static string CreateInfoString(Match m)
        {
            string ret = "";
            if (m == null) return ret;
            ret += "הם מתאימים בגלל " + "\r\nאין בעיה של שמות בשידוך הזה, כמו כן טווח הגיל מתאים\r\n";
            if (!string.IsNullOrEmpty(m.FaceColorCheckString))
                ret += m.FaceColorCheckString + "\r\n";
            if (!string.IsNullOrEmpty(m.FatCheckString))
                ret += m.FatCheckString + "\r\n";
            if (!string.IsNullOrEmpty(m.LooksCheckString))
                ret += m.LooksCheckString + "\r\n";
            if (!string.IsNullOrEmpty(m.ZeremOfEdaString))
                ret += m.ZeremOfEdaString + "\r\n";

            string in_me = "";
            string out_me = "";
            foreach(KeyValuePair<bool,string> item in m.WhoamiCheckString) {
                if (item.Key && !string.IsNullOrEmpty(item.Value))
                    in_me += item.Value + ",";
                else if(!string.IsNullOrEmpty(item.Value))
                    out_me += item.Value + ",";
               
            }
            if (!string.IsNullOrEmpty(in_me))
                ret += "התכונות הבאות נמצאו תואמות מכרטיס הלקוח" + "\r\n" + in_me + "\r\n";
            if (!string.IsNullOrEmpty(out_me))
                ret += "התכונות הבאות נמצאו לא תואמות מכרטיס הלקוח" + "\r\n" + out_me + "\r\n";

            in_me = out_me = "";
            foreach (KeyValuePair<bool, string> item in m.LookForCheckString)
            {
                if (item.Key && !string.IsNullOrEmpty(item.Value))
                    in_me += item.Value +",";
                else if (!string.IsNullOrEmpty(item.Value))
                    out_me += item.Value + ",";
               
            }
            if(!string.IsNullOrEmpty(in_me))
            ret += "התכונות הבאות נמצאו תואמות מכרטיס ההתאמה" + "\r\n" + in_me + "\r\n";
            if(!string.IsNullOrEmpty(out_me))
            ret += "התכונות הבאות נמצאו לא תואמות מכרטיס ההתאמה" + "\r\n" + out_me + "\r\n";

            in_me = out_me = "";
            foreach (KeyValuePair<bool, string> item in m.DicCheckString)
            {
                if (item.Key && !string.IsNullOrEmpty(item.Value))
                    in_me = item.Value + ",";
                else if (!string.IsNullOrEmpty(item.Value))
                    out_me = item.Value + ",";
                
            }
            if (!string.IsNullOrEmpty(in_me))
                ret += "נתונים נוספים שנמצאו תואמים בין שני הכרטיסים" + "\r\n" + in_me + "\r\n";
            if (!string.IsNullOrEmpty(out_me))
                ret += "נתונים נוספים שנמצאו לא תואמים בין שני הכרטיסים" + "\r\n" + out_me + "\r\n";

            return ret;
        }
        People p;
        public enum GeneralMatch { Same,Like,Diff,VeryDiff,None}

        public static int AddLevel(GeneralMatch level)
        {
            if (level == GeneralMatch.Same) return 3;
            else if (level == GeneralMatch.VeryDiff) return -1;
            else if (level == GeneralMatch.Like) return 2;
            else if (level == GeneralMatch.Diff) return 1;
            return 0;
        }
        public static ArrayList GetMatches(People people)
        {
            ArrayList list = new ArrayList();
            mydic_whoami.Clear();
            mydic_whoiwant.Clear();
            
            foreach (DictinorayRow row in DictinorayRow.DictinorayList)
            {
                int temp = row.IsMatchWord(people.Details.WhoAmI);
                if (temp==1 || temp==-1) {
                    if (temp == -1)
                        row.ChangeToUpSitewords();               
                    mydic_whoami.Add(row);  
                    mydic_not_type.Add(row);
                }
            }
            foreach (DictinorayRow row in DictinorayRow.DictinorayList)
            {
                int temp = row.IsMatchWord(people.Details.WhoAmI);
                if (temp == 1 || temp == -1)
                {
                    if (temp == -1)
                        row.ChangeToUpSitewords();
                    mydic_whoiwant.Add(row);
                    mydic_not_type.Add(row);
                }
            }

            MatchesChecks xmatch = new MatchesChecks();

            xmatch.p = people;
            string show = " AND SHOW <> 8 AND (show <2 or (show=5 and chadchan like '%{" + GLOBALVARS.MyUser.ID + "}%') or (show=4 and chadchan like '%{" + GLOBALVARS.MyUser.ID + "}%'))";
            if (GLOBALVARS.MyUser.Control == User.TypeControl.Manger || GLOBALVARS.MyUser.Control == User.TypeControl.Admin)
                show = " and show <> 8";

            string sql = "select zerem,looks,show,chadchan,whoami,whoiwant,[peopledetails].[relatedid],[Peoples].[ID],FirstName,facecolor,sexs,Lastname,tall,status,fat,age,show,notes from peoples inner join peopledetails on ID=relatedid  ";
            sql += xmatch.GetSqlCheck() + show;
            SqlDataReader reader= DBFunction.ExecuteReader(sql);
            while (reader.Read())
            {
                Match result = new Match();
                
                result.Name = reader["firstname"].ToString() + " " + reader["lastname"].ToString();
                result.ID = (int)reader["id"];

                //FAT CHECK
                result.FatCheckString = xmatch.FatCheck(reader["fat"].ToString(),ref result.FatMatchLevel);


                //FACECOLOR CHECK
                result.FaceColorCheckString = xmatch.FaceColorCheck(reader["facecolor"].ToString(),ref result.FaceColorMatchLevel);


                //Looks CHECK
                result.LooksCheckString = xmatch.LooksCheck(reader["looks"].ToString(),ref result.LooksMatchLevel);


                //FromDic CHECK
                result.DicCheckString.AddRange(xmatch.CheckFromDic(reader["looks"].ToString(),"LookChange", ref result.DicMatchLevel));
              //  result.DicCheckString.AddRange(xmatch.CheckFromDic(reader["zerem"].ToString(), "ZeremChange", ref result.DicMatchLevel));
                result.DicCheckString.AddRange(xmatch.CheckFromDic(reader["facecolor"].ToString(), "FaceChange", ref result.DicMatchLevel));
                


                // STATUS CHECK
                result.StatusCheckString = xmatch.StatusCheck(reader["status"].ToString(), ref result.MatchLevel);
              

                //EDA CHECK   NEEEED TO FIXXX
                result.ZeremOfEdaString = xmatch.ZeremCheck(reader["zerem"].ToString(),ref result.ZeremOfEdaMatchLevel);



                //WHOAMI + LOOKFOR CHECK

                result.WhoamiCheckString.AddRange(xmatch.FindPropties(reader["whoiwant"] as string,ref result.WhoAmIMatchLevel,false));

                result.LookForCheckString.AddRange(xmatch.FindPropties(reader["whoami"] as string, ref result.LookForMatchLevel, true));


                

                result.MatchLevel += result.FaceColorMatchLevel + result.FatMatchLevel + result.LooksMatchLevel +
                    result.ZeremOfEdaMatchLevel + result.LookForMatchLevel + result.WhoAmIMatchLevel + 
                    result.DicMatchLevel;

                list.Add(result);
            }
            reader.Close();
            
            return list;
        }
        
        public string GetSqlCheck()
        {
            string sql=null;
            string temp = null;


            temp = SexsCheck();
            if (!string.IsNullOrEmpty(temp))
                sql += temp;

            temp = GorTorNCheck();
            if (!string.IsNullOrEmpty(temp))
                sql += " AND " + temp;
            temp = EdaCheck();
            if (!string.IsNullOrEmpty(temp))
                sql += " AND " + temp;
            temp = NamesChecks();
            if (!string.IsNullOrEmpty(temp))
                sql += " AND " + temp;
                       
            temp = StatusCheck();
            if (!string.IsNullOrEmpty(temp))
                sql += " AND " + temp;
            temp = TallCheck();
            if (!string.IsNullOrEmpty(temp))
                sql += " AND " + temp;
            temp = AgeCheck();
            if (!string.IsNullOrEmpty(temp))
                sql += " AND " + temp;
            temp = BackgroundCheck();
            if (!string.IsNullOrEmpty(temp))
                sql += " AND " + temp;
            return sql;
        }
        /// <summary>
        /// Check that names correct. for example check the name of girl and mom not the same
        /// </summary>
        /// <returns></returns>
        public string NamesChecks()
        {
            string[] names = p.FirstName.Split(' ');
            string sql = null;
            for (int x=0;x < names.Length; x++) {
                if (string.IsNullOrEmpty(names[x].Trim())) continue;
                sql += " (DADNAME NOT LIKE N'%" + names[x] + "%') AND (MOMNAME NOT LIKE N'%" + names[x] + "%') ";
                if (x != names.Length-1 && !string.IsNullOrEmpty(names[x].Trim())) sql += " AND ";
            }
            sql=BuildSql.CheckForLastAnd(ref sql);
            return sql;
        }

        public string SexsCheck()
        {
            
            string sql = " where (sexs=";
            if (p.Sexs == 1)
                sql += "2) ";
            else
                sql += "1) ";
            
            return sql;
        }
        public string TallCheck()
        {

            string sql = " (Tall";
            if (p.Sexs ==1)
                sql += " <= " +  p.Tall.ToString() + ") ";
            else
                sql += " >= " +p.Tall.ToString() + ")";

            return sql;
        }

        public string BackgroundCheck()
        {

            string sql = "";
            if (!string.IsNullOrEmpty(p.Background))
                sql += " (Background=N'" + p.Background + "') ";
            return sql;
        }
        public string AgeCheck()
        {

            string sql = " (Age";
            int minage = 3;
            int maxage = 1;
            if(p.Age > 26)
            {
                minage = 4;maxage = 2;
            }
            if (p.Sexs == 2)
                sql += " BETWEEN "  + (p.Age - minage).ToString() + " AND " + (p.Age +maxage).ToString() + ") ";
            else
                sql += " BETWEEN " + (p.Age -1).ToString() + " AND " + (p.Age + 4).ToString() + ") ";

            return sql;
        }
        public string GorTorNCheck()
        {
            string sql = null;
            if (!string.IsNullOrEmpty(p.GorTorN.Trim()))
            {
                if (p.GorTorN == "עץ")
                    sql = " ( GORTORN <> N'ג') ";
                else
                    sql = " ( GORTORN <> N'עץ') ";
            }
            return sql;
        }
        public string StatusCheck()
        {
            string sql = " (status";
            if (p.Status == "רווק/ה")
                sql += "=N'רווק/ה') ";
            else
                sql += "<> N'רווק/ה')";

            return sql;
        }

        public string EdaCheck()
        {
            string sql = "";
            if (p.Status == "רווק/ה") {
                sql = " (eda";
                string eda = p.Eda;
                sql += "=N'" + eda + "') ";
                switch (eda)
                {
                    case "חוצניק":
                        sql = "";
                        break;
                    case "חצי תימני":
                        sql += " or (eda=N'תימני') ";
                        break;
                    case "תימני":
                        sql += " or (eda=N'חצי תימני') ";
                        break;
                    case "חצי חצי":
                    case "רבע ספרדי":
                        sql = " (eda=N'חצי חצי') or (eda=N'רבע ספרדי') ";
                        break;
                }
            }
            return sql;
        }

        public string FatCheck(string fat,ref int count)
        {
            string ret = "";
            
            if (p.Weight == fat) { 
                ret= "הם דומים מבחינת מבנה גוף, שניהם הוגדרו כ - " + fat;
                count += 2;
            }
            else if ((p.Weight=="רזה" && fat=="מלא") || (p.Weight == "מלא" && fat == "רזה")) { 
                ret= "הם לא דומים מבחינת מראה גופני היות וההתאמה הוגדר כ - " + fat + " והלקוח כ - " + p.Weight ;
                count -= 1;
            }
            else if(!string.IsNullOrEmpty(p.Weight) && !string.IsNullOrEmpty(fat))
            {
                ret = "קצת דומים מבחינת מראה גופני היות וההתאמה הוגדר כ - " + fat + " והלקוח כ - " + p.Weight;
                
            }
            return ret;
        }
        public string FaceColorCheck(string face, ref int count)
        {
            string ret = "";
            
            if (p.FaceColor == face)
            {
                ret = "הם דומים מבחינת צבע פנים, שניהם הוגדרו כ - " + face;
                count += 2;
            }
            else if ((p.FaceColor == "בהיר" && face == "כהה") || (p.FaceColor == "כהה" && face == "בהיר")) {
                ret = "הם לא דומים צבעי פנים היות וההתאמה הוגדר כ - " + face + " והלקוח כ - " + p.FaceColor;
                count -= 1;
            }
            else if (!string.IsNullOrEmpty(p.FaceColor) && !string.IsNullOrEmpty(face))
            {
                ret = "קצת דומים מבחינת צבעי פנים היות וההתאמה הוגדר כ - " + face + " והלקוח כ - " + p.FaceColor;
                
            }
            return ret;
        }
        public string LooksCheck(string look,ref int count)
        {
            string ret = "";
            
            if (p.Looks == look) {
                ret = "הם דומים מבחינת מראה חיצוני, שניהם הוגדרו כ - " + look;
                count += 2;
            }
            if ((p.Looks == "נאה מאד" && look == "סטנדרט") || (p.Looks == "סטנדרט" && look == "נאה מאד")) {
                ret = "הם לא דומים מבחינת מראה חיצוני היות וההתאמה הוגדר כ - " + look + " והלקוח כ - " + p.Looks;
                count -= 1;
            }
            return ret;
        }
       
        public List<KeyValuePair<bool, string>> FindPropties(string str,ref int counter, bool whoiwant = true)
        {
            List<KeyValuePair<bool, string>> ret = new List<KeyValuePair<bool, string>>();
            int tempcounter = 0;
            
            List<DictinorayRow> lst=mydic_whoiwant;
            if (!whoiwant)
                lst = mydic_whoami;
            foreach(DictinorayRow row in lst)
            {
                ret.Add(row.IsMatchWordWithInfo(str,ref tempcounter));
                counter += tempcounter;      
            }
            return ret;
        } 

        public string ZeremCheck(string zerem,ref int count)
        {
            string retstring = "";
            foreach(DictinorayRow row in mydic_not_type)
            {
                if (row.ZeremChange.Count > 0)
                {
                    if (row.Type == 1)
                    {
                        foreach(string w in row.ZeremChange)
                        if (zerem.Contains(w))
                            retstring = row.Desc;count -= 1;
                    }
                    else
                    {
                        foreach (string w in row.ZeremChange)
                            if (zerem.Contains(w))
                            retstring = row.Desc;count+=2;
                    }
                }
            }
            return retstring;
           
        }

        public List<KeyValuePair<bool, string>> CheckFromDic(string data,string propName, ref int count)
        {
            List<KeyValuePair<bool, string>> ret = new List<KeyValuePair<bool, string>>();
            string val;

            foreach (DictinorayRow row in mydic_not_type)
            {

                val = row.GetType().GetProperty(propName).GetValue(row, null) as string ;
                if (!string.IsNullOrEmpty(val))
                {
                    if (row.Type == 1)
                    {
                        if (data.Contains(val)) { 
                            ret.Add(new KeyValuePair<bool, string>(false, row.Desc));
                            count--;
                        }
                    }
                    else
                    {
                        if (data.Contains(val))
                            ret.Add(new KeyValuePair<bool, string>(true, row.Desc)); count++;
                    }
                }
            }
            return ret;
        }
        public string StatusCheck(string status,ref int count)
        {
            string ret = "";
            if (p.Status == "רווק/ה") { 

                return ret;
            }
            else if (p.Status == status) {
                ret = "קיימת התאמה בסטטוס, שניהם - " + status;
                count += 2;
            }
            else { 
                ret = "קצת שונים בסטטוס שלהם" + status;
                count -= 1;
            }
            return ret;
        }
        private string ReplaceAllBadStrings(string str)
        {
            return str.Replace(',', ' ').Replace('.', ' ').Replace("\r\n", " ");
                
        }
    }
    class Match:IComparable
    {
        public string Name = "";
        public float Age =0;
        public float Tall =0;
        public int ID;
        public string FatCheckString="";
        public int FatMatchLevel = 0;

        public string FaceColorCheckString = "";
        public int FaceColorMatchLevel =0;

        public string LooksCheckString = "";
        public int LooksMatchLevel = 0;

        public List<KeyValuePair<bool,string>> DicCheckString = new List<KeyValuePair<bool, string>>();
        public int DicMatchLevel = 0;

        public List<KeyValuePair<bool, string>> WhoamiCheckString = new List<KeyValuePair<bool, string>>();
        public int WhoAmIMatchLevel = 0;

        public List<KeyValuePair<bool, string>> LookForCheckString = new List<KeyValuePair<bool, string>>();
        public int LookForMatchLevel = 0;


        public string StatusCheckString = "";

        public string ZeremOfEdaString = "";
        public int ZeremOfEdaMatchLevel = 0;

        public string AnotherInformation = "";
        public int MatchLevel = 0;

        
       
        public int CompareTo(object obj)
        {
            if (this.MatchLevel == ((Match)obj).MatchLevel) return 0;
            else if(this.MatchLevel > ((Match)obj).MatchLevel) return -1;
            return 1;
        }
    }

    class DictinorayRow
    {
        public static List<DictinorayRow> DictinorayList = new List<DictinorayRow>();
        public List<string> Word;
        public bool MatchInWord = true;
        public List<string> UpsiteWord ;
        public string SubWord = "";
        public int Type = 0;
        public int Id = 0;
        public string EdaChange { get; set; }
        public string LookChange { get; set; }
        public string FaceChange { get; set; }
        public string StatusChange { get; set; }
        public string Desc { get; set; }
        public List<string> ZeremChange;

        public void ChangeToUpSitewords() { MatchInWord = false; }
        public DictinorayRow()
        {
            Word = new List<string>();
            UpsiteWord = new List<string>();
            ZeremChange = new List<string>();
        }
        public static void LoadDictonary()
        {


            SqlDataReader reader = DBFunction.ExecuteReader("select * from Dictionary");
            
            while (reader.Read())
            {
                DictinorayRow row = new DictinorayRow();
                row.LoadFromReader(reader);
                DictinorayList.Add(row);
            }
            reader.Close();
            
        }
        public void LoadFromReader(SqlDataReader reader)
        {
            string[] ret = SplWords(reader["Word"] as string);
            string[] temptxt = (reader["Word"] as string).Split('#');

            foreach(string str in temptxt[0].Split('^'))
            {
                Word.Add(str);
            }

            if(temptxt.Length == 2)
            {
                foreach (string str in temptxt[1].Split('^'))
                {
                    UpsiteWord.Add(str);
                }
            }


            if (reader["EdaChange"] != null)
                EdaChange = reader["EdaChange"] as string;


            if (reader["ZeremChange"] != null && !string.IsNullOrEmpty(reader["ZeremChange"] as string)) { 
                foreach (string str in (reader["ZeremChange"] as string).Split('^'))        
                    ZeremChange.Add(str);
            }

            if (!string.IsNullOrEmpty(reader["Desc"] as string))
                Desc = reader["Desc"] as string;

            Id = (int)reader["Id"];

            if (reader["type"] != null)
            {
                try
                {
                    Type = (int)reader["type"];
                }
                catch { }
            }


            if (reader["FaceChange"] != null)
                FaceChange = reader["FaceChange"] as string;

            if (reader["Lookschange"] != null)
                LookChange = reader["Lookschange"] as string;

            if (reader["Statuschange"] != null)
                StatusChange = reader["Statuschange"] as string;

            if (reader["Subword"] != null)
            {
                ret = SplWords(reader["Subword"] as string);
                if (ret != null)
                    SubWord = ret[0];
            }

        }
        private string[] SplWords(string txt)
        {
            if (string.IsNullOrEmpty(txt)) return null;
            string[] temptxt = txt.Split('#');
            string[] ret = new string[2];
            ret[0] = "";
            ret[1] = "";

            for (int x = 0; x < temptxt.Length; x++)
            {
                foreach (string word in temptxt[x].Split('^'))
                {
                    ret[x] += word + ",";
                }
                ret[x] = ret[x].Remove(ret[x].Length - 1);
            }
            if (string.IsNullOrEmpty(ret[0]))
                ret = null;
            return ret;
        }

        public int IsMatchWord(string str)
        {
            foreach(string word in UpsiteWord)
                if (str.Contains(word)) return -1;

            foreach (string word in Word)
                if (str.Contains(word)) return 1;

            return 0;

        }
        public KeyValuePair<bool, string> IsMatchWordWithInfo(string str,ref int counter)
        {
            bool found = false;
            string description = "";
            KeyValuePair<bool, string> ret=new KeyValuePair<bool, string>();
            foreach (string word in UpsiteWord) {
                if (str.Contains(word))
                {
                    if (!string.IsNullOrEmpty(Desc))
                        description = Desc ;
                    else
                        description= word;

                    if (MatchInWord)
                    {
                        counter--;
                        ret = new KeyValuePair<bool, string>(false, description);
                    }
                    else
                    {
                        counter++;
                        ret = new KeyValuePair<bool, string>(true, description);
                    }
                    found = true;
                    
                    if (found) break;
                }
            }
            if (found) return ret;

            foreach (string word in Word) { 
                if (str.Contains(word))
                {
                    if (!string.IsNullOrEmpty(Desc))
                        description = Desc;
                    else
                        description = word;

                    if (!MatchInWord)
                    {
                        counter--;
                        ret = new KeyValuePair<bool, string>(false, description);
                    }
                    else
                    {
                        counter++;
                        ret = new KeyValuePair<bool, string>(true, description);
                    }

                    found = true;
                    
                    if (found) break;
                }
            }
            if (found) return ret;
            return ret;
        }
    }
    }
