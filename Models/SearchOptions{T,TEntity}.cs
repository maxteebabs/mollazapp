using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApplication.Infrastructure;

namespace WebApplication.Models
{
    public class SearchOptions <T, TEntity> : IValidatableObject
    {
        private string[] Search { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var processor = new SearchOptionsProcessor<T, TEntity>(Search);
            var validTerms = processor.GetValidTerms().Select(x => x.Name);
            var inValidTerms = processor.GetAllTerms().Select(x => x.Name)
                .Except(validTerms, StringComparer.OrdinalIgnoreCase);
            foreach (var term in inValidTerms)
            {
                yield return new ValidationResult($"Invalid Search Term '{term}'."
                    , new []{nameof(Search)});
            }
        }
        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            var processor = new SearchOptionsProcessor<T, TEntity>(Search);
            return processor.Apply(query);
        }
    }
}