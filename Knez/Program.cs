using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Knez
{
	internal class Program
	{
		private static int _tempo = 200;

		public static int Tempo
		{
			get { return _tempo < 0 ? 0 : _tempo; }
			set { _tempo = value; }
		}

		private static void Main(string[] args)
		{
			Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Version);
			Console.WriteLine("Welcome to Knez.exe!");
			Console.WriteLine("To increase or decrease tempo press [+] or [-] key.");
			Console.WriteLine("To display current tempo press [T] key.");
			Console.WriteLine("To terminate program.....you cannot terminate Knez!");

			Countdown();

			Task.Factory.StartNew(() =>
			{
				while (true)
				{
					var consoleKeyInfo = Console.ReadKey(true);
					switch (consoleKeyInfo.Key)
					{
						case ConsoleKey.OemPlus:
						case ConsoleKey.Add:
							Tempo -= 50;
							break;
						case ConsoleKey.OemMinus:
						case ConsoleKey.Subtract:
							Tempo += 50;
							break;
						case ConsoleKey.T:
							Console.WriteLine("Current tempo: {0}", Tempo);
							break;
						default:
							continue;
					}
				}
			});

			foreach (string str in KnezItUp())
				Console.WriteLine(str);
		}

		private static void Countdown()
		{
			Console.WriteLine("Starting in:");
			for (int i = 6; i > 0; i--)
			{
				Console.Write("\r{0}...", i);
				Thread.Sleep(1000);
			}
			Console.WriteLine("\rGO! ");
		}

		private static IEnumerable<string> KnezItUp()
		{
			while (true)
			{
				yield return "Da l' si ikada";
				Thread.Sleep(Tempo);
				yield return "Mene voljela";
				Thread.Sleep(Tempo);
				yield return "Kao tebe ja";
				Thread.Sleep(Tempo);
			}
		}
	}
}