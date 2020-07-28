﻿using System.Collections.Generic;
using FluentValidation.Results;

namespace Application.Models.Results
{
    public class ValidationError
    {
        public string Message { get; }
        public Dictionary<string, List<string>> Errors { get; } = new Dictionary<string, List<string>>();

        public ValidationError(string message)
        {
            Message = message;
        }

        public ValidationError(IEnumerable<ValidationFailure> failures)
        {
            foreach (ValidationFailure failure in failures)
            {
                if (!Errors.ContainsKey(failure.PropertyName))
                    Errors[failure.PropertyName] = new List<string>();

                Errors[failure.PropertyName].Add(failure.ErrorMessage);
            }
        }
    }
}
