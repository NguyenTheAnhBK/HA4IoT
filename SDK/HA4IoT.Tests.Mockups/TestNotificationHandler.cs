﻿using System;
using System.Diagnostics;
using HA4IoT.Contracts.Logging;
using HA4IoT.Logger;

namespace HA4IoT.Tests.Mockups
{
    public class TestLogger : ILogger
    {
        public void Publish(LogEntrySeverity type, string message, params object[] parameters)
        {
            Debug.WriteLine(type + ": " + string.Format(message, parameters));
        }

        public void Info(string message, params object[] parameters)
        {
            Publish(LogEntrySeverity.Info, message, parameters);
        }

        public void Warning(string message, params object[] parameters)
        {
            Publish(LogEntrySeverity.Warning, message, parameters);
        }

        public void Warning(Exception exception, string message, params object[] parameters)
        {
            Publish(LogEntrySeverity.Warning, message + "\r\n" + exception.Message, parameters);
        }

        public void Error(string message, params object[] parameters)
        {
            Publish(LogEntrySeverity.Error, message, parameters);
        }

        public void Error(Exception exception, string message, params object[] parameters)
        {
            Publish(LogEntrySeverity.Error, message + "\r\n" + exception.Message, parameters);
        }

        public void Verbose(string message, params object[] parameters)
        {
            Publish(LogEntrySeverity.Verbose, message, parameters);
        }
    }
}
