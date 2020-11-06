using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using GraphQL.Data;
using HotChocolate;
using GraphQL.Extensions;
using System.Threading;
using GraphQL.DataLoader;
using HotChocolate.Types.Relay;
using HotChocolate.Types;

namespace GraphQL.Speakers
{
    [ExtendObjectType(Name = "Query")]
    public class SpeakerQueries
    {
        [UseApplicationDbContext]
        public Task<List<Speaker>> GetSpeakers([ScopedService] ApplicationDbContext context) => context.Speakers.ToListAsync();
        // public IQueryable<Speaker> GetSpeakers([Service] ApplicationDbContext context) => context.Speakers;

        // Wherever we handle id values we need to annotate them with the ID attribute in order to tell the execution engine what kind of ID this is.
        // We also can do that in the fluent API by using the ID descriptor method a field or argument descriptor.
        public Task<Speaker> GetSpeakerAsync(
            [ID(nameof(Speaker))]int id,
            SpeakerByIdDataLoader dataLoader,
            CancellationToken cancellationToken
           ) => dataLoader.LoadAsync(id, cancellationToken);
    }
}