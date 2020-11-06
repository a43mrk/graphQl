using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using GraphQL.Common;
using GraphQL.Data;
using GraphQL.Extensions;

namespace GraphQL.Sessions
{
    [ExtendObjectType( Name = "Mutation")]
    public class SessionMutations
    {
        
        [UseApplicationDbContext]
        public async Task<AddSessionPayload> AddSessionAsync(
            AddSessionInput input,
            [ScopedService] ApplicationDbContext dbContext,
            CancellationToken cancellationToken
        )
        {
            if(string.IsNullOrEmpty(input.Title))
            {
                return new AddSessionPayload(new UserError("The title cannot be empty.", "TITLE_EMPTY"));
            }

            if(input.SpeakerIds.Count == 0)
            {
                return new AddSessionPayload( new UserError("No speaker assigned.", "NO_SPEAKER"));
            }

            var session = new Session {
                Title = input.Title,
                Abstract = input.Abstract
            };

            foreach( int speakerId in input.SpeakerIds)
            {
                session.SessionSpeakers.Add(new SessionSpeaker {
                    SpeakerId = speakerId
                });
            }

            dbContext.Sessions.Add(session);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new AddSessionPayload(session);
        }

        [UseApplicationDbContext]
        public async Task<ScheduleSessionPayload> ScheduleSessionAsync(
            ScheduleSessionInput input,
            [ScopedService] ApplicationDbContext context
        )
        {
            if(input.EndTime < input.StartTime)
            {
                return new ScheduleSessionPayload(new UserError("endTime has to be larger than startTime.", "END_TIME_INVALID"));
            }

            Session session = await context.Sessions.FindAsync(input.SessionId);
            int? initialTrackId = session.TrackId;

            if( session is null){
                return new ScheduleSessionPayload( new UserError("Session not found.", "SESSION_NOT_FOUND"));
            }

            session.TrackId = input.TrackId;
            session.StartTime = input.StartTime;
            session.EndTime = input.EndTime;

            await context.SaveChangesAsync();
            
            return new ScheduleSessionPayload(session);
        }
    }
}