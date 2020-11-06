using System.Collections.Generic;
using GraphQL.Common;
using GraphQL.Data;

namespace GraphQL.Speakers
{
    public class AddSpeakerPayload : SpeakerPayloadBase
    {
        // public Speaker Speaker { get; }

        public AddSpeakerPayload(Speaker speaker) : base(speaker)
        {
            // Speaker = speaker;
        }

        public AddSpeakerPayload(IReadOnlyList<UserError> errors) : base(errors)
        {
        }
    }
}