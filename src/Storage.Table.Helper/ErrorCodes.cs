namespace Storage.Table.Helper;

public static class ErrorCodes
{
    public const int UnregisteredTableService = 500;
    public const int TableUnavailable = 501;
    public const int CannotUpsert = 502;
    public const int EntityDoesNotExist = 503;
    public const int Invalid = 504;
}

public static class ErrorMessages
{
    public const string UnregisteredTableService = "table service is unregistered for the storage account.";
    public const string TableUnavailable = "table is unavailable";
    public const string CannotUpsert = "data cannot be upserted into the storage table";
    public const string EntityDoesNotExist = "entity does not exist";
    public const string Invalid = "invalid";
}