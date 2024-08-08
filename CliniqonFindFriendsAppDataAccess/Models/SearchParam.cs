using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Models
{
    public class SearchParam : DateFilter
    {
        public string? Keyword { get; set; }
        public string? Gender { get; set; }
        public int RequestBy { get; set; }

    }
}
