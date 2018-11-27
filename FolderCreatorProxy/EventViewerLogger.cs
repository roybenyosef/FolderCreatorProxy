using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderCreatorProxy
{
    internal class EventViewerLogger
    {
        internal static void WriteInfoEvent(string message)
        {
            WriteLogEvent(message, EventLogEntryType.Information, 101, 1);
        }

        internal static void WriteErrorEvent(string message)
        {
            WriteLogEvent(message, EventLogEntryType.Error, 102, 1);
        }

        internal static void WriteLogEvent(string message, EventLogEntryType type, int eventId, short category)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry(message, type, eventId, category);
            }
        }
    }
}
