using PayPal.Api;

public class PayPalConfiguration
{
    public static APIContext GetAPIContext()
    {
        var config = new Dictionary<string, string>
        {
            { "clientId", "AZdkr6v_1FZG68xLjacHS8bQeVRiMqCBFcaSEhyp_W8mYwjwG3hUkNkdpHoUaXiGd4VhtLdMCIW4zG_C" },
            { "clientSecret", "EOSNx_ek2CYg6N2WqM4i81m2bXAyZ72hdY-pcX23yepPpHbarU8eDLiKpBVkpZ4pFwPMOiZ2WX7FwTj3" },
            { "mode", "sandbox" } // أو "live" للإصدار الحقيقي
        };

        var accessToken = new OAuthTokenCredential(config).GetAccessToken();
        return new APIContext(accessToken);
    }
}
