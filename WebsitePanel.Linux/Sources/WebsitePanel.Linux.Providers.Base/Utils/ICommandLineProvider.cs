using System;

namespace WebsitePanel.Utils
{
	/// <summary>
	/// Defines a contract that a system command provider needs to implement.
	/// </summary>
	public interface ICommandLineProvider
	{
		/// <summary>
		/// Executes the file specifed as if you were executing it via command-line interface.
		/// </summary>
		/// <param name="filePath">Path to the executable file (eq. .exe, .bat, .cmd and etc).</param>
		/// <param name="args">Arguments to pass to the executable file</param>
		/// <param name="outputFile">Path to the output file if you want the output to be written somewhere.</param>
		/// <returns>Output of the command being executed.</returns>
		string Execute (string filePath, string args, string outputFile);
	}
}

