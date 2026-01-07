/*
An exception if a deletion operation failed.
*/

public class DeletionFailedException : Exception
{
    public DeletionFailedException()
        : base() { }

    public DeletionFailedException(string message)
        : base(message) { }
}
