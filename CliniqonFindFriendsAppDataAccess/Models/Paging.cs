using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Models
{
    public class Paging
    {
        public bool TakeAll { get; set; }
        public int Skip { get; set; }

        public int Take { get; set; }

        public void FixUp()
        {
            if (Take == 0)
                Take = 100;
        }
    }
}
