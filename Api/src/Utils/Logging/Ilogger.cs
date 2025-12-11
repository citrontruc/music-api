/*
A simple interface to invoke a logging middleware.
*/

public interface IloggerMiddleware
{
    public Task InvokeAsync(HttpContext context);
}
