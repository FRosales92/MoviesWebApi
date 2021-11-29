namespace MoviesWebApi.Servicios
{
    public interface IFileStorage
    {
        Task<string> SaveFile(byte[] content, string extention, string container, string contentType);
        Task<string> EditFile(byte[] content, string extention, string container, string route, string contentType);
        Task DeleteFile(string Route, string container);
    }
}
