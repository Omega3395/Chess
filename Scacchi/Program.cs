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

		public static bool isAlreadyRunning = false;
		public static string log = "";

		public static void Main (string [] args)
		{

			//string extension = "jpg";

			Bitmap bmp1A = new Bitmap (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img1.jpg"));
			Bitmap bmp2A = new Bitmap (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img2.jpg"));

			Bitmap bmp1 = new Bitmap (bmp1A, new Size (150, 150));
			Console.Write ("CN1");
			Bitmap bmp2 = new Bitmap (bmp2A, new Size (150, 150));
			Console.Write ("CN2");

			Bitmap bmp3 = getDifferencBitmap (bmp2, bmp1, Color.Red);

			//bmp1.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img1-mod.png"), ImageFormat.Png);
			//bmp2.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img2-mod.png"), ImageFormat.Png);

			bmp3.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img3.png"), ImageFormat.Png);


			bmp1.Dispose ();
			bmp2.Dispose ();
			bmp3.Dispose ();


			Console.WriteLine ("Processo terminato");
			Console.Read ();



		}

		public static bool IsRed (int x, int y, Bitmap img)
		{
			if ((img.GetPixel (x, y).GetHue () >= 323) && (img.GetPixel (x, y).GetSaturation () >= 0.25) && (0.2916666 < img.GetPixel (x, y).GetBrightness ()) && (img.GetPixel (x, y).GetBrightness () < 0.6875))  //0.7083333   //0.3541
			{
				return true;
			} else if (((img.GetPixel (x, y).GetHue () < 22.59414) && (img.GetPixel (x, y).GetSaturation () >= 0.25) && (0.2916666 < img.GetPixel (x, y).GetBrightness ()) && (img.GetPixel (x, y).GetBrightness () < 0.6875))) {
				return true;
			} else {
				return false;
			}
		}

		public static string GetCoordinates (int x, int y, int w, int h)
		{
			string [] colon = { "A", "B", "C", "D", "E", "F", "G", "H" };
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

		public static Bitmap getDifferencBitmap (Bitmap bmp1, Bitmap bmp2, Color diffColor)
		{
			int n = bmp1.Size.Width / 10;
			int x1 = 0, x2 = 0, y1 = 0, y2 = 0;             //la foto deve essere di qualsiasi qualità ma in rapporto 1:1
			bool first = true, second = false;
			Size s1 = bmp1.Size;
			Size s2 = bmp2.Size;
			if (s1 != s2)
				return null;

			Bitmap bmp3 = new Bitmap (s1.Width, s1.Height);

			for (int y = 0; y < s1.Height; y++) {
				for (int x = 0; x < s1.Width; x++) {

					//Console.WriteLine((((y-1)*s1.Width+x)*100)/(3*s1.Height*s1.Width) + " %");

					/*Color c1 = bmp1.GetPixel (x, y);
					Color c2 = bmp2.GetPixel (x, y);

					if (c1 == c2)
						//bmp3.SetPixel (x, y, c1);
						Thread.Sleep (0);
					else
						bmp3.SetPixel (x, y, diffColor);*/

					//Un pò di colori...
					byte black = 100;       //100          dipendedall'illuminazione dell'ambiente
					byte white = 110;       //110
											//byte redR = 130;        //100
											//byte redGB = 125;       //80

					//Immagine 1
					if (IsRed (x, y, bmp1)) {                                      //((bmp2.GetPixel (x, y).R >= redR) && (bmp2.GetPixel (x, y).G <= redGB) && (bmp2.GetPixel (x, y).B <= redGB)) {
						bmp1.SetPixel (x, y, Color.Red);
					} else if ((bmp1.GetPixel (x, y).R <= black) && (bmp1.GetPixel (x, y).G <= black) && (bmp1.GetPixel (x, y).B <= black)) {
						bmp1.SetPixel (x, y, Color.Black);
					} else if ((bmp1.GetPixel (x, y).R >= white) && (bmp1.GetPixel (x, y).G >= white) && (bmp1.GetPixel (x, y).B >= white)) {
						bmp1.SetPixel (x, y, Color.White);
					} else {
						bmp1.SetPixel (x, y, Color.Transparent);
					}


					//Immagine 2
					if (IsRed (x, y, bmp2)) {                                      //((bmp2.GetPixel (x, y).R >= redR) && (bmp2.GetPixel (x, y).G <= redGB) && (bmp2.GetPixel (x, y).B <= redGB)) {
						bmp2.SetPixel (x, y, Color.Red);
					} else if ((bmp2.GetPixel (x, y).R <= black) && (bmp2.GetPixel (x, y).G <= black) && (bmp2.GetPixel (x, y).B <= black)) {
						bmp2.SetPixel (x, y, Color.Black);
					} else if ((bmp2.GetPixel (x, y).R >= white) && (bmp2.GetPixel (x, y).G >= white) && (bmp2.GetPixel (x, y).B >= white)) {
						bmp2.SetPixel (x, y, Color.White);
					} else {
						bmp2.SetPixel (x, y, Color.Transparent);
					}

					//Differenze
					if (bmp1.GetPixel (x, y).Equals (bmp2.GetPixel (x, y))) {
						bmp3.SetPixel (x, y, bmp1.GetPixel (x, y));
					} else {
						if (((IsRed (x, y, bmp1)) && (!IsRed (x, y, bmp2))) || (!IsRed (x, y, bmp1)) && (IsRed (x, y, bmp2)))                                                                     //((((bmp1.GetPixel (x, y).R >= redR) && (bmp1.GetPixel (x, y).G <= redGB) && (bmp1.GetPixel (x, y).B <= redGB)) || ((bmp2.GetPixel (x, y).R >= redR) && (bmp2.GetPixel (x, y).G <= redGB) && (bmp2.GetPixel (x, y).B <= redGB))))
							bmp3.SetPixel (x, y, Color.Purple);
					}

				}

			}

			for (int x = 0; x < s1.Width; x++) {
				for (int y = 0; y < s1.Height; y++) {

					//Console.WriteLine(((((x - 1) * s1.Height + y) * 100) / (3 * s1.Height * s1.Width)) + 100/3 + " %");

					if (bmp3.GetPixel (x, y).ToArgb ().Equals (Color.Purple.ToArgb ())) {

						int lenght = 0;

						do {
							if (y + lenght + 1 < bmp3.Size.Height) {
								lenght++;
							} else {
								break;
							}
						} while ((bmp3.GetPixel (x, y + lenght).Equals (bmp3.GetPixel (x, y))) && (lenght < 126));
						//(bmp3.GetPixel (x, y + lenght).R == 128) && (bmp3.GetPixel (x, y + lenght).G == 0) && (bmp3.GetPixel (x, y + lenght).B == 128)
						if (lenght > n) {
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

					//Console.WriteLine((((y - 1) * s1.Width + x) * 100) / (3 * s1.Height * s1.Width) + 200/3 + " %");

					if (bmp3.GetPixel (x, y).ToArgb ().Equals (Color.Purple.ToArgb ())) {

						int lenght = 0;

						do {
							if (x + lenght + 1 < bmp3.Size.Width) {
								lenght++;
							} else {
								break;
							}
						} while ((bmp3.GetPixel (x + lenght, y).Equals (bmp3.GetPixel (x, y)) || bmp3.GetPixel (x + lenght, y).ToArgb ().Equals (Color.Aquamarine.ToArgb ())) && (lenght < 126));

						if (lenght > n) {
							for (int j = x; j < x + lenght; j++) {
								if (bmp3.GetPixel (j, y).ToArgb ().Equals (Color.Aquamarine.ToArgb ())) {
									bmp3.SetPixel (j, y, Color.Coral);
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
									bmp3.SetPixel (j, y, Color.Aquamarine);
								}
								//Console.WriteLine ("" + lenght);
								//Console.WriteLine ("Args: " + x + " " + y);
								second = true;

							}
						}
					}

				}
			}

			bmp3.SetPixel (x1, y1, Color.Gold);
			bmp3.SetPixel (x2, y2, Color.Gold);

			string coordinate1, coordinate2;

			if (bmp1.GetPixel (x1, y1).ToArgb ().Equals (Color.Red.ToArgb ())) {
				coordinate1 = GetCoordinates (x1, y1, bmp3.Size.Width, bmp3.Size.Height);
				coordinate2 = GetCoordinates (x2, y2, bmp3.Size.Width, bmp3.Size.Height);
			} else {
				coordinate2 = GetCoordinates (x1, y1, bmp3.Size.Width, bmp3.Size.Height);
				coordinate1 = GetCoordinates (x2, y2, bmp3.Size.Width, bmp3.Size.Height);
			}

			string path = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "coordinate.txt");

			StreamWriter sr = new StreamWriter (path);

			//sr.WriteLine(x1 + "    " + y1 + "    " + x2 + "    " + y2);
			sr.WriteLine (coordinate2 + coordinate1);

			sr.Close ();

			//Console.WriteLine(x1 + "    " + y1 + "    " + x2 + "    " + y2);
			Console.WriteLine (coordinate2 + " -> " + coordinate1);

			return bmp3;


		}

		public static string GetBestMove (string arg = "", int time = 1000)
		{

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

				process.StandardInput.WriteLine ("position startpos " + log + arg);
				process.StandardInput.WriteLine ("go movetime " + time);
				Thread.Sleep (time + 10);

				process.StandardInput.Close ();

				string outputString = process.StandardOutput.ReadToEnd ();
				for (int i = 1; i < outputString.Split ('\n').Length; i++) {
					if (i == (outputString.Split ('\n').Length - 2)) {
						string bestmove = outputString.Split ('\n') [i]; //the semi-last line, like "bestmove e7e5"
						log = log + ToEssential (bestmove) + " ";
						return bestmove;
					}
				}

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

		/*public static string returnLastLine (string value)
		{
			for (int i = 1; i < value.Split ('\n').Length; i++) {
				if (i == (value.Split ('\n').Length - 2)) {
					return value.Split ('\n') [i];
				}
			}
			return "";
		}*/

	}
}