using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace emulator_helper
{
    class Program
    {
        public static DateTime cronjobTime = new DateTime();

        static void Main(string[] args)
        {
            Console.WriteLine("Listening for restart and cronjobs...");

            while (true)
            {
                Thread.Sleep(1000);

                var runningProcess = Process.GetProcessesByName("darkorbit");

                if (runningProcess.Length == 0)
                {
                    /* try-catch prevent stack error */
                    try
                    {
                        run_server();
                    } catch
                    {
                        Console.WriteLine(error_msg(100));
                    }

                    //Process.Start("C:\\Users\\Yusuf\\Desktop\\darkorbit-emulators\\DarkOrbit 10.0\\bin\\Debug\\darkorbit.exe");
                }

                if (cronjobTime.AddHours(1) < DateTime.Now)
                {
                    Console.WriteLine($"[{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}] - Ranks updated.");
                    new WebClient().DownloadString("http://infinityorbit.com/cronjobs/ranking");
                    cronjobTime = DateTime.Now;
                }
            }
        }
        
        static void run_server()
        {
            Console.WriteLine($"[{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}] - Emulator started.");
            Process.Start("C:\\dark\\emu\\DarkOrbit 10.0\\bin\\Debug\\dark_orbit.exe");
        }
        
        static string error_msg(int error_code)
        {
            switch (error_code) {

                case 100:
                    return "Darkorbit.exe not found - Please update Path in Sourcecode!";
                    break;
                default:
                    return "0";
                    break;
            }
        }
    }
}
