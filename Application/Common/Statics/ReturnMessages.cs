using System.Reflection;

namespace Application.Common.Statics;
 
public static class ReturnMessages
{
    public const string InvalidLength = "The {0} value should be {1} characters.";
    public const string InvalidGUID = "The {0} as guid not valid.";
    public const string InvalidMinimumLength = "The {0} Less Than Allowed Character Length";
    public const string InvalidMaximumLength = "The {0} More Than Allowed Character Length";
    public const string InvalidCharacters= "The {0} contains invalid character";
    public const string InvalidTypeNumber = "The {0} Should only Contain Digits";
    public const string InvalidMacAddress = "Invalid MAcAddress";
    public const string InvalidSerial = "Serial Should Be 18 Digits";
    public const string InvalidDeviceModelSerial = "Device Model Serial Should Be 8 Digits";
    public static string SuccessfulAdd(string model) => $"Added {model} Successfully.";
    public static string SuccessfulDelete(string model) => $"Deleted {model} Successfully.";
    public static string SuccessfulUpdate(string model) => $"Updated {model} Successfully.";
    public static string SuccessfulGet(string model) => $"Get {model} Successfully.";
    
    public static string FailedAdd(string model) => $"Adding {model} Failed.";
    public static string FailedDelete(string model) => $"Deleting {model} Failed.";
    public static string FailedUpdate(string model) => $"Updating {model} Failed.";
    public static string FailedGet(string model) => $"Get {model} Failed.";
    
    public static string Exception => "Internal Server Error.";

    public static string RequiredField(string field) => $"{field} Is Required.";
    public static string InvalidFormat(string field) => $"{field} Is Invalid.";
    public static string ContainsInvalidCharacter(string field) => $"{field} Contains Invalid Character(s).";
    public static string OutOfRange(string field,int min,int max) => $"{field} Must Be Between {min}-{max}.";
    public static string AlreadyExist(string model) => $"This {model} Already Exist!";
    public static string NotExist(string model) => $"This {model} Not Exist!";
    public static string NoDeviceModelForSerial() => "Serial Number Doesn't Belong To a Device Model";
    public static string BetweenError(short MinLenght,short MaxLenght,string Model) => $"This {Model} Lenght Must Be Between {MinLenght} And {MaxLenght}";
    public static string GeneralPrint(string model) => $"{model}";
}
