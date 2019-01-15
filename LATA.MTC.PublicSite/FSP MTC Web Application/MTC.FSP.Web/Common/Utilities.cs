using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Caching;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Common
{
    public static class Utilities
    {
        public static string GetApplicationSettingValue(string applicationSettingVariableName)
        {
            var retvalue = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(applicationSettingVariableName))
                    using (var db = new MTCDbContext())
                    {
                        if (db.MTCApplicationSettings.Any(p => p.Name == applicationSettingVariableName))
                        {
                            var mtcApplicationSetting = db.MTCApplicationSettings.FirstOrDefault(p => p.Name == applicationSettingVariableName);
                            if (mtcApplicationSetting != null)
                                retvalue = mtcApplicationSetting.Value;
                        }
                    }
            }
            catch
            {
                // ignored
            }
            return retvalue;
        }

        public static T GetFromCache<T>(string key)
        {
            if (HttpContext.Current == null) return default(T);
            if (HttpContext.Current.Cache[key] == null) return default(T);
            return (T)HttpContext.Current.Cache[key];
        }

        public static void AddToCache<T>(string key, T value, DateTime expireDate)
        {
            if (HttpContext.Current == null) return;
            if (value != null)
            {
                HttpContext.Current.Cache.Insert(key, value, null, expireDate, Cache.NoSlidingExpiration,
                    CacheItemPriority.High, null);
            }
            else
            {
                RemoveFromCache(key);
            }

        }

        public static void RemoveFromCache(string key)
        {
            if (HttpContext.Current == null) return;
            HttpContext.Current.Cache.Remove(key);
        }


        private static string _logsPath;

        public static string LogsPath
        {
            get
            {
                if (!string.IsNullOrEmpty(_logsPath)) return _logsPath;
                var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (binDirectory == null) return _logsPath;
                _logsPath = Path.Combine(binDirectory, @"..\");

                var webServerExecutionPath = HttpContext.Current?.Server.MapPath("~");
                if (!string.IsNullOrEmpty(webServerExecutionPath))
                    _logsPath = webServerExecutionPath;

                return _logsPath;
            }
            set { _logsPath = value; }
        }

        public static void LogInfo(string message)
        {
            var messageEntry = $"{DateTime.UtcNow} {message}";
            var logFilePath = LogsPath + "\\Logs.txt";
            WriteLog(messageEntry, logFilePath);
        }

        public static void LogError(string message)
        {
            var messageEntry = $"{DateTime.UtcNow} {message}";
            var logFilePath = LogsPath + "\\Errors.txt";
            WriteLog(messageEntry, logFilePath);
        }

        private static void WriteLog(string message, string logFilePath)
        {
            Console.WriteLine(message);
            try
            {
                WriteToFile(message, logFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"ERROR WriteToFile: " + ex.Message);
            }
        }

        private static readonly ReaderWriterLock Locker = new ReaderWriterLock();
        public static void WriteToFile(string input, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(fileName)) return;
                Locker.AcquireWriterLock(int.MaxValue);
                using (var w = File.AppendText(fileName))
                {
                    Debug.WriteLine(input);
                    w.WriteLine(input);
                }              
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Locker.ReleaseWriterLock();
            }
        }

    }
}