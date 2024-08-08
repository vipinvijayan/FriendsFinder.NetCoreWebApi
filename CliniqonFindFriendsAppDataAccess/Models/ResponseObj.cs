using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Models
{
    public class ResponseObj
    {
        public string ReleaseVersion { get; }
        public ResponseObj()
        {
            this.ReleaseVersion = "Release_Version_1.0.1";

        }
        public string Result { get; set; }
        public object ResponseData { get; set; }
    }
}
