using System;
using System.IO;

namespace Scacchi {

    class Values {

        public void Configuration() { }

        public static string LOG = ""; // NON MODIFICARE, contiene la lista delle mosse effettuate

        public const int MIN_RED_PIXELS = 50; // Numero minimo di pixel rossi per determinare un pedone        

        public const int WHITE_VALUE = 110; // Valori minimi per cui classificare bianco o nero
        public const int BLACK_VALUE = 70;

        public const int RESIZE_WIDTH = 152; // Dimensioni per il redimensionamento delle immagini (in pixel)
        public const int RESIZE_HEIGHT = 152;

        public static string CCE_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "stockfish-7-win", "Windows", "stockfish 7 x64.exe");

        public static string BMP1_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "img1.jpg");
        public static string BMP2_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "img2.jpg");

        public static string BMP3_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "img3.jpg");


    }

    class MainClass {

        public static void Main(string[] args) {

            string HumanMove;
            string ComputerMove;

            while (true) {

                HumanMove = ImageManipulator.HumanTurn(true);
                Values.LOG += HumanMove + " ";

                ComputerMove = CCEManipulator.GetBestMove(1000, true);
                Values.LOG += ComputerMove + " ";

                Console.WriteLine(HumanMove);
                Console.WriteLine(" " + ComputerMove);
                Console.WriteLine();

                Utils.WaitForPress();

            }

            //Console.WriteLine(Values.LOG);

            Console.WriteLine("Processo terminato");
            Console.Read();


        }




    }
}