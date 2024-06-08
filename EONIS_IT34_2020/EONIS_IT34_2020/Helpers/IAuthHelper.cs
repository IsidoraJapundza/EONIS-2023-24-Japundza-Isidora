using EONIS_IT34_2020.Models;

namespace EONIS_IT34_2020.Helpers
{
    public interface IAuthHelper
    {
        public bool AuthenticateCreds(AuthCreds creds, bool isEmployee);
        public string GenerateJwt(AuthCreds creds, string role);
    }
}
