using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class FileWatcher : FileSystemWatcher
    {
        public FileWatcher()
        {
            this.Path = "C:/Users/maciejt/Pictures";
            this.Filter = "*.jpg";
            this.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
           | NotifyFilters.FileName | NotifyFilters.DirectoryName; 
            
        }
    }
}
