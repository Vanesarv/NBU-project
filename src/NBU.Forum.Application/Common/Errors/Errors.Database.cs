namespace NBU.Forum.Application.Common.Errors;

using ErrorOr;

public static partial class Errors
{
    public static class Database
    {
        public static readonly Error Concurrency = Error.Failure("Database.Concurrency",
            "Concurrency violation occured while saving to the database.");

        public static readonly Error Saving = Error.Failure("Database.Saving",
            "An error occured while saving to the database.");

        public static readonly Error NotFound = Error.Failure("Database.NotFound",
            "Entity not found.");

        public static readonly Error Unexpected = Error.Failure("Database.Unexpected",
            "An unexpected error has occured.");
    }
}
