using Schiduch.Classes.Program;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Schiduch.Classes.Users
{
   public class Sector
    {
        public int IdSector { get; set; }
        public string SectorName { get; set; }
        public static void GetSectors()
        {
            SqlDataReader reader= DBFunction.ExecuteReader("select * from SectorUser order by idsector");
            GLOBALVARS.Sectors = new List<Sector>();
            while (reader.Read())
            {
                GLOBALVARS.Sectors.Add(
                    new Sector()
                    {
                        IdSector = (int)reader["IdSector"],
                        SectorName = (string)reader["SectorName"]
                    });
            };
            reader.Close();
        }
    }
}
