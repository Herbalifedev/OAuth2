namespace HL.OAuth2.Models
{
    public class AvatarInfo
    {
        /// <summary>
        /// Image size constants.
        /// </summary>
        internal const int SmallSize = 36;
        internal const int LargeSize = 300;

        /// <summary>
        /// Uri of small photo.
        /// </summary>
        public string Small { get; set; }

        /// <summary>
        /// Uri of normal photo.
        /// </summary>
        public string Normal { get; set; }

        /// <summary>
        /// Uri of large photo.
        /// </summary>
        public string Large { get; set; }
    }

    /// <summary>
    /// Contains information about user who is being authenticated.
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public UserInfo()
        {
            AvatarUri = new AvatarInfo();
        }

        /// <summary>
        /// Unique identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Friendly name of <see cref="UserInfo"/> provider (which is, in its turn, the client of OAuth/OAuth2 provider).
        /// </summary>
        /// <remarks>
        /// Supposed to be unique per OAuth/OAuth2 client.
        /// </remarks>
        public string ProviderName { get; set; }

        /// <summary>
        /// Email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// First name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Photo URI.
        /// </summary>
        public string PhotoUri
        {
            get { return AvatarUri.Normal; }
        }

        /// <summary>
        /// Contains URIs of different sizes of avatar.
        /// </summary>
        public AvatarInfo AvatarUri { get; private set; }
    }

    #region MPNS

    public class RegisterDeviceRequestInfo
    {
        public string access_token;
        public Data data;

        public RegisterDeviceRequestInfo(string _access_token, string _user_id, string _id, string _msg_service)
        {
            access_token = _access_token;
            data = new Data(_user_id, _id, _msg_service);
        }

        public class Data
        {
            public string user_id;
            public string id;
            public string msg_service;

            public Data(string _user_id, string _id, string _msg_service)
            {
                user_id = _user_id;
                id = _id;
                msg_service = _msg_service;
            }
        }
    }

    public class DeregisterDeviceRequestInfo
    {
        public string access_token;
        public Data data;

        public DeregisterDeviceRequestInfo(string _access_token, string _user_id, string _id, string _msg_service)
        {
            access_token = _access_token;
            data = new Data(_user_id, _id, _msg_service);
        }

        public class Data
        {
            public string user_id;
            public Device device;

            public Data(string _user_id, string _id, string _msg_service)
            {
                user_id = _user_id;
                device = new Device(_id, _msg_service);
            }

            public class Device
            {
                public string id;
                public string msg_service;

                public Device(string _id, string _msg_service)
                {
                    id = _id;
                    msg_service = _msg_service;
                }
            }
        }
    }

    public class CreateNotificationRequestInfo
    {
        public string access_token;
        public Data data;

        public CreateNotificationRequestInfo(string _access_token, string _notification_type, string _user_id, string _notifiable_type, string _notifiable_id)
        {
            access_token = _access_token;
            data = new Data(_notification_type, _user_id, _notifiable_type, _notifiable_id);
        }

        public class Data
        {
            public string _type;
            public string user_id;
            public string notifiable_id;
            public string notifiable_type;

            public Data(string __type, string _user_id, string _notifiable_type, string _notifiable_id)
            {
                _type = __type;
                user_id = _user_id;
                notifiable_id = _notifiable_id;
                notifiable_type = _notifiable_type;
            }
        }
    }

    public class PushNotificationRequestInfo
    {
        public string access_token;
        public Data data;

        public PushNotificationRequestInfo(string _access_token, string _user_id, string _url, string _message, string _notification_id, string _badge)
        {
            access_token = _access_token;
            data = new Data(_user_id, _url, _message, _notification_id, _badge);
        }

        public class Data
        {
            public string user_id;
            public string url;
            public string message;
            public string n10n_id;
            public string badge;

            public Data(string _user_id, string _url, string _message, string _notification_id, string _badge)
            {
                user_id = _user_id;
                url = _url;
                message = _message;
                n10n_id = _notification_id;
                badge = _badge;
            }
        }
    }

    #endregion

    #region MPMS

    public class UploadImageRequestInfo
    {
        public string access_token;
        public Data data;

        public UploadImageRequestInfo(string _access_token, string _username, string _img)
        {
            access_token = _access_token;
            data = new Data(_username, _img);
        }

        public class Data
        {
            public string username;
            public string img;

            public Data(string _username, string _img)
            {
                username = _username;
                img = _img;
            }
        }
    }

    public class UpdateImageRequestInfo
    {
        public string access_token;
        public Data data;

        public UpdateImageRequestInfo(string _access_token, string _username, string _img)
        {
            access_token = _access_token;
            data = new Data(_username, _img);
        }

        public class Data
        {
            public string username;
            public string img;

            public Data(string _username, string _img)
            {
                username = _username;
                img = _img;
            }
        }
    }

    public class RemoveImageRequestInfo
    {
        public string access_token;

        public RemoveImageRequestInfo(string _access_token)
        {
            access_token = _access_token;
        }
    }

    #endregion
}

