using System.Collections.Generic;

namespace Avanage.SorterFeelLite.UI.Models
{
    public class PageInfo
    {
        public const int ERROR = -1;
        public const int INFO = 1;

        public PageInfo(string message, int infoType) : this(message, infoType, new string[] { })
        {
        }

        public PageInfo(string message, int infoType, IEnumerable<string> errors)
        {
            Message = message;
            InfoType = infoType;
            Errors = errors;
        }

        public string Message { get; }
        public int InfoType { get; }
        public IEnumerable<string> Errors { get; }
    }
}