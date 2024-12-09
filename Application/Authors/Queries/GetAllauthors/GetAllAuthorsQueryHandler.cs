using Database.Databases;
using Domain.Models;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.Queries.GetAllauthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, OperationResult<List<Author>>>
    {
        private readonly IGenericRepository<Author> _genericRepository;
        private readonly IMemoryCache _memoryCache;
        private const string cacheKey = "allAuthors";

        public GetAllAuthorsQueryHandler(IGenericRepository<Author> genericRepository, IMemoryCache memoryCache)
        {
            _genericRepository = genericRepository;
            _memoryCache = memoryCache;
        }

        public async Task<OperationResult<List<Author>>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out List<Author> allAuthors))
            {
                allAuthors = (await _genericRepository.GetAllAsync()).ToList();
                _memoryCache.Set(cacheKey, allAuthors, TimeSpan.FromMinutes(5));
                if(allAuthors.Count == 0 || allAuthors == null)
                {
                    return OperationResult<List<Author>>.FailureResult("No authors found");
                }
            }
            
            return OperationResult<List<Author>>.SuccessResult(allAuthors);
        }
    }
}
