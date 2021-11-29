using Microsoft.EntityFrameworkCore;

namespace MoviesWebApi.Helpers
{
    public static class HttpContextExtensions
    {
        //HttpContex is extended to add headers, Iqueryable is used to get all cuantity records from the table 
        public async static Task InsertParameterPagination<T>(this HttpContext httpContext, 
            IQueryable<T> queryable, int recordsPerPage)
        {
            double cuantity = await queryable.CountAsync();
            double cuantityPages = Math.Ceiling(cuantity / recordsPerPage);
            httpContext.Response.Headers.Add("cuantityPages", cuantityPages.ToString());
        }
    }
}
