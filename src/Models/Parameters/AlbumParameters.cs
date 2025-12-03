/*
Paramaters for our Albums
*/

public class AlbumParameters : QueryStringParameters
{
    /// <summary>
    /// A method to check that the pages asked by the user follow pagination rules.
    /// </summary>
    /// /// <param name="albumParameters">Parameters to respect for pagination</param>
    /// <param name="pageSize">Number of records asked by the user</param>
    /// <param name="pageNumber">Initial page asked by the user</param>
    /// <returns></returns>
    public static  (int, int) CorrectPaginationParameters(AlbumParameters albumParameters, int? pageSize, int? pageNumber)
    {
        int interPageSize = pageSize ?? albumParameters.PageSize;
        int interPageNumber = pageNumber ?? albumParameters.PageNumber;
        int correctPageSize =
            (albumParameters.PageSize >= interPageSize)
                ? interPageSize
                : albumParameters.PageSize;
        int correctPageNumber =
            (
                albumParameters.PageNumber * albumParameters.PageSize
                >= interPageNumber * correctPageSize
            )
                ? interPageNumber
                : albumParameters.PageNumber * albumParameters.PageSize
                    - correctPageSize;
        return (correctPageSize, correctPageNumber);
    }
}
