using WalletComponent.Common.JwtOptions;

namespace WalletComponent.Providers
{
    public interface IJwtProvider
    {
         JwtSettings Setting { get; set; }
    }
}
