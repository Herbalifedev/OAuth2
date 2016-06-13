using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HL.OAuth2.Models
{
    public class MpmsResponse
    {
        public Data data;

        public class Data
        {
            public string id;
            public DateTime created_at;
            public DateTime updated_at;
            public Image image;

            public class Image
            {
                public ImgCategory original_jpeg;
                public ImgCategory original_png;

                public class ImgCategory
                {
                    public Img img;

                    public class Img
                    {
                        public string url;
                    }
                }
            }
        }
    }
}
