using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace emulator_helper
{
    class Program
    {
        public static string emulatorPath = "C:\\dark\\emu\\DarkOrbit 10.0\\bin\\Debug\\dark_orbit.exe";
        public static string rankingCronjobUrl = "http://yourserverurl.com/cronjobs/ranking";
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
                    run_server();
                }

                if (cronjobTime.AddHours(1) < DateTime.Now)
                {
                    run_cronjob();
                }
            }
        }

        static void run_server()
        {
            try
            {
                if (!File.Exists(emulatorPath))
                {
                    throw new FileNotFoundException();
                }

                Process.Start(emulatorPath);

                Console.WriteLine($"[{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}] - Emulator started.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(error_msg(100));
            }
            catch
            {
                Console.WriteLine(error_msg() + " - run_server()");
            }
        }

        static void run_cronjob()
        {
            try
            {
                new WebClient().DownloadString(rankingCronjobUrl);
                cronjobTime = DateTime.Now;

                Console.WriteLine($"[{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}] - Ranks updated.");
            }
            catch (WebException)
            {
                Console.WriteLine(error_msg(101));
            }
            catch
            {
                Console.WriteLine(error_msg() + " - run_cronjob()");
            }
        }

        static string error_msg(int error_code = -1)
        {
            var emulatorName = Path.GetFileName(emulatorPath);

            switch (error_code)
            {
                case 100:
                    return (string.IsNullOrEmpty(emulatorName) ? "Emulator" : emulatorName) + " not found - please update the emulator path in source code!";
                case 101:
                    return "Cronjob url for ranking does not working!";
                default:
                    return "Something went wrong!";
            }
        }
    }
}
