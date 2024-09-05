﻿using System;
using FluentValidation.Results;

namespace OnionArchitecture.Application.Exceptions;
public class ValidationException:Exception
{
    public ValidationException():base("One or more validation failures have occured.")
    {
        Errors = new Dictionary<string, string[]>();
    }
    public ValidationException(IEnumerable<ValidationFailure> failures):this()
    {
        Errors = failures
            .GroupBy(x=>x.PropertyName,x=>x.ErrorMessage)
            .ToDictionary(failureGroup=>failureGroup.Key,failureGroup=> failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}
