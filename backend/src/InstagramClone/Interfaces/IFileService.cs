﻿using System.IO;

namespace InstagramClone.Interfaces
{
	public interface IFileService
	{
		public Task<string> SaveFile(IFormFile file, string path, string fileName);
	}
}
