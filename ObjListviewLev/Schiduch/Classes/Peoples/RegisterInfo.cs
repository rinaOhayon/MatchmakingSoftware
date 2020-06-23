using System;
using System.Collections.Generic;

using System.Text;


namespace Schiduch
{
    public class RegisterInfo
    {
        public enum PayTypeE { CreditCard = 1, BankTransfer = 2, Check = 3, Cash = 4 }
        public enum RegTypeE { OneMonth = 1, ThreeMonth = 2, Year = 3, Always = 4 }
        public DateTime RegDate=DateTime.Now;
        public DateTime LastUpdate;
        public int RelatedId;
        public int ID;
        public string ByUserName;
        public int ByUser;
        public RegisterInfo()
        {
            RegDate = DateTime.Now;
            
        }
      

    }
}
