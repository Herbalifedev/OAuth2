using System;
using HL.OAuth2.Client;

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
                p.TestRegisterDevice();
                p.TestPushNotification();
                p.TestDeregisterDevice();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error:" + ex.Message + "\n" + ex.StackTrace);
            }
            System.Console.WriteLine("\nDone.");
            System.Console.ReadKey();
        }

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
    }
}
