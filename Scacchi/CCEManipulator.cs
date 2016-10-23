using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Scacchi {
	public class CCEManipulator {

		public CCEManipulator () {
		}

		static Process CCE;

		public static void StartCCE (string path) {

			ProcessStartInfo CCEStartInfo = new ProcessStartInfo (path);
			CCEStartInfo.RedirectStandardInput = true;
			CCEStartInfo.RedirectStandardOutput = true;
			CCEStartInfo.UseShellExecute = false;

			CCE = Process.Start (CCEStartInfo);

			return;

		}

		public static void CloseCCE () {

			CCE.Close ();

			return;

		}

		public static string GetBestMove (string arg, int time = 1000) {



			if (CCE != null) {

				CCE.StandardInput.WriteLine ("position startpos moves " + Values.LOG + arg);
				CCE.StandardInput.WriteLine ("go movetime " + time);
				Thread.Sleep (time + 10);

				CCE.StandardInput.Close ();

				return Utils.GetLastLine (CCE.StandardOutput.ReadToEnd ());

			}

			return null;

		}

	}
}
