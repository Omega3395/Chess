using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml.Serialization;
using System.Collections.Generic;

using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading;
using System.Linq;
using System.Reflection;


namespace Scacchi
{
	class MainClass
	{
		public static void Main (string [] args)
		{

			string extension = "jpg";

			Bitmap bmp1 = new Bitmap (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img1.jpg"));
			Bitmap bmp2 = new Bitmap (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img2.jpg"));
			Bitmap bmp3 = getDifferencBitmap (bmp2, bmp1, Color.Red);

			//bmp1.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img1-mod.png"), ImageFormat.Png);
			//bmp2.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img2-mod.png"), ImageFormat.Png);

			bmp3.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img3.png"), ImageFormat.Png);

			bmp1.Dispose ();
			bmp2.Dispose ();
			bmp3.Dispose ();

			//Detect_object();

			//getResult ();

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

					/*Color c1 = bmp1.GetPixel (x, y);
					Color c2 = bmp2.GetPixel (x, y);

					if (c1 == c2)
						//bmp3.SetPixel (x, y, c1);
						Thread.Sleep (0);
					else
						bmp3.SetPixel (x, y, diffColor);*/

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

		/*public static void getResult ()
		{

			const int width = 7; //width of chessboard no. squares in width - 1
			const int height = 7; // heght of chess board no. squares in heigth - 1
			Size patternSize = new Size (width, height); //size of chess board to be detected

			Bgr [] line_colour_array = new Bgr [width * height]; // just for displaying coloured lines of detected chessboard
			Image<Gray, Byte> [] Frame_array_buffer = new Image<Gray, byte> [100];
			MCvPoint3D32f [] [] corners_object_list = new MCvPoint3D32f [Frame_array_buffer.Length] [];
			PointF [] [] corners_points_list = new PointF [Frame_array_buffer.Length] [];

			IntrinsicCameraParameters IC = new IntrinsicCameraParameters ();
			ExtrinsicCameraParameters [] EX_Param;

			//Image<Gray, Byte> InputImage = new Image<Gray, byte> ("img1.png");

			//CvInvoke.cvShowImage("image input: ", InputImage.Ptr);

			//PointF [] corners = new PointF [] { };
			//corners = CameraCalibration.FindChessboardCorners (InputImage, PatternSize, Emgu.CV.CvEnum.CALIB_CB_TYPE.ADAPTIVE_THRESH | Emgu.CV.CvEnum.CALIB_CB_TYPE.FILTER_QUADS);

			CameraCalibration.DrawChessboardCorners (Frame_array_buffer, patternSize, corners_points_list);

			//CvInvoke.cvShowImage("result: ", InputImage.Ptr);

		}*/

		/*public void matchimage(System.Drawing.Bitmap img1, System.Drawing.Bitmap img2) {

			string img1_ref, img2_ref;
			int count1 = 0, count2 = 0;
			bool flag = true;

			if (img1.Width == img2.Width && img1.Height == img2.Height) {
				for (int i = 0; i < img1.Width; i++) {
					for (int j = 0; j < img1.Height; j++) {
						img1_ref = img1.GetPixel(i, j).ToString();
						img2_ref = img2.GetPixel(i, j).ToString();
						if (img1_ref != img2_ref) {
							count2++;
							flag = false;
							break;
						}
						count1++;
					}

				}
				if (flag == false)
					MessageBox.Show("Sorry, Images are not same , " + count2 + " wrong pixels found");
				else
					MessageBox.Show(" Images are same , " + count1 + " same pixels found and " + count2 + " wrong pixels found");
			} else
				MessageBox.Show("can not compare this images");

			img1.Dispose();
			img2.Dispose();
		}*/

		/*public static void Detect_object() {

			Size patternSize = new Size(8, 8);

			Image<Gray, Byte> chessboardImage = new Image<Gray, byte>(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "img1-old.png"));
			//PointF[] corners;
			PointF[] corners = 
				CameraCalibration.FindChessboardCorners(
					chessboardImage,
					patternSize,
					Emgu.CV.CvEnum.CALIB_CB_TYPE.ADAPTIVE_THRESH | Emgu.CV.CvEnum.CALIB_CB_TYPE.NORMALIZE_IMAGE | Emgu.CV.CvEnum.CALIB_CB_TYPE.FILTER_QUADS
				);

			chessboardImage.FindCornerSubPix(
				new PointF[][] { corners }, 
				new Size(10, 10),
				new Size(-1, -1),
				new MCvTermCriteria(0.05));

			CameraCalibration.DrawChessboardCorners(chessboardImage, patternSize, corners);

		}*/

	}
}