using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using GraphQL.Data;
using GraphQL.DataLoader;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace GraphQL.Types
{
    public class AttendeeType : ObjectType<Attendee>
    {
        protected override void Configure(IObjectTypeDescriptor<Attendee> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField( t => t.Id )
                .ResolveNode((ctx, id) => ctx.DataLoader<AttendeeByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field( t => t.SessionAttendees )
                .ResolveWith<AttendeeResolvers>( t => t.GetSessionsAsync(default!, default!, default!, default))
                .Name("sessions");
        }

        private class AttendeeResolvers
        {
            public async Task<IEnumerable<Session>> GetSessionsAsync(
                Attendee attendee,
                [ScopedService] ApplicationDbContext dbContext,
                SessionByIdDataLoader sessionById,
                CancellationToken cancellationToken
            )
            {
                int[] speakerIds = await dbContext.Attendees
                    .Where(a => a.Id == attendee.Id)
                    .Include(a => a.SessionAttendees)
                    .SelectMany( a => a.SessionAttendees.Select( t => t.SessionId))
                    .ToArrayAsync();

                return await sessionById.LoadAsync(speakerIds, cancellationToken);
            }
        }
    }
}