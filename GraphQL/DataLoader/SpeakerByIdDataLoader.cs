using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using GreenDonut;
using HotChocolate.DataLoader;
using GraphQL.Data;

namespace GraphQL.DataLoader
{
    public class SpeakerByIdDataLoader : BatchDataLoader<int, Speaker>
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public SpeakerByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<ApplicationDbContext> dbContextFactory
        ): base(batchScheduler) {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        } 

        protected override async Task<IReadOnlyDictionary<int, Speaker>> LoadBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken){
            await using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

            return await dbContext.Speakers
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync( t => t.Id, cancellationToken);
        }
    }
}