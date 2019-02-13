using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;

namespace Lichen.Util
{
    public static class Error
    {
        static string logPath = "log.txt";

        public static void StartLog()
        {
            logPath = GlobalServices.GetSaveDirectory("log.txt");
#if !DEBUG
            File.Delete("log.txt");
#endif
            using (StreamWriter w = File.AppendText(logPath))
            {
                w.WriteLine("==== ==== ==== ==== ====");
                w.WriteLine(GlobalServices.GameName);
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            }
        }

        public static void EndLog()
        {
            using (StreamWriter w = File.AppendText(logPath))
            {
                w.WriteLine("Closing program normally.");
            }
        }

        public static void Log(string message)
        {
            Debug.WriteLine(message);

            using (StreamWriter w = File.AppendText(logPath))
            {
                w.WriteLine(message);
            }
        }

        public static void LogError(string message)
        {
            Log("ERROR: " + message);
        }

        public static void LogErrorAndShutdown(string message)
        {
            Log("CRITICAL ERROR: " + message);
            Log("Forcibly terminating the program.");
            //throw new Exception(message);
            Environment.Exit(0);
        }

        public static void LogWarning(string message)
        {
            Log("WARNING: " + message);
        }
    }
}
