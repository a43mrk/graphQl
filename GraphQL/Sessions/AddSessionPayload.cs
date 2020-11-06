using GraphQL.Common;
using GraphQL.Data;

namespace GraphQL.Sessions
{
    public class AddSessionPayload : Payload
    {
        public Session? Session { get; }

        public AddSessionPayload(Session session)
        {
            Session = session;
        }

        public AddSessionPayload(UserError error): base(new [] {error}) {}
        
    }
}