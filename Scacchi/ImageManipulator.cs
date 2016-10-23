using System;
using System.Drawing;

namespace Scacchi {
	public class ImageManipulator {

		public static int x1 = 0, x2 = 0, y1 = 0, y2 = 0;

		public ImageManipulator () {
		}

		internal static bool IsRed (Color pixel) {

			float Hue = pixel.GetHue ();
			float Saturation = pixel.GetSaturation ();
			float Brightness = pixel.GetBrightness ();

			// Hue >= 325
			// Saturation >=0.25
			// 0.125 < Brightness < 0.6875   before: 0.2916666
			if (Hue >= 325 && Saturation >= 0.25 && 0.125 <= Brightness && Brightness <= 0.6875) {
				return true;

				// Hue < 22.59414
				//Saturation >= 0.25
				// 0.125 < Brightness < 0.6875 before: 0.2916666
			} else if (Hue < 22.59414 && Saturation >= 0.25 && 0.125 < Brightness && Brightness < 0.6875) {
				return true;
			}

			return false;

		}


		internal static string GetCoordinates (int x, int y, int w, int h) {

			char [] Columns = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

			//Get the column
			int Column = 0;
			do {
				Column++;
			} while (w * Column / 8 < x);

			//Get the Row
			int Row = 0;
			do {
				Row++;
			} while (h * Row / 8 < y);
			int FRow = 9 - Row;

			return Convert.ToString (Columns [Column - 1]) + Convert.ToString (FRow);
		}

		/// <summary>
		/// Gets the human move from 2 given images.
		/// </summary>
		/// <returns>The move relevated.</returns>
		/// <param name="bmp">The "difference" image.</param>
		/// <param name="bmp1">The first image.</param>
		public static string GetHumanMove (Bitmap bmp, Bitmap bmp1) {

			int [,] corners = GetCorners (bmp);

			x1 = corners [0, 0];
			y1 = corners [0, 1];
			x2 = corners [1, 0];
			y2 = corners [1, 1];

			bmp.SetPixel (x1, y1, Color.Gold);
			bmp.SetPixel (x2, y2, Color.Gold);

			string CoordFrom, CoordTo;
			int W = bmp.Size.Width;
			int H = bmp.Size.Height;

			if (bmp1.GetPixel (x1, y1).ToArgb ().Equals (Color.Red.ToArgb ())) {
				CoordFrom = GetCoordinates (x1, y1, W, H);
				CoordTo = GetCoordinates (x2, y2, W, H);
			} else {
				CoordTo = GetCoordinates (x1, y1, W, H);
				CoordFrom = GetCoordinates (x2, y2, W, H);
			}

			return CoordFrom + CoordTo;
		}

		protected static Bitmap AdjustColors (Bitmap bmp) {

			for (int y = 0; y < bmp.Size.Height; y++) {
				for (int x = 0; x < bmp.Size.Width; x++) {

					Color CurrentPixel = bmp.GetPixel (x, y);

					if (IsRed (CurrentPixel)) {
						bmp.SetPixel (x, y, Color.Red);
					} else if (
						(CurrentPixel.R <= Values.BLACK_VALUE)
						&& (CurrentPixel.G <= Values.BLACK_VALUE)
						&& (CurrentPixel.B <= Values.BLACK_VALUE)) {
						bmp.SetPixel (x, y, Color.Black);
					} else if (
						(CurrentPixel.R >= Values.WHITE_VALUE)
						&& (CurrentPixel.G >= Values.WHITE_VALUE)
						&& (CurrentPixel.B >= Values.WHITE_VALUE)) {
						bmp.SetPixel (x, y, Color.White);
					} else {
						bmp.SetPixel (x, y, Color.Transparent);
					}
				}
			}
			return bmp;
		}

		/// <summary>
		/// Gets the differences between 2 images.
		/// </summary>
		/// <returns>The difference bitmap.</returns>
		/// <param name="bmp1">First image.</param>
		/// <param name="bmp2">Second image.</param>
		public static Bitmap getDifferenceBitmap (Bitmap bmp1, Bitmap bmp2) {

			Size s1 = bmp1.Size;
			Size s2 = bmp2.Size;

			Bitmap bmp3 = new Bitmap (s1.Width, s1.Height);

			bmp1 = AdjustColors (bmp1);
			bmp2 = AdjustColors (bmp2);

			for (int y = 0; y < bmp3.Size.Height; y++) {
				for (int x = 0; x < bmp3.Size.Width; x++) {

					bool Red1 = IsRed (bmp1.GetPixel (x, y));
					bool Red2 = IsRed (bmp2.GetPixel (x, y));

					if (bmp1.GetPixel (x, y).Equals (bmp2.GetPixel (x, y))) {
						bmp3.SetPixel (x, y, bmp1.GetPixel (x, y));
					} else {
						if ((!Red1 && Red2) || (Red1 && !Red2))
							bmp3.SetPixel (x, y, Color.Purple);
					}
				}
			}
			return bmp3;
		}

		internal static int [,] GetCorners (Bitmap bmp) {

			GetVerticalsLines (bmp);
			GetOrizontalLines (bmp);

			int [,] Corners = { { x1, y1 }, { x2, y2 } };
			return Corners;

		}

		static void GetVerticalsLines (Bitmap bmp) {

			Size s1 = bmp.Size;
			Size s2 = bmp.Size;
			if (s1 != s2) {
				return;
			}

			for (int x = 0; x < s1.Width; x++) {
				for (int y = 0; y < s1.Height; y++) {

					Color InitialPixel = bmp.GetPixel (x, y);

					if (InitialPixel.ToArgb ().Equals (Color.Purple.ToArgb ())) {

						int lenght = 0;
						Color PixelInRow;

						do {
							if (y + lenght + 1 < bmp.Size.Height) {
								lenght++;
								PixelInRow = bmp.GetPixel (x, y + lenght);
							} else {
								break;
							}
						} while (PixelInRow.Equals (InitialPixel) || PixelInRow.ToArgb ().Equals (Color.Aquamarine.ToArgb ()));

						if (lenght > Values.MIN_HEIGHT) { // Per ogni colonna con n (> MIN_HEIGHT) pixel Purple consecutivi, colorali di Aquamarine
							for (int j = y; j < y + lenght; j++) {
								bmp.SetPixel (x, j, Color.Aquamarine);
							}
						}
					}
				}
			}
		}

		static void GetOrizontalLines (Bitmap bmp) {

			bool first = true, second = false;

			Size s1 = bmp.Size;
			Size s2 = bmp.Size;
			if (s1 != s2) {
				return;
			}

			for (int y = 0; y < s1.Height; y++) {
				for (int x = 0; x < s1.Width; x++) {

					Color InitialPixel = bmp.GetPixel (x, y);

					if (InitialPixel.ToArgb ().Equals (Color.Purple.ToArgb ())) {

						int lenght = 0;
						Color PixelInRow;

						do {
							if (x + lenght + 1 < bmp.Size.Width) {
								lenght++;
								PixelInRow = bmp.GetPixel (x + lenght, y);
							} else {
								break;
							}
						} while (PixelInRow.Equals (InitialPixel) || PixelInRow.ToArgb ().Equals (Color.Aquamarine.ToArgb ()));

						if (lenght > Values.MIN_WIDTH) { // Per ogni riga con n (> MIN_WIDTH) pixel Purple consecutivi, colorali di Aquamarine
							for (int j = x; j < x + lenght; j++) {

								if (bmp.GetPixel (j, y).ToArgb ().Equals (Color.Aquamarine.ToArgb ())) { // Se il colore era già acquamarine, coloralo di Coral
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
}
