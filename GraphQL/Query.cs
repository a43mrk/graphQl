using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using GraphQL.Data;
using HotChocolate;
using GraphQL.Extensions;
using System.Threading;
using GraphQL.DataLoader;

namespace GraphQL
{
    public class Query
    {
        [UseApplicationDbContext]
        public Task<List<Speaker>> GetSpeakers([ScopedService] ApplicationDbContext context) => context.Speakers.ToListAsync();
        // public IQueryable<Speaker> GetSpeakers([Service] ApplicationDbContext context) => context.Speakers;

        public Task<Speaker> GetSpeakerAsync(int id, SpeakerByIdDataLoader dataLoader, CancellationToken cancellationToken)
            => dataLoader.LoadAsync(id, cancellationToken);
    }
}