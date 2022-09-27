namespace Yorf.Results.Core;

public record ValidationError
{
    public string Identifier { get; private set; } = "";
    public string ErrorMessage { get; private set; } = "";
    public string ErrorCode { get; private set; } = "";
    public ValidationSeverity Severity { get; private set; } = ValidationSeverity.Error;

    public ValidationError(string identifier, string errorMessage)
    {
        Identifier = identifier;
        ErrorMessage = errorMessage;
    }

    public ValidationError(string identifier, string errorMessage, ValidationSeverity severity)
    {
        Identifier = identifier;
        ErrorMessage = errorMessage;
        Severity = severity;
    }


    public ValidationError(string errorCode, string identifier, string errorMessage)
    {
        ErrorCode = errorCode;
        Identifier = identifier;
        ErrorMessage = errorMessage;
    }

    public ValidationError(string errorCode, string identifier, string errorMessage, ValidationSeverity severity)
    {
        ErrorCode = errorCode;
        Identifier = identifier;
        ErrorMessage = errorMessage;
        Severity = severity;
    }
}