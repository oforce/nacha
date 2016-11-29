using System.Collections.Generic;

namespace Nacha.Helpers
{
    public static class FieldValidator
    {
        public static void Validate(string Name, string Value, ref List<string> Message)
        {
            if (Value == null || Value.Trim() == "")
            {
                Message.Add(Name);
            }
        }

        public static void Validate(string Name, short Value, ref List<string> Message)
        {
            if (Value == 0)
            {
                Message.Add(Name);
            }
        }

        public static void Validate(string Name, int Value, ref List<string> Message)
        {
            if (Value == 0)
            {
                Message.Add(Name);
            }
        }

        public static void Validate(string Name, long Value, ref List<string> Message)
        {
            if (Value == 0)
            {
                Message.Add(Name);
            }
        }

        public static void ValidatePositive(string Name, long Value, ref List<string> Message)
        {
            if (Value < 0)
            {
                Message.Add(Name);
            }
        }
    }
}
