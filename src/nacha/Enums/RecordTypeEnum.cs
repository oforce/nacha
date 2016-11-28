namespace Nacha.Enums
{
    public enum RecordTypeEnum : short
    {
        FileHeader = 1,
        BatchHeader = 5,
        EntryDetail = 6,
        BatchControl = 8,
        FileControl = 9
    }
}
