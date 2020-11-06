using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GraphQL.Data;
using GraphQL.DataLoader;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Resolvers;

namespace GraphQL.Types
{
    public class TrackType : ObjectType<Track>
    {
        protected override void Configure(IObjectTypeDescriptor<Track> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField( t => t.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<TrackByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(t => t.Sessions)
                .ResolveWith<TrackResolvers>(t => t.GetSessionsAsync(default!, default!, default!, default))
                .Name("sessions");
        }
        
        private class TrackResolvers {
            public async Task<IEnumerable<Session>> GetSessionsAsync(
                Track track,
                [ScopedService] ApplicationDbContext dbContext,
                SessionByIdDataLoader sessionById,
                CancellationToken cancellationToken
            )
            {
                int[] sessionIds = await dbContext.Sessions
                    .Where(s => s.Id == track.Id)
                    .Select(s => s.Id)
                    .ToArrayAsync();

                return await sessionById.LoadAsync(sessionIds, cancellationToken);
            }
        }
    }
}