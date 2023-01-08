using System.Runtime.CompilerServices;
using Application.Common;

namespace Application.Interfaces.General
{
    public interface ILogService<T>
    {
        void Info(LogType type, object data, string tag = "", [CallerMemberName] string methodName = "");
        void Warning(LogType type, object data, string tag = "", [CallerMemberName] string methodName = "");
        void Error(LogType type, object data, string tag = "", [CallerMemberName] string methodName = "");
        void Debug(LogType type, object data, string tag = "", [CallerMemberName] string methodName = "");
        void Trace(LogType type, object data, string tag = "", [CallerMemberName] string methodName = "");
        void Fatal(LogType type, object data, string tag = "", [CallerMemberName] string methodName = "");
    }
}
