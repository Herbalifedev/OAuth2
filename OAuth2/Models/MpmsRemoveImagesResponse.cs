using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HL.OAuth2.Models
{
    public class MpmsRemoveImagesResponse : BaseResponse
    {
        public Data data;

        public class Data
        {
            public string[] successful_list;
            public string[] failed_list;
            public DateTime updated_at;
        }
    }
}
