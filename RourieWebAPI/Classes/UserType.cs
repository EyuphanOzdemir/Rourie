using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RourieWebAPI.Classes
{
    public class UserType
    {
        public UserType(string text, int value)
        {
            Text = text;
            Value = value;
        }

        public int Value { get; }
        public string Text { get; }
    }
}
