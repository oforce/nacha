using CMS.Nacha.Models;
using System;

namespace CMS.Nacha.Helpers
{
    public class AddEntryEventArgs:EventArgs
    {
        public Entry Entry { get; set; }
    }
}
