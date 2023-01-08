using System.Runtime.CompilerServices;
using Application.Common;
using Application.Interfaces.General;
using ExtensionMethods;

namespace Application.Services.General
{
    public class LogService<T> : ILogService<T>

    {
        private string SolutionName { get; set; }
        public LogService()
        {
            this.SolutionName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName).Replace(".exe","");
        }
        public void Debug(LogType type, object data, string tag = "", [CallerMemberName] string methodName = "")
        {
            var Log=new Log()
            {
                class_name = typeof(T).Name,
                method_name = methodName,
                data = (data.GetType().Equals("".GetType()) ? new { result = data } : data),
                type = type.ToString(),
                tag = tag,  
                level="debug",
                project_name = this.SolutionName,
                time =DateTime.Now.Ticks.ToString()
            };
            fulDublexConnectionS.chap(Log.Serialize());
        }

        public void Error(LogType type, object data, string tag = "", [CallerMemberName] string methodName = "")
        {
            var Log = new Log()
            {
                class_name = typeof(T).Name,
                method_name = methodName,
                data = (data.GetType().Equals("".GetType()) ? new { result = data } : data),
                type = type.ToString(),
                tag = tag,
                level = "error",
                project_name = this.SolutionName,
                time = DateTime.Now.Ticks.ToString()
            };
            fulDublexConnectionS.chap(Log.Serialize());
        }

        public void Fatal(LogType type, object data, string tag = "", [CallerMemberName] string methodName = "")
        {
            var Log = new Log()
            {
                class_name = typeof(T).Name,
                method_name = methodName,
                data = (data.GetType().Equals("".GetType()) ? new { result = data } : data),
                type = type.ToString(),
                tag = tag,
                level = "fatal",
                project_name = this.SolutionName,
                time = DateTime.Now.Ticks.ToString()
            };
            fulDublexConnectionS.chap(Log.Serialize());
        }

        public void Info(LogType type, object data, string tag = "", [CallerMemberName] string methodName = "")
        {
            var Log = new Log()
            {
                class_name = typeof(T).Name,
                method_name = methodName,
                data = (data.GetType().Equals("".GetType()) ? new { result = data } : data),
                type = type.ToString(),
                tag = tag,
                level = "info",
                project_name = this.SolutionName,
                time = DateTime.Now.Ticks.ToString()
            };
            fulDublexConnectionS.chap(Log.Serialize());
        }

        public void Trace(LogType type, object data, string tag = "", [CallerMemberName] string methodName = "")
        {
            var Log = new Log()
            {
                class_name = typeof(T).Name,
                method_name = methodName,
                data = (data.GetType().Equals("".GetType()) ? new { result = data } : data),
                type = type.ToString(),
                tag = tag,
                level = "trace",
                project_name = this.SolutionName,
                time = DateTime.Now.Ticks.ToString()
            };
            fulDublexConnectionS.chap(Log.Serialize());
        }

        public void Warning(LogType type, object data, string tag = "", [CallerMemberName] string methodName = "")
        {
            var Log = new Log()
            {
                class_name = typeof(T).Name,
                method_name = methodName,
                data = (data.GetType().Equals("".GetType()) ? new { result = data } : data),
                type = type.ToString(),
                tag = tag,
                level = "warning",
                project_name = this.SolutionName,
                time = DateTime.Now.Ticks.ToString()
            };
            fulDublexConnectionS.chap(Log.Serialize());
        }
    }
}
