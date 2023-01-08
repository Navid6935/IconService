using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common;

public class Log
{
    public string level { get; set; }
    public string type { get; set; }
    public string tag { get; set; }
    public string time { get; set; }
    public string project_name { get; set; }
    public string class_name { get; set; }
    public string method_name { get; set; }
    public object data { get; set; }
}

public enum LogType
{
    request,
    response,
    proccess,
    exception
}
