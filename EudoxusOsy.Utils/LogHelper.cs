using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading;
using log4net;
using Imis.Web.Utils;

namespace EudoxusOsy.Utils
{
    public static class LogHelper
    {
        public static void LogError<T>(Exception ex)
        {
            LogManager.GetLogger(typeof(T)).Error(ExceptionHelper.FormatDetails(), ex);
        }

        public static void LogError<TSource>(Exception ex, TSource logSource, string message = "") where TSource : class
        {
            string name = string.Empty;
            if (logSource is string)
                name = logSource.ToString();
            else if (logSource == null)
            {
                name = "LogHelper";
            }
            else
                name = logSource.GetType().FullName;
            LogManager.GetLogger(name).Error(!string.IsNullOrWhiteSpace(message) ? ExceptionHelper.FormatDetails(message) : ExceptionHelper.FormatDetails(), ex);
        }

        public static void LogMessage<T>(string message, T logSource = default(T))
        {
            LogManager.GetLogger(typeof(T)).Info(ExceptionHelper.FormatDetails(message));
        }
    }
}