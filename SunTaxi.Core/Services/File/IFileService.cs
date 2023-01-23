using System;
using System.Text;

namespace SunTaxi.Core.Services
{
	public interface IFileService
	{
        Task<string[]> ProcessReadAsync(string? filePath, Encoding? encoding = null);
    }
}

