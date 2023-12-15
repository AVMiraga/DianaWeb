using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Helpers
{
    public static class FileManager
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image/");
        }

        public static bool IsSizeAllowed(this IFormFile file, int kb)
        {
            return file.Length / 1024 <= kb;
        }

        public static string SaveFile(this IFormFile file, string root, string folder)
        {
            if (!Directory.Exists(Path.Combine(root, folder)))
            {
                Directory.CreateDirectory(Path.Combine(root, folder));
            }

            string fileName = Guid.NewGuid().ToString() + file.FileName;
            string path = Path.Combine(root, folder, fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }

        public static bool DeleteFile(this string fileName, string root, string folder)
        {
            string path = Path.Combine(root, folder, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }
    }
}