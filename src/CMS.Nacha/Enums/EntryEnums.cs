namespace CMS.Nacha.Enums
{
    public enum AddendaRecordIndicatorEnum : short
    {
        NotIncluded = 0,
        Included = 1
    }

    public enum TransactionCodeEnum : short
    {
        AutomatedDepositChecking = 22,
        PrenoteOfCheckingCredit = 23,
        AutomatedPaymentChecking = 27,
        PrenoteOfCheckingDebit = 28,
        AutomatedDepositSavings = 32,
        PrenoteOfSavingsCredit = 33,
        AutomatedPaymentSavings = 37,
        PrenoteOfSavingsDebit = 38
    }

}
