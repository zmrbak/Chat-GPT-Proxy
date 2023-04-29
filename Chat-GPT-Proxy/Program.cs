using OpenAI;
using OpenAI.Proxy;
using System.Security.Authentication;

namespace Chat_GPT_Proxy
{
    public class Program
    {
        private const string UserToken = "sess-";
        private class AuthenticationFilter : AbstractAuthenticationFilter
        {
            public override void ValidateAuthentication(IHeaderDictionary request)
            {
                if (!request.Authorization.ToString().Contains(UserToken))
                {
                    throw new AuthenticationException("User is not authorized");
                }
            }
        }

        public static void Main(string[] args)
        {
            var auth = OpenAIAuthentication.LoadFromEnv();
            var settings = new OpenAIClientSettings(/* your custom settings if using Azure OpenAI */);
            var openAIClient = new OpenAIClient(auth, settings);
            var proxy = OpenAIProxyStartup.CreateDefaultHost<AuthenticationFilter>(args, openAIClient);
            proxy.Run();
        }
    }
}
