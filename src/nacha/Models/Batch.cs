using Nacha.Constants;
using Nacha.Enums;
using Nacha.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Nacha.Models
{
    public class Batch
    {
        private int _nextEntryNumber = 1;
        private List<Entry> _entries;

        public Batch()
        {
            _entries = new List<Entry>();

            Header = new BatchHeader();
            Control = new BatchControl();

            // Keep the Header and Control CompanyIdentification Properties In Sync
            Header.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                switch (e.PropertyName)
                {
                    case nameof(Header.CompanyIdentification):
                        Control.CompanyIdentification = Header.CompanyIdentification;
                        break;

                    case nameof(Header.ServiceClassCode):
                        Control.ServiceClassCode = Header.ServiceClassCode;
                        break;
                }
            };

            Control.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                switch (e.PropertyName)
                {
                    case nameof(Control.CompanyIdentification):
                        Header.CompanyIdentification = Control.CompanyIdentification;
                        break;

                    case nameof(Control.ServiceClassCode):
                        Header.ServiceClassCode = Control.ServiceClassCode;
                        break;
                }
            };
        }

        protected void NotifyEntryAdded(Entry entry)
        {
            EntryAdded?.Invoke(this, new AddEntryEventArgs{ Entry = entry });
        }

        public event EventHandler<AddEntryEventArgs> EntryAdded;

        public BatchHeader Header { get; }

        public BatchControl Control { get; }

        public IReadOnlyList<Entry> Entries
        {
            get { return _entries.AsReadOnly(); }
        }

        public void AddEntry(Entry entry)
        {
            // default the header based on the first entry supplied
            if (Header.StandardEntryClassCode == null)
            {
                Header.StandardEntryClassCode = entry.StandardEntryClassCode;
            }

            // a batch can only contain one type of Entry
            if (Header.StandardEntryClassCode != entry.StandardEntryClassCode)
            {
                throw new InvalidOperationException(
                    String.Format(ExceptionConstants.Batch_StandardEntryClassCodeMismatched,
                    Header.StandardEntryClassCode, entry.StandardEntryClassCode));
            }

            // based on the batch control Entry Count being only 6 digits
            if (_nextEntryNumber >= 1000000)
            {
                throw new InvalidOperationException(
                    ExceptionConstants.Batch_EntryMaximumExceeded);
            }

            entry.Validate();

            Control.TotalCreditAmount += ControlHelper.GetCreditAmount(entry);

            Control.TotalDebitAmount += ControlHelper.GetDebitAmount(entry);

            entry.TraceNumber = CalculateEntryTraceNumber();

            _entries.Add(entry);
            _nextEntryNumber++;

            Control.EntryAndAddendaCount++;

            Control.EntryHash = ControlHelper.GetEntryHash(entry.RdfiRtn, Control.EntryHash);

            NotifyEntryAdded(entry);
        }

        private string CalculateEntryTraceNumber()
        {
            // prepend OriginatingDfi to a zero padded 7 digit entry number
            var _return = "000000" + _nextEntryNumber.ToString();
            _return = Header.OriginatingDfi + _return.Substring(_return.Length - 7, 7);
            return _return;
        }
    }
}
