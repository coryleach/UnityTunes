using System;
using System.Diagnostics;

namespace Gameframe.Tunes
{
    public static class ShellUtility
    {
        /// <summary>
        /// Get the string output of a command in the shell
        /// </summary>
        /// <param name="command">Command string to be run</param>
        /// <param name="trimOutput">True if we should trim new line and line feed from end of output stream</param>
        /// <returns>String output of the command.</returns>
        public static string GetCommandResult(string command, bool trimOutput = true)
        {
#if UNITY_EDITOR_WIN
          var commandBytes = System.Text.Encoding.Unicode.GetBytes(command);
          var encodedCommand = Convert.ToBase64String(commandBytes);
          var processInfo = new ProcessStartInfo("powershell.exe", $"-EncodedCommand {encodedCommand}")
          {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true
          };
#else
            var processInfo = new ProcessStartInfo("/bin/bash", $"-c \"{command.Replace("\\", "\\\\")}\"")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
#endif

            var process = Process.Start(processInfo);
            if (process == null)
            {
                return null;
            }

            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            var exitCode = process.ExitCode;
            process.Close();

            if (exitCode != 0)
            {
                return null;
            }

            if (trimOutput)
            {
                output = output.TrimEnd('\n', '\r');
            }

            return output;
        }
    }
}