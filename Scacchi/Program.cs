using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;


namespace Scacchi
{
	class MainClass
	{

		public static int n = 10;                                      //bmp.Size.Width / 11;
		public static int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
		public static string move = "";
		public static bool isAlreadyRunning = false;
		public static string log = "";

		public static void Main (string [] args)
		{

			Bitmap bmp1A = new Bitmap (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img1.jpg"));
			Bitmap bmp2A = new Bitmap (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img2.jpg"));

			Bitmap bmp1 = new Bitmap (bmp1A, new Size (150, 150));
			Bitmap bmp2 = new Bitmap (bmp2A, new Size (150, 150));

			Bitmap bmp3 = getDifferencBitmap (bmp2, bmp1, Color.Red);
			GetVerticalsLines (bmp3);
			GetOrizontalLines (bmp3);

			Console.WriteLine (move);

			//bmp1.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img1-mod.png"), ImageFormat.Png);
			//bmp2.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img2-mod.png"), ImageFormat.Png);

			bmp3.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img3.png"), ImageFormat.Png);


			bmp1.Dispose ();
			bmp2.Dispose ();
			bmp3.Dispose ();

			string HumanMove = "e2e4";
			log += HumanMove + " ";

			string ComputerMove = ToEssential (GetBestMove (log));
			log += ComputerMove + " ";

			HumanMove = "d2d4";
			log += HumanMove + " ";

			ComputerMove = ToEssential (GetBestMove (log));
			log += ComputerMove + " ";

			Console.WriteLine (log);


			Console.WriteLine ("Processo terminato");
			Console.Read ();


		}

		public static bool IsRed (int x, int y, Bitmap img)
		{
			if ((img.GetPixel (x, y).GetHue () >= 323) && (img.GetPixel (x, y).GetSaturation () >= 0.25) && (0.2416666 < img.GetPixel (x, y).GetBrightness ()) && (img.GetPixel (x, y).GetBrightness () < 0.6875)) {
				return true;
			} else if (((img.GetPixel (x, y).GetHue () < 22.59414) && (img.GetPixel (x, y).GetSaturation () >= 0.25) && (0.2916666 < img.GetPixel (x, y).GetBrightness ()) && (img.GetPixel (x, y).GetBrightness () < 0.6875))) {
				return true;
			} else {
				return false;
			}
		}

		public static string GetCoordinates (int x, int y, int w, int h)
		{
			string [] colon = { "a", "b", "c", "d", "e", "f", "g", "h" };
			int colonna = 0;
			do {
				colonna++;
			} while (w * colonna / 8 < x);

			int riga = 0;
			do {
				riga++;
			} while (h * riga / 8 < y);
			int rigaF = 9 - riga;
			string coordinate = Convert.ToString (colon [colonna - 1]) + Convert.ToString (rigaF);
			return coordinate;
		}

		public static void GetOrizontalLines (Bitmap bmp)
		{
			bool first = true, second = false;
			Size s1 = bmp.Size;
			Size s2 = bmp.Size;
			if (s1 == s2) {

				for (int y = 0; y < s1.Height; y++) {
					for (int x = 0; x < s1.Width; x++)                                              //Console.WriteLine((((y - 1) * s1.Width + x) * 100) / (3 * s1.Height * s1.Width) + 200/3 + " %");
					{

						if (bmp.GetPixel (x, y).ToArgb ().Equals (Color.Purple.ToArgb ())) {

							int lenght = 0;

							do {
								if (x + lenght + 1 < bmp.Size.Width) {
									lenght++;
								} else {
									break;
								}
							} while (bmp.GetPixel (x + lenght, y).Equals (bmp.GetPixel (x, y)) || bmp.GetPixel (x + lenght, y).ToArgb ().Equals (Color.Aquamarine.ToArgb ()));

							if (lenght > n) {
								for (int j = x; j < x + lenght; j++) {
									if (bmp.GetPixel (j, y).ToArgb ().Equals (Color.Aquamarine.ToArgb ())) {
										bmp.SetPixel (j, y, Color.Coral);
										if (first) {
											x1 = j;
											y1 = y;
										}
										if (second) {
											x2 = j;
											y2 = y;
										}
										first = false;
									} else {
										bmp.SetPixel (j, y, Color.Aquamarine);
									}

									second = true;

								}
							}
						}
					}
				}
			}
		}

		public static void GetVerticalsLines (Bitmap bmp)
		{
			Size s1 = bmp.Size;
			Size s2 = bmp.Size;
			if (s1 == s2) {

				for (int x = 0; x < s1.Width; x++) {
					for (int y = 0; y < s1.Height; y++)                                 //Console.WriteLine((((y - 1) * s1.Width + x) * 100) / (3 * s1.Height * s1.Width) + 200/3 + " %");
					{

						if (bmp.GetPixel (x, y).ToArgb ().Equals (Color.Purple.ToArgb ())) {

							int lenght = 0;

							do {
								if (y + lenght + 1 < bmp.Size.Height) {
									lenght++;
								} else {
									break;
								}
							} while (bmp.GetPixel (x, y + lenght).Equals (bmp.GetPixel (x, y)) || bmp.GetPixel (x, y + lenght).ToArgb ().Equals (Color.Aquamarine.ToArgb ()));

							if (lenght > n) {
								for (int j = y; j < y + lenght; j++) {
									bmp.SetPixel (x, j, Color.Aquamarine);
								}
							}
						}
					}
				}
			}
		}


		public static string GetHumanMove (Bitmap bmp, Bitmap bmp1)
		{
			bmp.SetPixel (x1, y1, Color.Gold);
			bmp.SetPixel (x2, y2, Color.Gold);

			string coordinate1, coordinate2;

			if (bmp1.GetPixel (x1, y1).ToArgb ().Equals (Color.Red.ToArgb ())) {
				coordinate1 = GetCoordinates (x1, y1, bmp.Size.Width, bmp.Size.Height);
				coordinate2 = GetCoordinates (x2, y2, bmp.Size.Width, bmp.Size.Height);
			} else {
				coordinate2 = GetCoordinates (x1, y1, bmp.Size.Width, bmp.Size.Height);
				coordinate1 = GetCoordinates (x2, y2, bmp.Size.Width, bmp.Size.Height);
			}

			return coordinate1 + coordinate2;
		}

		public static Bitmap AdgiustColors (Bitmap bmp)
		{
			int black = 100;
			int white = 110;
			for (int y = 0; y < bmp.Size.Height; y++) {
				for (int x = 0; x < bmp.Size.Width; x++) {
					if (IsRed (x, y, bmp)) {
						bmp.SetPixel (x, y, Color.Red);
					} else if ((bmp.GetPixel (x, y).R <= black) && (bmp.GetPixel (x, y).G <= black) && (bmp.GetPixel (x, y).B <= black)) {
						bmp.SetPixel (x, y, Color.Black);
					} else if ((bmp.GetPixel (x, y).R >= white) && (bmp.GetPixel (x, y).G >= white) && (bmp.GetPixel (x, y).B >= white)) {
						bmp.SetPixel (x, y, Color.White);
					} else {
						bmp.SetPixel (x, y, Color.Transparent);
					}
				}
			}
			return bmp;
		}

		public static Bitmap getDifferencBitmap (Bitmap bmp1, Bitmap bmp2, Color diffColor)
		{

			Size s1 = bmp1.Size;
			Size s2 = bmp2.Size;

			Bitmap bmp3 = new Bitmap (s1.Width, s1.Height);

			bmp1 = AdgiustColors (bmp1);
			bmp2 = AdgiustColors (bmp2);

			for (int y = 0; y < bmp3.Size.Height; y++) {
				for (int x = 0; x < bmp3.Size.Width; x++) {
					if (bmp1.GetPixel (x, y).Equals (bmp2.GetPixel (x, y))) {
						bmp3.SetPixel (x, y, bmp1.GetPixel (x, y));
					} else {
						if (((IsRed (x, y, bmp1)) && (!IsRed (x, y, bmp2))) || (!IsRed (x, y, bmp1)) && (IsRed (x, y, bmp2)))
							bmp3.SetPixel (x, y, Color.Purple);
					}
				}
			}

			return bmp3;
		}


		public static string GetBestMove (string arg = "", int time = 1000)
		{

			ProcessStartInfo processStartInfo = new ProcessStartInfo (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "stockfish-7-mac", "Mac", "stockfish-7-64"));
			processStartInfo.RedirectStandardInput = true;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.UseShellExecute = false;

			Process process = Process.Start (processStartInfo);

			if (process != null) {

				if (!isAlreadyRunning) {
					process.StandardInput.WriteLine ("uci");
					process.StandardInput.WriteLine ("isready");
					process.StandardInput.WriteLine ("ucinewgame");
					isAlreadyRunning = true;
				}

				process.StandardInput.WriteLine ("position startpos moves " + log + arg);
				process.StandardInput.WriteLine ("go movetime " + time);
				Thread.Sleep (time + 10);

				process.StandardInput.Close ();

				return GetLastLine (process.StandardOutput.ReadToEnd ());

			}

			return "";

		}

		public static string ToEssential (string arg)
		{
			arg = arg.Replace ("bestmove ", "");
			if (arg.Contains ("ponder")) {
				int I = arg.IndexOf (" ");
				return arg.Remove (I, arg.Length - I);
			}
			return arg;

		}

		public static string GetLastLine (string value)
		{
			for (int i = 1; i < value.Split ('\n').Length; i++) {
				if (i == (value.Split ('\n').Length - 2)) {
					return value.Split ('\n') [i];
				}
			}
			return "";
		}

	}
}