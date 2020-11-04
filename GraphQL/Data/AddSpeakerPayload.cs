
namespace GraphQL.Data
{
    public class AddSpeakerPayload
    {
        public Speaker Speaker { get; }

        public AddSpeakerPayload(Speaker speaker)
        {
            Speaker = speaker;
        }
    }
}