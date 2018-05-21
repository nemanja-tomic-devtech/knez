using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Knez
{
    internal static class Program
    {
        private static int _tempo = 200;
        private static int COUNTDOWN_TIMER = 6;

        public static int Tempo
        {
            get => _tempo < 0 ? 0 : _tempo;
            set => _tempo = value;
        }

        private static void Main(string[] args)
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine("Welcome to Knez.exe!");
            Console.WriteLine("To increase or decrease tempo press [+] or [-] key.");
            Console.WriteLine("To display current tempo press [T] key.");
            Console.WriteLine("To start playing strofa press [S] key.");
            Console.WriteLine("To terminate program.....you cannot terminate Knez!");

            Countdown();

            var src = new CancellationTokenSource();
            var token = src.Token;

            var knezTask = knezItUp(token);

            var readTask = Task.Run(
                () => {
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
                            case ConsoleKey.S:
                                src.Cancel();
                                knezTask.Wait();

                                src = new CancellationTokenSource();
                                token = src.Token;
                                knezTask = strofaItUp(Strofa1, token);
                                break;
                            default:
                                continue;
                        }
                    }
                });



            Task.WaitAll(readTask);
        }

        private static async Task strofaItUp(List<string> lyrics, CancellationToken token)
        {
            await Task.Run(
                async () => {
                    foreach (var stih in lyrics)
                    {
                        Console.WriteLine(stih);
                        await Task.Delay(Tempo);
                    }
                }).ContinueWith(task => knezItUp(token));
        }

        private static async Task knezItUp(CancellationToken token)
        {
            await Task.Run(
                async () => {
                    while (true)
                    {
                        if (token.IsCancellationRequested)
                            break;
                        foreach (var stih in Refren)
                        {
                            if (token.IsCancellationRequested)
                                break;
                            Console.WriteLine(stih);
                            await Task.Delay(Tempo);
                        }
                    }
                });
        }



        private static void Countdown()
        {
            Console.WriteLine("Starting in:");
            for (var i = COUNTDOWN_TIMER; i > 0; i--)
            {
                Console.Write("\r{0}...", i);
                Thread.Sleep(1000);
            }
            Console.WriteLine("\rGO! ");
        }

        private static readonly List<string> Refren = new List<string> {
            "Da l' si ikada",
            "Mene voljela",
            "Kao tebe ja"
        };

        private static List<string> Strofa1 = new List<string> {
            "Nikad vise nece biti k'o sto bilo je",
            "A od svega sto je bilo malo ostaje",
            "Sada jadno moje srce tuga ubija",
            "Sve bi dao kad bi znao"
        };
    }
}