using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Scacchi {
	public class CCEManipulator {

		public CCEManipulator () {
		}

		public static string GetBestMove (string arg, int time = 1000) {

			ProcessStartInfo processStartInfo = new ProcessStartInfo (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "stockfish-7-mac", "Mac", "stockfish-7-64"));
			processStartInfo.RedirectStandardInput = true;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.UseShellExecute = false;

			Process process = Process.Start (processStartInfo);

			if (process != null) {

				process.StandardInput.WriteLine ("position startpos moves " + Values.LOG + arg);
				process.StandardInput.WriteLine ("go movetime " + time);
				Thread.Sleep (time + 10);

				process.StandardInput.Close ();

				return Utils.GetLastLine (process.StandardOutput.ReadToEnd ());

			}

			return null;

		}

	}
}
