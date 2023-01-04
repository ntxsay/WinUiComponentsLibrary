using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth0WinUI.ViewModels
{
    public class AuthenticateInfoVM
    {
        public string AccessToken { get; init; }
        public DateTimeOffset? AccessTokenExpiration { get; init; }
        public UserInfoVM UserInfo { get; init; }
        public bool IsAuthenticated { get; init; }
    }

    public class UserInfoVM
    {
        [JsonProperty("sub")]
        public string Sub { get; init; }

        [JsonProperty("nickname")]
        public string NickName { get; init; }

        [JsonProperty("name")]
        public string Name { get; init; }

        [JsonProperty("email")]
        public string Email { get; init; }

        [JsonProperty("email_verified")]
        public bool IsEmailVerified { get; init; }


        [JsonProperty("picture")]
        public string Picture { get; init; }

        [JsonProperty("updated_at")]
        public DateTimeOffset? UpdatedAt { get; init; }

        [JsonProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")]
        public string[] Roles { get; init; }
    }
}
