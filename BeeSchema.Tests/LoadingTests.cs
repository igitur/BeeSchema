using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace BeeSchema.Tests
{
	[TestFixture]
	public class LoadingTests
	{
		[Test]
		public void LoadFromResources()
		{
			var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
			var resourcesDirectory = Path.Combine(currentDirectory, "Resources");

			foreach (var dir in Directory.GetDirectories(resourcesDirectory))
			{
				var binFile = Directory.GetFiles(dir, "*.bin").SingleOrDefault();
				var beeFile = Directory.GetFiles(dir, "*.bee").SingleOrDefault();

				if (binFile == null)
					throw new InvalidOperationException($"Expected exactly one .bin file in {dir}");

				if (beeFile == null)
					throw new InvalidOperationException($"Expected exactly one .bee file in {dir}");

				Assert.IsTrue(CanParse(binFile, beeFile));
			}
		}

		private bool CanParse(string binFile, string beeFile)
		{
			try
			{
				var schema = Schema.FromFile(beeFile);
				// Parse our example binary data with the resulting Schema instance.
				var result = schema.Parse(binFile);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
