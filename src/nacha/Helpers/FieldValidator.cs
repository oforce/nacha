namespace Nacha.Helpers
{
    public static class FieldValidator
    {
        public static void ValidateString(string Name, string Value, ref string Message)
        {
            if (Value == null || Value.Trim() == "")
            {
                Message += Name + ", ";
            }
        }

        public static void ValidateShort(string Name, short Value, ref string Message)
        {
            if (Value == 0)
            {
                Message += Name + ", ";
            }
        }

        public static void ValidateInt(string Name, int Value, ref string Message)
        {
            if (Value == 0)
            {
                Message += Name + ", ";
            }
        }

        public static void ValidateLong(string Name, long Value, ref string Message)
        {
            if (Value == 0)
            {
                Message += Name + ", ";
            }
        }

        public static void ValidatePositiveLong(string Name, long Value, ref string Message)
        {
            if (Value < 0)
            {
                Message += Name + ", ";
            }
        }
    }
}
