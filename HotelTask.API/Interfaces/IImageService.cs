using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelTask.API.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadCustomerAvatar(string stringImage);
    }
}
