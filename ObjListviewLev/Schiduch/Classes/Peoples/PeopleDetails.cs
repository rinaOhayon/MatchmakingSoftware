using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;


namespace Schiduch
{
    public class PeopleDetails
    {
        public string Street = "";
        public string Pic; ///need fix
        public string Schools="";
        public int OwnChildrenCount;
        public string Tel1 = "";
        public string Tel2 = "";
        public string WhoAmI = "";
        public string WhoIWant = "";
        public string DadName = "";
        public string MomName = "";
        public int ChildrenCount;
        public string SiblingsSchools = "";
        public string MomLname = "";
        public string MomWork = "";
        public float MoneyGives;
        public float MoneyRequired;
        // 27.01 changed from string to bool במקום הערות כספיות הפכתי את זה לגמיש בכסף
        public bool MoneyNotesFlex;
        public string HomeRav = "";
        public string MechutanimNames = "";
        public int RelatedId;
        public string ZevetInfo = "";
        public string FriendsInfo = "";
        public string MoneyToShadchan = "";
        public string Notes = "";
        //todo27.01 details
        public string YesivaKorHighSchool = "";
        public string YeshivaGorSeminary = "";
        public string KibutzorMaslul = "";
        public bool Licence;
        public bool Smoker;
        public string EdaExpectation = "";
        public float AgeExpectation;
        public string DadYeshiva = "";
        public string MomSeminary = "";
        public string StatusParents = "";
        public string CommunityTo = "";
        public string ParentHealth = "";
        public string ParentHealthDetails = "";
        public int LocationChild;
        public int NumMarriedSibilings;

        public string ContactShiduch = "";
        public string ContactPhone = "";
        public string FamilyAbout = "";
        public string Telephone = "";
        public string PhoneOfBachur = "";
        public string PhoneKosherLevel = "";
        public string Mail = "";
    }
}
