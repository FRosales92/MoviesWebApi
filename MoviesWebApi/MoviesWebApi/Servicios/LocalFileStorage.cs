namespace MoviesWebApi.Servicios
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment env; //Obtiene la ruta 
        private readonly IHttpContextAccessor httpContextAccessor; // Obtiene el dominio

        public LocalFileStorage(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }
        public Task DeleteFile(string route, string container)
        {
            if (route != null)
            {
                var fileName = Path.GetFileName(route);
                string directoryFile = Path.Combine(env.WebRootPath, container, fileName);
                if (File.Exists(directoryFile))
                {
                    File.Delete(directoryFile);
                }
            }
            return Task.FromResult(0);  
        }

        public async Task<string> EditFile(byte[] content, string extention, string container, string route, string contentType)
        {
            await DeleteFile(route, container);
            return await SaveFile(content, extention, container, contentType);
        }

        public async Task<string> SaveFile(byte[] content, string extention, string container, string contentType)
        {
            var fileName = $"{Guid.NewGuid()}{extention}";
            string folder =  Path.Combine(env.WebRootPath, container);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string route = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(route,content);

            var urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var urlToDB = Path.Combine(urlActual, container, fileName).Replace("\\","//");
            return urlToDB;

        }
    }
}
