using FileHelpers;
using CMS.Nacha.Constants;
using CMS.Nacha.Enums;
using System;

namespace CMS.Nacha.Models
{
    public class BaseModel
    {
        [FieldOrder(10), FieldFixedLength(1)]
        private short _recordType;

        public short RecordType
        {
            get { return _recordType; }
            protected set
            {
                if (Enum.IsDefined(typeof(RecordTypeEnum), value))
                {
                    _recordType = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(ExceptionConstants.BaseModel_RecordTypeInvalid);
                }
            }
        }
    }
}
