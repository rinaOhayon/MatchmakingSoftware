using Schiduch.Classes.Users;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Schiduch.Classes.Program
{
    public static class GLOBALVARS
    {
        public static bool AllLoad = false;
        public static User MyUser;
        public static float version;
        public static DateTime LastSwUpdateFile;
        public static DateTime LastPeopleCheckFile;
        public static DateTime LastAlertsCheckFile;
        public static DateTime LastSwUpdateDB;
        public static int StatusSw;
        public static DateTime LastPeopleCheckDB;
        public static DateTime LastUserChangeFile;
        public static DateTime LastUserChangeDB;
        public static DateTime LastChatChangeFile;
        public static DateTime LastChatChangeDB;
        public static DateTime LastAlertsCheckDB;
        public static List<Sector> Sectors;
        public static List<KeyValueClass> SectorsKeyValue;
        public static List<User> Users;
        public static ArrayList Shadchanim;
        public static ArrayList Clients;
        public static bool IsDeveloper = false;
    }
}
