using SNR_ClientApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    internal class FileManagerService
    {

        public static readonly String FILE_EXTENSION = ".ser";

    private String snrichPath;
        public FileManagerService()
        {
            snrichPath = ApplicationProperties.properties.GetValueOrDefault("snrich.dir").ToString();
        }
        internal void deleteFile(string fileName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(snrichPath));
            File.Delete(snrichPath + fileName + FILE_EXTENSION);
        }
    }
}
