namespace CMS.Nacha.Constants
{
    public static class ExceptionConstants
    {
        // DotNet
        public const string DotNet_ArgumentOutOfRangeException =
            "Specified argument was out of the range of valid values.\r\nParameter name: ";

        // Shared
        public const string Shared_RequiredProperties =
            "Please provide all required properties: ";

        public const string Shared_RoutingNumberInvalid =
            "RoutingTransitNumber must be 8 numeric characters.";

        // BaseModel
        public const string BaseModel_RecordTypeInvalid =
            "You must be a value from the RecordTypeEnum Enum";

        // File
        public const string File_BatchMaximumExceeded =
            "You have exceeded the maximum number of 999,999 batches.";

        // FileHeader
        public const string FileHeader_CreationDateTimeInvalid =
            "CreationDate and CreationTime must match. A mismatch is likely caused by a defect in this assembly.";

        public const string FileHeader_IdInvalid =
            "Id must be a single uppercase alphabetic character.";

        public const string FileHeader_ImmediateDestinationInvalid =
            "ImmediateDestination must have 10 characters and start with a space followed by 9 numerics.";

        public const string FileHeader_ImmediateOriginInvalid =
            "ImmediateOrigin must be 10 numeric characters.";

        // Batch
        public const string Batch_StandardEntryClassCodeMismatched =
            "This batch can only contain {0} entries. {1} supplied.";

        public const string Batch_EntryMaximumExceeded =
            "You have exceeded the maximum number of 999,999 entries.";

        // BatchHeader
        public const string BatchHeader_CompanyIdentificationInvalid =
            "CompanyIdentification must be 10 numeric characters.";

        public const string BatchHeader_OriginatingDfiInvalid =
            "OriginatingDfi must be 8 numeric characters.";

        public const string BatchHeader_OriginatorStatusCodeInvalid =
            "OriginatorStatusCode must be a value from the BatchHeaderOriginatorStatusCodeConstant Class.";

        public const string BatchHeader_ServiceClassCodeInvalid =
            "ServiceClassCode must be a value from the BatchServiceClassCodeConstant Class.";

        public const string BatchHeader_StandardEntryClassCodeInvalid =
            "StandardEntryClassCode must be a value from the StandardEntryClassCodeConstant Class.";

        //Entry
        public const string Entry_StandardEntryClassCodeInvalid =
            "StandardEntryClassCode must be a value from the StandardEntryClassCodeConstant Class.";

        public const string Entry_TransactionCodeInvalid =
            "TransactionCode must be a value from the TransactionCodeEnum Enum.";

        public const string Entry_RdfiRtnInvalid =
            "RdfiRtn must be 10 numeric characters.";

        public const string Entry_CheckDigitInvalid =
            "CheckDigit must be 1 numeric character.";

        public const string Entry_AmountInvalid =
            "Amount cannot be less than zero.";

        public const string Entry_AddendaRecordIndicatorInvalid =
            "AddendaRecordIndicator must be a value from the AddendaRecordIndicatorEnum Enum.";
    }
}
