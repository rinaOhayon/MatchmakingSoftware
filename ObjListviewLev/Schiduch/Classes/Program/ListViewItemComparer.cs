using System;
using System.Collections.Generic;
using System.Collections;

using System.Text;
using System.Windows.Forms;


namespace Schiduch
{
    class ListViewItemComparer : IComparer
    {
        private int col;

        private int ColumnToSort;

        private SortOrder OrderOfSort;

        private CaseInsensitiveComparer ObjectCompare;
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }
        public ListViewItemComparer()
        {
            col = 0;
            ColumnToSort = 0;
            OrderOfSort = SortOrder.None;
            ObjectCompare = new CaseInsensitiveComparer();
        }
        public ListViewItemComparer(int column)
        {
            col = column;
            OrderOfSort = SortOrder.None;
            ObjectCompare = new CaseInsensitiveComparer();
        }
        //public int Compare(object x, object y)
        //{
        //    int returnVal = -1;
        //    returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text.Trim(),
        //    ((ListViewItem)y).SubItems[col].Text.Trim(), StringComparison.Ordinal);
        //    return returnVal;
        //}


        public int Compare(object x, object y)
        {


            int compareResult;
            ListViewItem listviewX, listviewY;
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;
            //try
            //{

            //    DateTime dateX = Convert.ToDateTime(listviewX.SubItems[ColumnToSort].Text);
            //    DateTime dateY = Convert.ToDateTime(listviewY.SubItems[ColumnToSort].Text);
            //    compareResult = ObjectCompare.Compare(dateX, dateY);
            //}
            //catch
            //{
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
        //    }
            //if(OrderOfSort == SortOrder.Descending)
            //    return (-compareResult);
            //return compareResult;
            if (OrderOfSort == SortOrder.Ascending)
            {
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                return (-compareResult);
            }
            else
            {
                return 0;
            }
        }
    }
}
