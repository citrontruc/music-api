/*
An exception if a deletion operation failed.
*/

internal class DeletionFailedException : Exception
{
    public DeletionFailedException()
        : base() { }

    public DeletionFailedException(string message)
        : base(message) { }

    public DeletionFailedException(string message, Exception innerException)
        : base(message, innerException) { }
}
