using System.Collections.Generic;

namespace Cnc.Shared.Messages.Responses
{
    public enum Command
    {
        ASK_DATETIME
    }

    public class AskCommandResponse : IResponse
    {

        public Command Command { get; set; }

        public Dictionary<string, string> Metadata { get; set; }

    }
}