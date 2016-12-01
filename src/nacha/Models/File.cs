using CMS.Nacha.Constants;
using CMS.Nacha.Helpers;
using System;
using System.Collections.Generic;

namespace CMS.Nacha.Models
{
    public class File
    {
        public File()
        {
            // centralizing default values in constructor vs field initializers for 
            // readability and consistency. 
            _batches = new List<Batch>();

            Header = new FileHeader();
            Control = new FileControl();
        }

        private int _recordCount = 3; // File, Header, and Control
        private int _nextBatchNumber = 1;

        private List<Batch> _batches;

        // this has no setter and mandates a coding style but ensures proper structure
        public FileHeader Header { get; }

        // this has no setter and mandates a coding style but ensures proper structure
        public FileControl Control { get; }

        // Expose a readonly list as we want to control Add. We considered an 
        // extension method but they can't override the base class implementation. 
        public IReadOnlyList<Batch> Batches
        {
            get { return _batches.AsReadOnly(); }
        }

        public void Validate()
        {
            Header.Validate();
            Control.Validate();
        }

        public void AddBatch(Batch batch)
        {
            if (_nextBatchNumber > 999999)
            {
                throw new InvalidOperationException(
                    ExceptionConstants.File_BatchMaximumExceeded);
            }

            _recordCount = _recordCount + 2; // batch header and control

            batch.Header.BatchNumber = _nextBatchNumber;
            batch.Control.BatchNumber = _nextBatchNumber;

            Control.BatchCount = _nextBatchNumber;
            Control.BlockCount = GetBlockCount(_recordCount);

            _nextBatchNumber++;
            _batches.Add(batch);
            
            batch.EntryAdded += delegate (object sender, AddEntryEventArgs args)
            {
                // current implementation assumes no addenda records so
                // every entry will increase the record count by 1
                _recordCount++;
                
                Control.BlockCount = GetBlockCount(_recordCount);

                Control.EntryAndAddendaCount++;

                Control.EntryHash = ControlHelper.GetEntryHash(args.Entry.RdfiRtn, Control.EntryHash);

                Control.TotalDebitAmount += ControlHelper.GetDebitAmount(args.Entry);

                Control.TotalCreditAmount += ControlHelper.GetCreditAmount(args.Entry);
            };
        }

        public int GetBlockCount(int recordCount)
        {
            decimal _recordCount = Convert.ToDecimal(recordCount);
            return Convert.ToInt32(Math.Ceiling(_recordCount / 10));
        }
    }
}
