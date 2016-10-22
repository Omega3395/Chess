using System;

namespace Scacchi
{
	public class ImageController
	{
		public static ImageController ()
		{

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

	}
}
