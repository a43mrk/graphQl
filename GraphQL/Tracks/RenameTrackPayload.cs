using System.Collections.Generic;
using GraphQL.Data;
using GraphQL.Common;

namespace GraphQL.Tracks
{
    public class RenameTrackPayload : TrackPayloadBase
    {
        public RenameTrackPayload(Track track) : base(track) { }
        public RenameTrackPayload(IReadOnlyList<UserError> errors) : base(errors) { }
    }
}