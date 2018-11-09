using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LandonApi.Infrastructure
{
    public class SortOptionProcessor<T, TEntity>
    {
        private readonly string[] _orderBy;

        public SortOptionProcessor(string[] orderBy)
        {
            _orderBy = orderBy;
        }

        public IEnumerable<SortTerm> GetAllTerms()
        {
            if (_orderBy == null) yield break;

            foreach(var term in _orderBy)
            {
                if (string.IsNullOrEmpty(term)) continue;

                var tokens = term.Split(' ');
                if(tokens.Length == 0)
                {
                    yield return new SortTerm { Name = term };
                    continue;
                }

                var descending = tokens.Length > 1 && tokens[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

                yield return new SortTerm
                {
                    Name = tokens[0],
                    Descending = descending
                };
            }
        }

        public IEnumerable<SortTerm> GetValidTerms()
        {
            var queryTerms = GetAllTerms().ToArray();
            if (!queryTerms.Any()) yield break;

            var declaredTerms = GetTermFromModel();

            foreach(var term in queryTerms)
            {
                var declaredTerm = declaredTerms
                    .SingleOrDefault(x => x.Name.Equals(term.Name, StringComparison.OrdinalIgnoreCase));

                if (declaredTerm == null) continue;

                yield return new SortTerm
                {
                    Name = declaredTerm.Name,
                    Descending = term.Descending
                };
            }
        }

        private static IEnumerable<SortTerm> GetTermFromModel()
        => typeof(T).GetTypeInfo()
                    .DeclaredProperties
                    .Where(p => p.GetCustomAttributes<SortableAttribute>().Any())
                    .Select(p => new SortTerm { Name = p.Name });
    }
}
