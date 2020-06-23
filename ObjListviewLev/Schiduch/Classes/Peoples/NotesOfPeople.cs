using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Schiduch
{
  public   class NotesOfPeople
    {
        public int NoteId;
        public int UserId;
        public string UserName;
        public int PeopleId;
        public string NoteText;
        public DateTime NoteDate;

        public NotesOfPeople ReaderToNotes(ref SqlDataReader reader)
        {
            NotesOfPeople n = new NotesOfPeople();
            n.NoteId = int.Parse(reader["NoteId"].ToString());
            n.UserId = int.Parse(reader["UserId"].ToString());
            n.UserName = (string)reader["UserName"];
            n.PeopleId = int.Parse(reader["PeopleId"].ToString());
            n.NoteText = (string)reader["NoteText"];
            n.NoteDate = DateTime.Parse(reader["NoteDate"].ToString());

            return n;
        }
    }
}
