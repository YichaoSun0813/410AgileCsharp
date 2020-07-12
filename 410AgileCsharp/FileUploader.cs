//Resources https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file

using System;
using System.IO;

namespace _410AgileCsharp
{
	class FileUploader
	{
		// example filePath: @"\\IPAddress or remote machine Name\ShareFolder\FileName.txt"
		public void Load(string filePath)
		{
			try
			{
				using StreamReader str = new StreamReader(filePath);
				var line = str.ReadToEnd();
				Console.WriteLine(line);
			}
			catch (IOException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
