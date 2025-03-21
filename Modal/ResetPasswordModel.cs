namespace JwtUser.Modal
{
    public class ResetPasswordModel
    {
        public string username { get; set; }
        public string oldpassword { get; set; }
        public string newpassword { get; set; }
    }
}
