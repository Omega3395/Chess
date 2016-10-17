using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Diagnostics;
using System.Threading;
using MiscUtil.IO;

namespace Scacchi
{
	class MainClass
	{
		public static Bitmap bmp1 = new Bitmap (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img1.jpg"));
		public static Bitmap bmp2 = new Bitmap (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img2.jpg"));
		public static bool isAlreadyRunning = false;

		public static void Main (string [] args)
		{
			/*
			Bitmap bmp3 = getDifferencBitmap (bmp2, bmp1, Color.Red);

			//bmp1.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img1-mod.png"), ImageFormat.Png);
			//bmp2.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img2-mod.png"), ImageFormat.Png);

			bmp3.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img3.png"), ImageFormat.Png);


			bmp1.Dispose ();
			bmp2.Dispose ();
			bmp3.Dispose ();
			*/

			send ("e2e4");
			send ("e2e4 e7e5");

			Console.WriteLine ("Processo terminato");

		}

		public static Bitmap getDifferencBitmap (Bitmap bmp1, Bitmap bmp2, Color diffColor)
		{
			Size s1 = bmp1.Size;
			Size s2 = bmp2.Size;
			if (s1 != s2)
				return null;

			Bitmap bmp3 = new Bitmap (s1.Width, s1.Height);

			for (int y = 0; y < s1.Height; y++) {
				for (int x = 0; x < s1.Width; x++) {

					//Un pò di colori...
					byte black = 100;
					byte white = 180;
					byte redR = 200;
					byte redGB = 100;

					//Immagine 1
					if ((bmp1.GetPixel (x, y).R <= black) && (bmp1.GetPixel (x, y).G <= black) && (bmp1.GetPixel (x, y).B <= black)) {
						bmp1.SetPixel (x, y, Color.Black);
					} else if ((bmp1.GetPixel (x, y).R >= white) && (bmp1.GetPixel (x, y).G >= white) && (bmp1.GetPixel (x, y).B >= white)) {
						bmp1.SetPixel (x, y, Color.White);
					} else if ((bmp1.GetPixel (x, y).R >= redR) && (bmp1.GetPixel (x, y).G <= redGB) && (bmp1.GetPixel (x, y).B <= redGB)) {
						bmp1.SetPixel (x, y, Color.Red);
					}

					//Immagine 2
					if ((bmp2.GetPixel (x, y).R <= black) && (bmp2.GetPixel (x, y).G <= black) && (bmp2.GetPixel (x, y).B <= black)) {
						bmp2.SetPixel (x, y, Color.Black);
					} else if ((bmp2.GetPixel (x, y).R >= white) && (bmp2.GetPixel (x, y).G >= white) && (bmp2.GetPixel (x, y).B >= white)) {
						bmp2.SetPixel (x, y, Color.White);
					} else if ((bmp2.GetPixel (x, y).R >= redR) && (bmp2.GetPixel (x, y).G <= redGB) && (bmp2.GetPixel (x, y).B <= redGB)) {
						bmp2.SetPixel (x, y, Color.Red);
					}

					//Differenze
					if (bmp1.GetPixel (x, y).Equals (bmp2.GetPixel (x, y))) {
						bmp3.SetPixel (x, y, bmp1.GetPixel (x, y));
					} else {
						if ((bmp1.GetPixel (x, y).R >= 253 && bmp2.GetPixel (x, y).G <= 2 && bmp2.GetPixel (x, y).B <= 2) || (bmp2.GetPixel (x, y).R >= 253 && bmp2.GetPixel (x, y).G <= 2 && bmp2.GetPixel (x, y).B <= 2))
							bmp3.SetPixel (x, y, Color.Purple);
					}

				}

			}

			for (int x = 0; x < s1.Width; x++) {
				for (int y = 0; y < s1.Height; y++) {

					if (bmp3.GetPixel (x, y).ToArgb ().Equals (Color.Purple.ToArgb ())) {

						int lenght = 0;

						do {
							lenght++;
						} while (bmp3.GetPixel (x, y + lenght).Equals (bmp3.GetPixel (x, y)));
						//(bmp3.GetPixel (x, y + lenght).R == 128) && (bmp3.GetPixel (x, y + lenght).G == 0) && (bmp3.GetPixel (x, y + lenght).B == 128)
						if (lenght > 140) {
							for (int j = y; j < y + lenght; j++)
								bmp3.SetPixel (x, j, Color.Aquamarine);
							//Console.WriteLine ("" + lenght);
							//Console.WriteLine ("Args: " + x + " " + y);
						}
					}

				}
			}

			for (int y = 0; y < s1.Height; y++) {
				for (int x = 0; x < s1.Width; x++) {

					if (bmp3.GetPixel (x, y).ToArgb ().Equals (Color.Purple.ToArgb ())) {

						int lenght = 0;

						do {
							lenght++;
						} while (bmp3.GetPixel (x + lenght, y).Equals (bmp3.GetPixel (x, y)) || bmp3.GetPixel (x + lenght, y).ToArgb ().Equals (Color.Aquamarine.ToArgb ()));

						if (lenght > 140) {
							for (int j = x; j < x + lenght; j++)
								bmp3.SetPixel (j, y, Color.Aquamarine);
							//Console.WriteLine ("" + lenght);
							//Console.WriteLine ("Args: " + x + " " + y);
						}
					}

				}
			}



			return bmp3;
		}

		public static void send (string a)
		{

			// /Volumes/Macintosh SSD/Users/AlbertoGiacalone/Desktop/stockfish-7-mac/Mac/stockfish-7-64
			string path = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "output.txt");

			ProcessStartInfo processStartInfo = new ProcessStartInfo ("/Volumes/Macintosh SSD/Users/AlbertoGiacalone/Desktop/stockfish-7-mac/Mac/stockfish-7-64");
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

				process.StandardInput.WriteLine ("position startpos moves " + a);
				process.StandardInput.WriteLine ("go movetime 2000");
				Thread.Sleep (2010);
				//Console.WriteLine (process.StandardOutput.ReadToEnd ());

				process.StandardInput.Close (); // line added to stop process from hanging on ReadToEnd()

				string outputString = process.StandardOutput.ReadToEnd ();
				string lastline = "";
				for (int i = 1; i < outputString.Split ('\n').Length; i++) {
					if (i == (outputString.Split ('\n').Length - 2)) {
						lastline = outputString.Split ('\n') [i];
					}
				}

				Console.WriteLine (lastline);
			}


		}


	}
}