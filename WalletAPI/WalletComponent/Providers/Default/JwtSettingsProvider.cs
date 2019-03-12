using Microsoft.Extensions.Options;
using WalletComponent.Common.JwtOptions;

namespace WalletComponent.Providers.Default
{
    public class JwtSettingsProvider: IJwtProvider
    {
        public JwtSettings Setting { set; get; }

        public JwtSettingsProvider(IOptions<JwtSettings> setting)
        {
            Setting = setting.Value;
        }

    }
}
