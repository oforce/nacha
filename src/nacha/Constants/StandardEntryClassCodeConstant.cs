namespace Nacha.Constants
{
    public static class StandardEntryClassCodeConstant
    {
        // description from https://en.wikipedia.org/wiki/Automated_Clearing_House
        // commented out values that are not implemented
        //public const string ARC = "ARC"; // Accounts receivable conversion. A consumer check converted to a one-time ACH debit. The difference between ARC and POP is that ARC can result from a check mailed in whereas POP is in-person
        //public const string BOC = "BOC"; // Back office conversion. A single entry debit initiated at the point of purchase or at a manned bill payment location to transfer funds through conversion to an ACH debit entry during back office processing. Unlike ARC entries, BOC conversions require that the customer be present, and that the vendor post a notice that checks may be converted to BOC ACH entries
        public const string CCD = "CCD"; // Corporate Credit or Debit Entry. Used to consolidate and sweep cash funds within an entity's controlled accounts, or make/collect payments to/from other corporate entities.
        //public const string CTX = "CTX"; // Corporate trade exchange. Transactions that include ASC X12 or EDIFACT information.
        //public const string POP = "POP"; // Point-of-purchase. A check presented in-person to a merchant for purchase is presented as an ACH entry instead of a physical check.
        public const string PPD = "PPD"; // Prearranged payment and deposits. Used to credit or debit a consumer account. Popularly used for payroll direct deposits and preauthorized bill payments.
        //public const string RCK = "RCK"; // Represented check entries. A physical check that was presented but returned because of insufficient funds may be represented as an ACH entry.
        //public const string TEL = "TEL"; // Telephone-initiated entry. Oral authorization by telephone to issue an ACH entry such as checks by phone. (TEL code allowed for inbound telephone orders only. NACHA disallows the use of this code for outbound telephone solicitations unless a prior business arrangement with the customer has been established.)
        //public const string WEB = "WEB"; // Web-initiated entry. Electronic authorization through the Internet to create an ACH entry.
    }
}
