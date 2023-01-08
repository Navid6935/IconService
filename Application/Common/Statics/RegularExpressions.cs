using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Statics;

public static class RegularExpressions
{
    public const string InvalidCharacters = "^[^\"'!@#%\\^&\\*()_\\-\\+=~`\\[\\]\\{\\}\\|;:<>\\?\\/,\\\\]{1,}$";
    public const string InvalidAddress = "^[^\"'!@#%\\^&\\*()_\\-\\+=~`\\[\\]\\{\\}\\|;:<>\\?\\/,\\\\]{1,}$";
    public const string InvalidNumber = "^[0-9]{1,}$";
    public const string InvalidSerial = "^[0-9]{18}$";
    public const string InvalidDeviceModelSerial = "^[0-9]{8}$";
    public const string InvalidMac = "^(?:[0-9a-fA-F]{2}:){5}[0-9a-fA-F]{2}|(?:[0-9a-fA-F]{2}-){5}[0-9a-fA-F]{2}|(?:[0-9a-fA-F]{2}){5}[0-9a-fA-F]{2}$";
    public const string InvalidGUUID = "^((?!00000000-0000-0000-0000-000000000000).)*$";


}
