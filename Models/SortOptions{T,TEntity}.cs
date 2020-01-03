using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApplication.Infrastructure;

namespace WebApplication.Models
{
    public class SortOptions <T, TEntity> : IValidatableObject
    {
        public string[] OrderBy { get; set; }
        //asp.net core calls this to validate incoming parameters
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var processor = new SortOptionsProcessor<T, TEntity>(OrderBy);
            var validTerms = processor.GetValidTerms().Select(x => x.DateCreated);
            var invalidTerms = processor.GetAllTerms().Select(x => x.DateCreated)
                .Except(validTerms, StringComparer.OrdinalIgnoreCase);
            foreach (var term in invalidTerms)
            {
                yield return new ValidationResult($"Invalid sort term '{term}'."
                , new []{nameof(OrderBy)});
            }
        }
        //the service code will call this to apply these sort options to a database query
        public IQueryable<TEntity> Apply(IQueryable<TEntity>query)
        {
            var processor = new SortOptionsProcessor<T, TEntity>(OrderBy);
            return processor.Apply(query);
        }
    }
}