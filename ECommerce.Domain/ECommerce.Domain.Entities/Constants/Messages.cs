namespace ECommerce.Domain.Entities.Constants;

public static class Messages
{
    #region Authorization
    public const string AuthorizationDenied = "AuthorizationDenied";
    public const string Authorization_CanNotGetClaimsPrincipal = "Authorization_CanNotGetClaimsPrincipal";
    #endregion

    #region Claim
    public const string Claim_AlreadyExists = "Claim_AlreadyExists";
    public const string Claim_CanNotDeleted = "Claim_CanNotDeleted";
    public const string Claim_CanNotUpdated = "Claim_CanNotUpdated";
    public const string Claim_CollectionCountMismatchComparingToIds = "Claim_CollectionCountMismatchComparingToIds";
    public const string Claim_IdIsNull = "Claim_IdIsNull";
    public const string Claim_IdsAreNull = "Claim_IdsAreNull";
    public const string Claim_NotFound = "Claim_NotFound";

    // Fluent Validation Custom Errors
    public const string Claim_TitleIsNull = "Claim_TitleIsNull";
    #endregion

    #region Person
    public const string Person_AlreadyExists = "Person_AlreadyExists";
    public const string Person_CanNotDeleted = "Person_CanNotDeleted";
    public const string Person_CollectionCountMismatchComparingToIds = "Person_CollectionCountMismatchComparingToIds";
    public const string Person_IdIsNull = "Person_IdIsNull";
    public const string Person_IdsAreNull = "Person_IdsAreNull";
    public const string Person_NotFound = "Person_NotFound";
    public const string Person_RoleIdIsNull = "Person_RoleIdIsNull";
    public const string Person_RoleIdsAreNull = "Person_RoleIdsAreNull";
    public const string Person_TokenExpired = "Person_TokenExpired";
    public const string Person_TokenInvalid = "Person_TokenInvalid";
    public const string Person_WrongPassword = "Person_WrongPassword";

    // Fluent Validation Custom Errors
    public const string Person_FirstNameIsNull = "Person_FirstNameIsNull";
    public const string Person_LastNameIsNull = "Person_LastNameIsNull";
    public const string Person_EmailIsNull = "Person_EmailIsNull";
    public const string Person_EmailIsNotValid = "Person_EmailIsNotValid";
    public const string Person_CallingCodeIsNull = "Person_CallingCodeIsNull";
    public const string Person_CallingCodeIsNotValid = "Person_CallingCodeIsNotValid";
    public const string Person_PhoneIsNull = "Person_PhoneIsNull";
    public const string Person_PhoneIsNotValid = "Person_PhoneIsNotValid";
    public const string Person_PasswordIsNull = "Person_PasswordIsNull";
    public const string Person_PasswordIsNotValid = "Person_PasswordIsNotValid";
    public const string Person_RefreshTokenDurationIsLessThanOneSecond = "Person_RefreshTokenDurationIsLessThanOneSecond";
    public const string Person_RefreshTokenIsNull = "Person_RefreshTokenIsNull";
    public const string Person_AccessTokenIsNull = "Person_AccessTokenIsNull";
    #endregion

    #region Role
    public const string Role_CanNotDeleted = "Role_CanNotDeleted";
    public const string Role_CollectionCountMismatchComparingToIds = "Role_CollectionCountMismatchComparingToIds";
    public const string Role_IdIsNull = "Role_IdIsNull";
    public const string Role_IdsAreNull = "Role_IdsAreNull";
    public const string Role_NotFound = "Role_NotFound";

    // Fluent Validation Custom Errors
    public const string Role_TitleIsNull = "Role_TitleIsNull";
    public const string Role_DetailIsNull = "Role_DetailIsNull";
    public const string Role_ClaimForRoleDtosAreNull = "Role_ClaimForRoleDtosAreNull";
    #endregion

    #region RoleClaim
    public const string RoleClaim_AlreadyExists = "RoleClaim_AlreadyExists";
    public const string RoleClaim_CanNotDeleted = "RoleClaim_CanNotDeleted";
    public const string RoleClaim_CollectionCountMismatchComparingToIds = "RoleClaim_CollectionCountMismatchComparingToIds";
    public const string RoleClaim_IdIsNull = "RoleClaim_IdIsNull";
    public const string RoleClaim_IdsAreNull = "RoleClaim_IdsAreNull";
    public const string RoleClaim_NotFound = "RoleClaim_NotFound";

    // Fluent Validation Custom Errors
    public const string RoleClaim_RoleIdIsNull = "RoleClaim_RoleIdIsNull";
    public const string RoleClaim_ClaimDtosAreNull = "RoleClaim_ClaimDtosAreNull";
    #endregion

    #region Common
    public static string AlreadyExists(string name)
    {
        return $"{name} already exists!";
    }

    public static string AreNull(string name)
    {
        return $"{name} are null!";
    }

    public static string CanNotDeleted(string name)
    {
        return $"{name} can not deleted!";
    }

    public static string CanNotUpdated(string name)
    {
        return $"{name} can not updated!";
    }

    public static string CollectionCountMismatchComparingToIds(string name)
    {
        return $"{name} collection count mismatch comparing to ids!";
    }

    public static string ConfigurationParseError(string name)
    {
        return $"Did not parse '{name}' section in appsettings.json!";
    }

    public static string IsRequired(string name)
    {
        return $"{name} is required!";
    }

    public static string IdIsNull(string name)
    {
        return $"{name}Id is null!";
    }

    public static string IdsAreNull(string name)
    {
        return $"{name}Ids are null!";
    }

    public static string IsNotValid(string name)
    {
        return $"{name} is not valid!";
    }

    public static string FilterIsNull(string name)
    {
        return $"{name} is null!";
    }

    public static string MoreThanOneRecord(string name)
    {
        return $"{name} entity returned more than one record!";
    }

    public static string NotFound(string name)
    {
        return $"{name} not found!";
    }

    public static string RepositoryFailed(string name)
    {
        return $"{name} repository failed!";
    }
    #endregion
}