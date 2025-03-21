namespace JwtUser.Modal
{
    public class ConfirmForgotPasswordModel
    {
        public string username { get; set; }
        public string otptext { get; set; }
        public string newpassword { get; set; }
    }
}
