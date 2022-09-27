using System;
using System.Collections.Generic;

namespace Yorf.Results.Core;

public interface IResult
{
    ResultStatus Status { get; }
    IEnumerable<string> Errors { get; }
    ValidationError[] ValidationErrors { get; }
    Type? ValueType { get; }
    object? GetValue();
}