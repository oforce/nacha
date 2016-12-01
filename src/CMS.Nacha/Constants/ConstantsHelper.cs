using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace CMS.Nacha.Constants
{
    public static class ConstantsHelper
    {

        public static bool IsValid(Type T, string Value)
        {
            // get the public constant fields
            var _properties = GetFields(T);

            // get the list of string values from the propertyinfo objects 
            var _constants = _properties.Select(constant => (string)constant.GetRawConstantValue()).ToList();

            // see if the provided value is in the list
            var _match = _constants.SingleOrDefault(contantValue => contantValue == Value);

            return _match != null;
        }

        public static bool IsValid(Type T, short Value)
        {
            // get the public constant fields
            var _properties = GetFields(T);

            // get the list of string values from the propertyinfo objects 
            var _constants = _properties.Select(constant => (short?)constant.GetRawConstantValue()).ToList();

            // see if the provided value is in the list
            short? _match = _constants.SingleOrDefault(contantValue => contantValue == Value);

            return _match != null;
        }

        private static IEnumerable<FieldInfo> GetFields(Type T)
        {
            return T.GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly);
        }
    }
}
