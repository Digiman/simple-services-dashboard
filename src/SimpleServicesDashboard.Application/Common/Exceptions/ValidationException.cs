using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace SimpleServicesDashboard.Application.Common.Exceptions;

/// <summary>
/// Exception happened during validation teh request.
/// </summary>
[Serializable]
public class ValidationException : Exception
{
    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(string message) : base(message) { }
    public ValidationException(string message, Exception inner) : base(message, inner) { }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]>? Errors { get; }
}