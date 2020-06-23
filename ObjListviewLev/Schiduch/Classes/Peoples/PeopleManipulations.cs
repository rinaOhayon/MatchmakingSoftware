using System;
using System.Collections.Generic;

using System.Text;

using System.Data.SqlClient;
using Schiduch.Classes.Program;

namespace Schiduch
{
    class PeopleManipulations
    {
        public enum RtpFor { ForSearch = 1, ForTempData = 2 };
        public static bool ReaderToPeople(ref People peopleobj, ref SqlDataReader reader, bool register = false, bool detail = false)
        {
            peopleobj.Background = (string)reader["Background"];
            peopleobj.Age = float.Parse(reader["Age"].ToString());
            peopleobj.City = (string)reader["city"];
            peopleobj.Eda = (string)reader["Eda"];
            peopleobj.DadWork = (string)reader["DadWork"];
            peopleobj.ID = (int)reader["relatedid"];
            peopleobj.Lasname = (string)reader["Lastname"];
            peopleobj.WorkPlace = (string)reader["WorkPlace"];
            peopleobj.Sexs = (int)reader["Sexs"];
            peopleobj.Register.ByUserName = (string)reader["ByUserName"];
            peopleobj.Status = (string)reader["Status"];
            peopleobj.LearnStaus = (string)reader["LearnStatus"];
            peopleobj.FirstName = (string)reader["FirstName"];
            peopleobj.FutureLearn = (bool)reader["FutureLearn"];
            peopleobj.Beard = (string)reader["Beard"];
            peopleobj.CoverHead = (string)reader["CoverHead"];
            peopleobj.FaceColor = (string)reader["FaceColor"];
            peopleobj.GorTorN = (string)reader["GorTorN"];
            peopleobj.Looks = (string)reader["Looks"];
            peopleobj.OpenHead = (int)reader["OpenHead"];
            peopleobj.Show = int.Parse(reader["Show"].ToString());
            peopleobj.StakeM = (string)reader["StakeM"];
            peopleobj.Details.OwnChildrenCount = int.Parse((string)reader["OwnChildrenCount"].ToString());
            peopleobj.Tall = float.Parse(reader["Tall"].ToString());
            peopleobj.TneedE = (string)reader["TneedE"];
            peopleobj.Weight = reader["fat"].ToString();
            peopleobj.Zerem = (string)reader["Zerem"];
            peopleobj.Details.MoneyToShadchan = (string)reader["MoneyToShadchan"];
            peopleobj.Tz = (string)reader["Tz"];
            peopleobj.KindChasidut = (string)reader["KindChasidut"];
            peopleobj.ShiducNum = (string)reader["ShiducNum"];
            peopleobj.HealthStatus = (string)reader["HealthStatus"];
            peopleobj.HealthDetails = (string)reader["HealthDetails"];
            peopleobj.ZeremMom = (string)reader["ZeremMom"];
            peopleobj.BirthDayHebrew = (string)reader["BirthDayHebrew"];

            if (register)
            {
                peopleobj.Register.ID = (int)reader["ID"];
               peopleobj.Register.RegDate = DateTime.Parse(reader["regdate"].ToString());
                
                //peopleobj.Register.RegType=(RegisterInfo.RegTypeE)reader["type"];
                peopleobj.Register.RelatedId = (int)reader["relatedid"];
               
                peopleobj.Register.LastUpdate = DateTime.Parse(reader["lastupdate"].ToString());
            }

            if (detail)
            {
                //need to addd chadcan
                try
                {
                    peopleobj.Chadchan = reader["chadchan"].ToString();
                }
                catch (Exception e) { };
                peopleobj.Details.ChildrenCount = (int)reader["ChildrenCount"];
                peopleobj.Details.DadName = (string)reader["DadName"];
                peopleobj.Details.FriendsInfo = (string)reader["FriendsInfo"];
                peopleobj.Details.HomeRav = (string)reader["HomeRav"];
                peopleobj.Details.MechutanimNames = (string)reader["MechutanimNames"];
                peopleobj.Details.MomLname = (string)reader["MomLname"];
                peopleobj.Details.MomName = (string)reader["MomName"];
                peopleobj.Details.MomWork = (string)reader["MomWork"];
                peopleobj.Details.MoneyGives = float.Parse(reader["MoneyGives"].ToString());
                peopleobj.Details.MoneyRequired = float.Parse(reader["MoneyRequired"].ToString());
                peopleobj.Details.MoneyNotesFlex = (bool)reader["MoneyNotesFlex"];
                // peopleobj.Details.Pic = (string)reader["Pic"];
                peopleobj.Details.RelatedId = (int)reader["relatedid"];
                peopleobj.Details.Schools = (string)reader["Schools"];
                peopleobj.Details.SiblingsSchools = (string)reader["SiblingsSchools"];
                peopleobj.Details.Street = (string)reader["street"];
                peopleobj.Details.Tel1 = (string)reader["tel1"];
                peopleobj.Details.Tel2 = (string)reader["tel2"];
                peopleobj.Details.WhoAmI = (string)reader["whoami"];
                peopleobj.Details.Notes = (string)reader["notes"];
                peopleobj.Details.WhoIWant = (string)reader["WhoIWant"];
                peopleobj.Details.ZevetInfo = (string)reader["ZevetInfo"];
                peopleobj.Details.YesivaKorHighSchool = (string)reader["YesivaKorHighSchool"];
                peopleobj.Details.YeshivaGorSeminary = (string)reader["YeshivaGorSeminary"];
                peopleobj.Details.KibutzorMaslul = (string)reader["KibutzorMaslul"];
                peopleobj.Details.Licence = (bool)reader["Licence"];
                peopleobj.Details.Smoker = (bool)reader["Smoker"];
                peopleobj.Details.EdaExpectation = (string)reader["EdaExpectation"];
                peopleobj.Details.AgeExpectation = float.Parse(reader["AgeExpectation"].ToString());
                peopleobj.Details.DadYeshiva = (string)reader["DadYeshiva"];
                peopleobj.Details.MomSeminary = (string)reader["MomSeminary"];
                peopleobj.Details.StatusParents = (string)reader["StatusParents"];
                peopleobj.Details.CommunityTo = (string)reader["CommunityTo"];
                peopleobj.Details.ParentHealth = (string)reader["ParentHealth"];
                peopleobj.Details.ParentHealthDetails = (string)reader["ParentHealthDetails"];
                peopleobj.Details.LocationChild = (int)reader["LocationChild"];
                peopleobj.Details.NumMarriedSibilings = (int)reader["NumMarriedSibilings"];
                peopleobj.Details.ContactShiduch = (string)reader["ContactShiduch"];
                peopleobj.Details.ContactPhone = (string)reader["ContactPhone"];
                peopleobj.Details.FamilyAbout = (string)reader["FamilyAbout"];
                peopleobj.Details.Telephone = (string)reader["Telephone"];
                peopleobj.Details.PhoneOfBachur = (string)reader["PhoneOfBachur"];
                peopleobj.Details.PhoneKosherLevel = (string)reader["PhoneKosherLevel"];
                peopleobj.Details.Mail = (string)reader["Mail"];

            }

            if (GLOBALVARS.MyUser.Control > User.TypeControl.User && DBFunction.ColumnExists(reader, "Reason"))
            {
                peopleobj.Reason = (int)reader["reason"];
                peopleobj.RealId = int.Parse(reader["mrelatedID"].ToString());
                peopleobj.TempId = (int)reader["TID"];
                peopleobj.ByUser = (int)reader["ByUser"];
            }

            return true;
        }
        public static bool ReaderToPeople(ref People peopleobj, ref SqlDataReader reader, RtpFor WhatFor)
        {

            if (WhatFor == RtpFor.ForSearch)
            {
                peopleobj.Age = float.Parse(reader["Age"].ToString());
                peopleobj.Lasname = (string)reader["lastname"];
                peopleobj.Sexs = (int)reader["sexs"];
                peopleobj.FirstName = (string)reader["firstname"];
                peopleobj.Details.Schools = (string)reader["schools"];
                peopleobj.Tall = float.Parse(reader["tall"].ToString());
                peopleobj.ID = int.Parse(reader["id"].ToString());
                peopleobj.City = (string)reader["City"];
                peopleobj.FaceColor = (string)reader["FaceColor"];
                peopleobj.Looks = (string)reader["Looks"];
                peopleobj.WorkPlace = (string)reader["WorkPlace"];
                peopleobj.Beard = (string)reader["Beard"];
                peopleobj.Zerem = (string)reader["Zerem"];
                peopleobj.ZeremMom = (string)reader["ZeremMom"];
                peopleobj.LearnStaus = (string)reader["LearnStatus"];
                peopleobj.DadWork = (string)reader["DadWork"];
                peopleobj.Background = (string)reader["Background"];
                peopleobj.Status = (string)reader["Status"];
                peopleobj.Weight = (string)reader["fat"];
                peopleobj.Tz = (string)reader["Tz"];
                peopleobj.KindChasidut = (string)reader["KindChasidut"];
                peopleobj.HealthStatus = (string)reader["HealthStatus"];
                peopleobj.Eda = (string)reader["Eda"];
                peopleobj.Details.DadName = (string)reader["DadName"];
                peopleobj.Details.Street = (string)reader["Street"];
                peopleobj.Details.MomName = (string)reader["MomName"];
                peopleobj.Details.MomWork = (string)reader["MomWork"];

                try
                {
                    //peopleobj.Register.LastUpdate = (DateTime)reader["lastupdate"];
                    if (GLOBALVARS.MyUser.Control > User.TypeControl.User && DBFunction.ColumnExists(reader, "Reason"))
                    {
                        peopleobj.Reason = (int)reader["reason"];
                        peopleobj.ByUser = (int)reader["byuser"];
                        peopleobj.RealId = int.Parse(reader["mrelatedID"].ToString());
                        peopleobj.TempId = (int)reader["TID"];

                    }
                }
                catch { };


            }
            return true;
        }
        /// <summary>
        /// set values of temp peoplr for view in listOfTemp, to know he new people in program
        /// </summary>
        /// <param name="reader">data of current people</param>
        /// <param name="p">people</param>
        public static void ConvertTempPeople(ref SqlDataReader reader, ref People p)
        {
            p.FirstName = (string)reader["FirstName"];
            p.Lasname = (string)reader["Lastname"];
            p.Register.RegDate = DateTime.Parse(reader["regdate"].ToString());
            p.Register.ByUserName = (string)reader["ByUserName"];
            p.Sexs = (int)reader["Sexs"];
            p.Status = (string)reader["Status"];
            p.Eda = (string)reader["Eda"];
            p.Background = (string)reader["Background"];
            p.City = (string)reader["city"];
            p.Age = float.Parse(reader["Age"].ToString());
            p.Details.Schools = (string)reader["schools"];
            p.WorkPlace = (string)reader["WorkPlace"];
            p.LearnStaus = (string)reader["LearnStatus"];
            p.DadWork = (string)reader["DadWork"];
            p.Details.MomWork = (string)reader["MomWork"];
            p.ID = (int)reader["id"];
        }
        public static People ReaderToPeopleCopy(ref People peopleobj, ref SqlDataReader reader, RtpFor WhatFor)
        {

            if (WhatFor == RtpFor.ForSearch)
            {
                peopleobj.Age = float.Parse(reader["Age"].ToString());
                peopleobj.Lasname = (string)reader["lastname"];
                peopleobj.Sexs = (int)reader["sexs"];
                peopleobj.FirstName = (string)reader["firstname"];
                peopleobj.Chadchan = (string)reader["Chadchan"];
                peopleobj.Details.Schools = (string)reader["schools"];
                peopleobj.Tall = float.Parse(reader["tall"].ToString());
                peopleobj.Details.Notes = (string)reader["notes"];
                peopleobj.Show = (int)reader["show"];
                peopleobj.ID = int.Parse(reader["id"].ToString());
                try
                {
                    //peopleobj.Register.LastUpdate = (DateTime)reader["lastupdate"];
                    if (GLOBALVARS.MyUser.Control > User.TypeControl.User && DBFunction.ColumnExists(reader, "Reason"))
                    {
                        peopleobj.Reason = (int)reader["reason"];
                        peopleobj.ByUser = (int)reader["byuser"];
                        peopleobj.RealId = int.Parse(reader["mrelatedID"].ToString());
                        peopleobj.TempId = (int)reader["TID"];

                    }
                }
                catch { };


            }
            return peopleobj;
        }

        public static bool ShowHide(People p)
        {
            string Sql = "Userid=" + GLOBALVARS.MyUser.ID + " AND ALLOWID=" + p.ID;
            if (DBFunction.CheckExist(Sql, "LimitedAllow"))
            {
                return true;
            }
            return false;
        }


        public static bool AllowHide(int id, int toid)
        {
            string Sql = "insert into LimitedAllow(UserId,AllowId) VALUES(" + id + "," + toid + ")";
            return DBFunction.Execute(Sql);
        }

    }
}
