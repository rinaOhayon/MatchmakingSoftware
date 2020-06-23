using System;
using System.Collections.Generic;
using System.Text;

namespace Schiduch
{
    [Serializable]
    public class KeyValueClass
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
        public KeyValueClass(string txt,int xvalue)
        {
            Text = txt;
            Value = xvalue;
        }
        public KeyValueClass(string txt, object xvalue)
        {
            Text = txt;
            Value = xvalue;
        }
        public KeyValueClass()
        {
            Text = "";
            Value = 0;
        }
    }
}
