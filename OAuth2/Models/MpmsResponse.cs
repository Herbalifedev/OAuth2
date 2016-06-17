using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HL.OAuth2.Models
{
    public class MpmsResponse : BaseResponse
    {
        public Data data;

        public class Data
        {
            public string filenameguid;
            public string baseurl;
            public DateTime created_at;
            public DateTime updated_at;
        }
    }
}
