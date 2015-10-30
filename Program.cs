using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace ApplicationLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            //place for saving application's name
            string logPath = @"log.txt";

            //dump list
            Dictionary<string, bool> appList = new Dictionary<string, bool>();

            //Create log if not exist
            if(!File.Exists(logPath))
            {
                File.Create(logPath).Dispose();
            }
            

            while (true)
            {
                //get processes
                foreach (Process p in Process.GetProcesses("."))
                {
                    try
                    {
                        if (p.MainWindowTitle.Length > 0)
                        {
                            //application process info
                            string pName = p.ProcessName.ToString();
                            string pTitle = p.MainWindowTitle.ToString();
                            
                            //log time
                            string pLogDate = DateTime.Now.ToString();

                            string pToString = pName + " -> " + pTitle;

                            //if application is already logged
                            if (appList.ContainsKey(pToString))
                            {
                                //mark existance
                                appList[pToString] = true;                               
                            }
                            else
                            {
                                //new application! logging...
                                appList.Add(pToString, true);
                                string toLog = "[" + pLogDate + "] " + pToString;
                                Console.WriteLine(toLog);
                                File.AppendAllText(logPath, toLog + Environment.NewLine);
                            }
                        }
                    }
                    catch { }
                }

                //check appList backsteps to prevent exceptions
                for (int i = appList.Count-1; i >= 0; i--)
                {
                    var item = appList.ElementAt(i);
                    string key = item.Key;

                    //if application no longer exsists - remove it
                    if (appList[key] == false)
                    {
                        appList.Remove(key);
                    }
                    else
                    {
                        //unmarke it, for next round
                        appList[key] = false;
                    }
                    
                }
               
                //sleep for second, then recheck
                Thread.Sleep(1000);

            }
            
        }
    }
}
