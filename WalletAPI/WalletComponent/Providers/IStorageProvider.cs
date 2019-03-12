using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WalletComponent.Providers
{
    public interface IStorageProvider
    {
        Task UploadStreamAsync(Stream stream, string path);
    }
}
