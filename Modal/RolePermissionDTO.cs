namespace JwtUser.Modal
{
    public class PermissionDto
    {
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }

    public class UserMenuPermissionDto
    {
        public string UserRole { get; set; }
        public string MenuCode { get; set; }
        public PermissionDto Permissions { get; set; }
    }
}
