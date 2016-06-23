using System;
using System.Drawing;
using HL.OAuth2.Client;
using HL.OAuth2.Models;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using RestSharp;
using System.Net;
using System.IO;

namespace HL.OAuth2.Console
{
    class Program
    {
        //static string token = null;

        static void Main(string[] args)
        {
            try
            {
                var p = new Program();

                #region MPNS

                if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["testMPNS"]))
                {
                    p.TestRegisterDevice();
                    p.TestPushNotification();
                    p.TestDeregisterDevice();
                }

                #endregion

                #region MPMS

                if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["testMPMS"]))
                {
                    var uploadResponse = p.TestUploadImage();

                    if (uploadResponse != null && uploadResponse is MpmsResponse && !string.IsNullOrEmpty(((MpmsResponse)uploadResponse).data.filenameguid))
                    {
                        var updateResponse = p.TestUpdateImage(((MpmsResponse)uploadResponse).data.filenameguid);

                        if (updateResponse != null && updateResponse is MpmsResponse && !string.IsNullOrEmpty(((MpmsResponse)updateResponse).data.filenameguid))
                        {
                            //p.TestRemoveImage(((MpmsResponse)updateResponse).data.filenameguid);
                            p.TestRemoveImages(new string[] { "111", "222", "333", ((MpmsResponse)updateResponse).data.filenameguid });
                        }
                    }

                    //p.TestRemoveImages(new string[] { "111", "222", "333" });
                }

                #endregion
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error:" + ex.Message + "\n" + ex.StackTrace);
            }
            System.Console.WriteLine("\nDone.");
            System.Console.ReadKey();
        }

        #region MPNS

        public void TestRegisterDevice()
        {
            MpnsClientManager.RegisterDevice("mpns1", "token1", "apns");
        }

        public void TestDeregisterDevice()
        {
            MpnsClientManager.DeregisterDevice("mpns1", "token1", "apns");
        }
        
        public void TestPushNotification()
        {
            MpnsClientManager.PushNotification("mpns1",
                                               "hlgauges://srdr/Crm/NutritionClub/Customer?contactID=1234567890",
                                               "This is a push-notification reminder to contact someone whose ID=1234567890",
                                               "n0t1f1cat10n_1d",
                                               "5");
        }

        #endregion

        #region MPMS

        public BaseResponse TestUploadImage()
        {
            // Load image from web
            WebClient wc = new WebClient();
            byte[] downloadedDataBytes = wc.DownloadData("https://www.myherbalife.com/SharedUI/images/logo-colored.png");
            MemoryStream mStream = new MemoryStream(downloadedDataBytes);
            var image = Image.FromStream(mStream);
            var fileFormat = image.RawFormat;
            var imageFormat = string.Empty;

            // Determine the image's format. Only support jpeg and png
            if (fileFormat.Equals(ImageFormat.Jpeg))
            {
                imageFormat = "jpeg";
            }
            else if (fileFormat.Equals(ImageFormat.Png))
            {
                imageFormat = "png";
            }
            else
            {
                imageFormat = "InvalidFormat";
            }

            // Encode image in base64 format
            string base64image = string.Empty;
            using (var ms = new MemoryStream())
            {
                image.Save(ms, fileFormat);
                byte[] imageBytes = ms.ToArray();
                base64image = Convert.ToBase64String(imageBytes);
            }

            // Perform upload image
            var response = MpmsClientManager.UploadImage("user1", string.Format("data:image/{0};base64,{1}", imageFormat, base64image));
            if ((int)response.StatusCode < 400)
            {
                // Successful request
                var convertedObj = JsonConvert.DeserializeObject<MpmsResponse>(response.Content);
                System.Console.WriteLine(SimpleJson.SerializeObject(convertedObj));
                return convertedObj;
            }
            else if ((int)response.StatusCode == 499)
            {
                // Failed request. Handling error.
                var convertedObj = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                System.Console.WriteLine(SimpleJson.SerializeObject(convertedObj));
                return convertedObj;
            }
            return null;

        }

        public BaseResponse TestUpdateImage(string imageId)
        {
            var image_id = imageId;
            var response = MpmsClientManager.UpdateImage(image_id, "user1", "data:image/jpg;base64,iVBORw0KGgoAAAANSUhEUgAABAAAAAQAAQMAAABF07nAAAAABlBMVEURDxH1\n9/UiPQ8IAAAENUlEQVR4nO3dQZKiQBAFUDpm4dIjcBSPBkfjKB7BpYuOZkJi\nqOqkQLSnbXHm/ZVkFFmPbUZVWPVPTgUAAAAAAAAAAAAAAAAAAAAAALAJwHu1\nkre0/hTqu7Lh+VKvL7/ata6HhwBOAAAAAAAAAAAAAAAAAAAAAAD/OuD4OMC+\nbDakGwHna6vibgnwa2H5CQAAAAAAAAAAAAAAAAAAAAAAYLOALvRJpcmcsBmf\n8sGxSz6iMAHibDGXAAAAAAAAAAAAAAAAAAAAAAAAXg6wNKY7lu8AAAAAAAAA\nAAAAAAAAAAAAAAA8BhAzjMSWblweng2oAQAAAAAAAAAAAAAAAAAAAAAAvglQ\nfw/gfAdgJgmQ54RtaWpLeQLMBAAAAAAAAAAAAAAAAAAAAAAAYJOAa1kC5Cuf\nqRTHdFcDAAAAAAAAAAAAAAAAAAAAAACQAXckHkzLZ8nibvXtDQEAAAAAAAAA\nAAAAAAAAAAAAALYBWBrTDX26st70f25cxuTjZcdxVT6lNpT6UFoHzNwCBQAA\nAAAAAAAAAAAAAAAAAAAAeAxgptnfAbo7AZPEtodxt11YlAH15bEdAXmQGAEp\nAAAAAAAAAAAAAAAAAAAAAAD/IyC+OClNAF3YO07J4sm1t/hiW92SBEifBgAA\nAAAAAAAAAAAAAAAAAAAA8AqAS5b+0DJmcrysGXebefH0o4B9uRQAAAAAAAAA\nAAAAAAAAAAAAAGARMKQqkv8rYDVxmJdzKDs/BrBfANQAAAAAAAAAAAAAAAAA\nAAAAAADXAO9VkWHYlYZpH7HUhNJxLPVpQT0+5THdsCqWegAAAAAAAAAAAAAA\nAAAAAAAAgK8A0m53APKqWJoABlQ3Lsu5lCZjuqafzUf83Kb8tPi1BwAAAAAA\nAAAAAAAAAAAAAAAAgM0CUvahdS6149OuL7I6psurfgwQB5UAAAAAAAAAAAAA\nAAAAAAAAAK8HWDXdlsF0+fErggEAAAAAAAAAAAAAAAAAAAAAADYEqMvdZkqP\nAdyWcwK0Y9fVOWEupQAAAAAAAAAAAAAAAAAAAAAAAGwSMLmYWSZf+UyAtwVm\nBrTjbqeFrgcAAAAAAAAAAAAAAAAAAAAAAIAI2C907MJuQ7Om/3yWLO32FgHx\nnSFpszxeexIgbwYAAAAAAAAAAAAAAAAAAAAAADCpddXn1AEQh3l5cveV3AjY\nB0DcDQAAAAAAAAAAAAAAAAAAAAAA4MUBi2fJVgFDqS37tOOqIcdyy3rYLeQA\nAAAAAAAAAAAAAAAAAAAAAADwZUDKObVuwzsp05Nr1edboAAAAAAAAAAAAAAA\nAAAAAAAAAJsEzCQCYnZh5frfXqalAAAAAAAAAAAAAAAAAAAAAAAAmwRcyxIg\npynlq5xHA1oAAAAAAAAAAAAAAAAAAAAAAIDbAc8MAAAAAAAAAAAAAAAAAAAA\nAADA0wG/AdWz/3VSSS8rAAAAAElFTkSuQmCC\n");
            if ((int)response.StatusCode < 400)
            {
                // Successful request
                var convertedObj = JsonConvert.DeserializeObject<MpmsResponse>(response.Content);
                System.Console.WriteLine(SimpleJson.SerializeObject(convertedObj));
                return convertedObj;
            }
            else if ((int)response.StatusCode == 499)
            {
                // Failed request. Handling error.
                var convertedObj = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                System.Console.WriteLine(SimpleJson.SerializeObject(convertedObj));
                return convertedObj;
            }
            return null;
        }

        public BaseResponse TestRemoveImage(string imageId)
        {
            var image_id = imageId;
            var response = MpmsClientManager.RemoveImage(imageId);

            if ((int)response.StatusCode < 400)
            {
                // Successful request
                var convertedObj = JsonConvert.DeserializeObject<MpmsResponse>(response.Content);
                System.Console.WriteLine(SimpleJson.SerializeObject(convertedObj));
                return convertedObj;
            }
            else if ((int)response.StatusCode == 499)
            {
                // Failed request. Handling error.
                var convertedObj = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                System.Console.WriteLine(SimpleJson.SerializeObject(convertedObj));
                return convertedObj;
            }
            return null;
        }

        public BaseResponse TestRemoveImages(string[] imageIds)
        {
            var response = MpmsClientManager.RemoveImages(imageIds);
            if ((int)response.StatusCode < 400)
            {
                // Successful request
                var convertedObj = JsonConvert.DeserializeObject<MpmsRemoveImagesResponse>(response.Content);
                System.Console.WriteLine(SimpleJson.SerializeObject(convertedObj));
                return convertedObj;
            }
            else if ((int)response.StatusCode == 499)
            {
                // Failed request. Handling error.
                var convertedObj = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                System.Console.WriteLine(SimpleJson.SerializeObject(convertedObj));
                return convertedObj;
            }
            return null;
        }

        #endregion
    }
}
