namespace LINGYUN.Abp.Identity;

public class IdentitySecurityLogExtendActionConsts
{
    public static string CaptchaSucceeded { get; set; } = "CaptchaSucceeded";
    public static string CaptchaFailed { get; set; } = "CaptchaFailed";
    public static string CaptchaError { get; set; } = "CaptchaError";
    public static string LoginTwoFactorSucceeded { get; set; } = "LoginTwoFactorSucceeded";
    public static string LoginTwoFactorFailed { get; set; } = "LoginTwoFactorFailed";
    public static string LoginRecoveryCodeSucceeded { get; set; } = "LoginRecoveryCodeSucceeded";
    public static string LoginRecoveryCodeFailed { get; set; } = "LoginRecoveryCodeFailed";
    public static string ResetAuthenticator { get; set; } = "ResetAuthenticator";
    public static string VerifyAuthenticator { get; set; } = "VerifyAuthenticator";
    public static string ChangeProfilePicture { get; set; } = "ChangeProfilePicture";
    public static string RevokeSession { get; set; } = "RevokeSession";
}
