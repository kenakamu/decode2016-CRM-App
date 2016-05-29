using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace QuickAppointment.DependencyService
{
    public interface IAuthenticator
    {
        /// <summary>
        /// アクセストークンの取得
        /// </summary>
        /// <param name="resource">リソース名</param>
        /// <param name="clientId">クライアント ID</param>
        /// <param name="redirectUri">Redirect URI</param>
        /// <returns></returns>
        Task<AuthenticationResult> AcquireTokenAsync(string resource, string clientId, string redirectUri);
    }
}
