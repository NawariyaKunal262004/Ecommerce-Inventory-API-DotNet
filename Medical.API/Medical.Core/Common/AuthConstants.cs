namespace Medical.Core.Common;

public static class AuthConstants
{
    public const string Admin = "Admin";
    public const string Biller = "Biller";

    public static readonly string[] ValidRoles = [Admin, Biller];
}
