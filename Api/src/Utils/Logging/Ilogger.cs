/*
A simple interface to invoke a logging middleware.
*/

internal interface IloggerMiddleware
{
    public Task InvokeAsync(HttpContext context);
}
