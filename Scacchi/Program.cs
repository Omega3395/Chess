using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Security.Cryptography;

namespace Scacchi {

	class Values {

		public void Configuration () { }

		public static string LOG = ""; // NON MODIFICARE, contiene la lista delle mosse effettuate

		public const int MIN_WIDTH = 10; // Dimensione minima per considerare un pedone reale (in pixel)
		public const int MIN_HEIGHT = 10;

		public const int WHITE_VALUE = 110; // Valori minimi per cui classificare bianco o nero
		public const int BLACK_VALUE = 100;

		public const int RESIZE_WIDTH = 150; // Dimensioni per il redimensionamento delle immagini (in pixel)
		public const int RESIZE_HEIGHT = 150;

	}

	class MainClass {


		public static string move = "";
		public static bool isAlreadyRunning = false;

		public static void Main (string [] args) {

			// Prendi e ridimensiona le immagini
			Bitmap bmp1 = Utils.ResizeImages (new Bitmap (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img1.jpg")), Values.RESIZE_WIDTH, Values.RESIZE_HEIGHT);
			Bitmap bmp2 = Utils.ResizeImages (new Bitmap (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img2.jpg")), Values.RESIZE_WIDTH, Values.RESIZE_HEIGHT);

			Bitmap bmp3 = ImageManipulator.getDifferencBitmap (bmp2, bmp1, Color.Red);


			bmp1.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img1-mod.png"), ImageFormat.Png);
			bmp2.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img2-mod.png"), ImageFormat.Png);

			bmp3.Save (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "img3.png"), ImageFormat.Png);


			bmp1.Dispose ();
			bmp2.Dispose ();
			bmp3.Dispose ();


			string HumanMove = "e2e4";
			Utils.AddToLog (HumanMove);

			string ComputerMove = Utils.ToEssential (CCEManipulator.GetBestMove (Values.LOG));
			Utils.AddToLog (ComputerMove);

			HumanMove = "d2d4";
			Utils.AddToLog (HumanMove);

			ComputerMove = Utils.ToEssential (CCEManipulator.GetBestMove (Values.LOG));
			Utils.AddToLog (ComputerMove);

			Console.WriteLine (Values.LOG);



			Console.WriteLine ("Processo terminato");
			Console.Read ();


		}




	}
}