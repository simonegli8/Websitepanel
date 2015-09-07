using System;
using System.Diagnostics;
using System.IO;

namespace WebsitePanel.Utils
{
	/// <summary>
	/// Provides a default implementation of running system commands.
	/// </summary>
	public sealed class DefaultCommandLineProvider : ICommandLineProvider
	{
		/// <summary>
		/// Creates a new process and executes the file specifed as if you were executing it via command-line interface.
		/// </summary>
		/// <param name="filePath">Path to the executable file (eq. .exe, .bat, .cmd and etc).</param>
		/// <param name="args">Arguments to pass to the executable file</param>
		/// <param name="outputFile">Path to the output file if you want the output to be written somewhere.</param>
		/// <returns>Output of the command being executed.</returns>
		public string Execute (string filePath, string args, string outputFile)
		{
			// launch system process
			ProcessStartInfo startInfo = new ProcessStartInfo (filePath, args);
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			startInfo.RedirectStandardOutput = true;
			startInfo.UseShellExecute = false;
			startInfo.CreateNoWindow = true;

			// get working directory from executable path
			startInfo.WorkingDirectory = Path.GetDirectoryName (filePath);
			Process proc = Process.Start (startInfo);

			// analyze results
			StreamReader reader = proc.StandardOutput;
			string results = "";
			if (!String.IsNullOrEmpty (outputFile)) {
				// stream to writer
				StreamWriter writer = new StreamWriter (outputFile);
				int BUFFER_LENGTH = 2048;
				int readBytes = 0;
				char[] buffer = new char[BUFFER_LENGTH];

				while ((readBytes = reader.Read (buffer, 0, BUFFER_LENGTH)) > 0) {
					writer.Write (buffer, 0, readBytes);
				}
				writer.Close ();
			} else {
				// return as string
				results = reader.ReadToEnd ();
			}
			reader.Close ();

			return results;
		}
	}
}

