﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace logger
{
    class Logger : ILog
    {
        Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> warnings = new Dictionary<string, List<string>>();
        bool CreateDict(string message, bool isError)
        {
            string date = String.Format("{0:dd-MM-yyyy}", DateTime.Now);
            if (!isError)
            {
                try
                {
                    if (!warnings[date].Contains(message)) warnings[date].Add(message);
                    else return false;
                }
                catch (KeyNotFoundException)
                {
                    List<string> error = new List<string>
                {
                    message
                };
                    warnings.Add(date, error);
                }
                return true;
            }
            else
            {
                try
                {
                    if (!errors[date].Contains(message)) errors[date].Add(message);
                    else return false;
                }
                catch (KeyNotFoundException)
                {
                    List<string> error = new List<string>
                {
                    message
                };
                    errors.Add(date, error);
                }
                return true;
            }
        }
        string CreatePath (string type)
        {
            string date = String.Format("{0:dd-MM-yyyy}", DateTime.Now);
            string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\" + date;
            Directory.CreateDirectory(dir);
            string path = dir + @"\" + type + ".log";
            return path;
        }
        void CreateLog (string path, string text)
        {
            TextWriter file = new StreamWriter(path, true);
            Console.WriteLine(text);
            file.WriteLine(text);
            file.Close();
        }
        public void Debug(string message)
        {
            string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (DEBUG): {1}", DateTime.Now, message);
            string path = CreatePath("debug");
            CreateLog(path, text);
        }

        public void Debug(string message, Exception e)
        {
            string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (DEBUG): {1} {2}", DateTime.Now, message, e.Message);
            string path = CreatePath("debug");
            CreateLog(path, text);
        }

        public void DebugFormat(string message, params object[] args)
        {
            string arg = "";
            for (int i = 0; i < args.Length; i++) arg += args[i];
            string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (DEBUG): {1} {2}", DateTime.Now, message, arg);
            string path = CreatePath("debug");
            CreateLog(path, text);
        }

        public void Error(string message)
        {
            CreateDict(message,true);
            string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (ERROR): {1}", DateTime.Now, message);
            string path = CreatePath("error");
            CreateLog(path, text);
        }

        public void Error(string message, Exception e)
        {
            CreateDict(e.Message,true);
            string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (ERROR): {1} {2}", DateTime.Now, message, e.Message);
            string path = CreatePath("error");
            CreateLog(path, text);
        }

        public void Error(Exception ex)
        {
            CreateDict(ex.Message, true);
            string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (ERROR): {1}", DateTime.Now, ex.Message);
            string path = CreatePath("error");
            CreateLog(path, text);
        }

        public void ErrorUnique(string message, Exception e)
        {
            if(CreateDict(e.Message, true))
            {
                string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (ERROR): {1} {2}", DateTime.Now, message, e.Message);
                string path = CreatePath("error");
                CreateLog(path, text);
            }
        }

        public void Fatal(string message)
        {
            string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (FATAL): {1}", DateTime.Now, message);
            string path = CreatePath("fatal");
            CreateLog(path, text);
            System.Environment.Exit(-1);
        }

        public void Fatal(string message, Exception e)
        {
            string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (FATAL): {1} {2}", DateTime.Now, message, e.Message);
            string path = CreatePath("fatal");
            CreateLog(path, text);
            System.Environment.Exit(-1);
        }

        public void Info(string message)
        {
            string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (INFO): {1}", DateTime.Now, message);
            string path = CreatePath("info");
            CreateLog(path, text);
        }

        public void Info(string message, Exception e)
        {
            string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (INFO): {1} {2}", DateTime.Now, message, e.Message);
            string path = CreatePath("info");
            CreateLog(path, text);
        }

        public void Info(string message, params object[] args)
        {
            string arg = "";
            for (int i = 0; i < args.Length; i++) arg += args[i];
            string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (INFO): {1} {2}", DateTime.Now, message, arg);
            string path = CreatePath("info");
            CreateLog(path, text);
        }

        public void SystemInfo(string message, Dictionary<object, object> properties)
        {
            string arg = "CPU: " + properties["CPU"].ToString() + ", GPU: " + properties["GPU"].ToString();
            string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (SYSTEMINFO): {1} {2}", DateTime.Now, message, arg);
            string path = CreatePath("systeminfo");
            CreateLog(path, text);
        }

        public void Warning(string message)
        {
            CreateDict(message, false);
            string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (WARNING): {1}", DateTime.Now, message);
            string path = CreatePath("warning");
            CreateLog(path, text);
        }

        public void Warning(string message, Exception e)
        {
            CreateDict(e.Message, false);
            string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (WARNING): {1} {2}", DateTime.Now, message, e.Message);
            string path = CreatePath("warning");
            CreateLog(path, text);
        }

        public void WarningUnique(string message)
        {
            if (CreateDict(message, false))
            {
                string text = String.Format("{0:dd-MM-yyyy hh:mm:ss} (WARNING): {1}", DateTime.Now, message);
                string path = CreatePath("warning");
                CreateLog(path, text);
            }
        }
    }
}
