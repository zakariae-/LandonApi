﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LandonApi.Models
{
    public class SortOptions<T, TEntity> : IValidatableObject
    {
        public string[] OrderBy { get; set; }

        // The ASP.NET Core calls this to validate the incoming parameters
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        // The service code will call this apply these sort options to a database query
        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
