using System;
using System.Drawing;
using System.Threading;
namespace Scacchi {
    public class Utils {
        public Utils() {
        }

        public static string ToEssential(string arg) {
            arg = arg.Replace("bestmove ", "");
            if (arg.Contains("ponder")) {
                int I = arg.IndexOf(" ");
                return arg.Remove(I, arg.Length - I);
            }
            return arg;

        }

        public static string GetLastLine(string value) {
            for (int i = 1; i < value.Split('\n').Length; i++) {
                if (i == (value.Split('\n').Length - 2)) {
                    return value.Split('\n')[i];
                }
            }
            return "";
        }

        public static bool IsBetween(float x, double Min, double Max) {
            // return (value >= Min && value <= Max);
            if (x >= Min && x <= Max)
                return true;
            else
                return false;
        }

        public static Bitmap ResizeImages(Bitmap image, int width, int height) {

            Bitmap newImage = new Bitmap(image, new Size(width, height));
            return newImage;

        }

        public static void WaitForPress() {
            while (true) {
                Thread.Sleep(500);
                if (Console.ReadKey().Key == ConsoleKey.RightArrow) {
                    break;
                }
            }
        }

        public static void AddToLog(string value) {

            Values.LOG += value + " ";

        }

    }
}