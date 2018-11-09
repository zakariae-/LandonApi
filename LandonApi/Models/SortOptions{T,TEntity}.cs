using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LandonApi.Infrastructure;

namespace LandonApi.Models
{
    public class SortOptions<T, TEntity> : IValidatableObject
    {
        public string[] OrderBy { get; set; }

        // The ASP.NET Core calls this to validate the incoming parameters
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var processor = new SortOptionProcessor<T, TEntity>(OrderBy);
            var validTerms = processor.GetValidTerms().Select(x => x.Name);

            var invalidTerms = processor.GetAllTerms().Select(x => x.Name)
                                        .Except(validTerms, StringComparer.OrdinalIgnoreCase);

            foreach(var term in invalidTerms)
            {
                yield return new ValidationResult(
                    $"Invalid sort term '{term}'.", 
                    new[] { nameof(OrderBy) });
            }
        }

        // The service code will call this apply these sort options to a database query
        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
