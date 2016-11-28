using Nacha.Models;
using System;

namespace Nacha.Helpers
{
    public class AddEntryEventArgs:EventArgs
    {
        public Entry Entry { get; set; }
    }
}
