/*
Method for pagination verification.
*/

public class PaginationParameters : QueryStringParameters
{
    /// <summary>
    /// A method to check that the pages asked by the user follow pagination rules.
    /// </summary>
    /// /// <param name="albumParameters">Parameters to respect for pagination</param>
    /// <param name="pageSize">Number of records asked by the user</param>
    /// <param name="pageNumber">Initial page asked by the user</param>
    /// <returns></returns>
    public static (int, int) CorrectPaginationParameters(
        PaginationParameters paginationParameters,
        int? pageSize,
        int? pageNumber
    )
    {
        int interPageSize = pageSize ?? paginationParameters.PageSize;
        int interPageNumber = pageNumber ?? paginationParameters.PageNumber;
        int correctPageSize =
            (paginationParameters.PageSize >= interPageSize)
                ? interPageSize
                : paginationParameters.PageSize;
        int correctPageNumber =
            (
                paginationParameters.PageNumber * paginationParameters.PageSize
                >= interPageNumber * correctPageSize
            )
                ? interPageNumber
                : paginationParameters.PageNumber * paginationParameters.PageSize - correctPageSize;
        return (correctPageSize, correctPageNumber);
    }
}
