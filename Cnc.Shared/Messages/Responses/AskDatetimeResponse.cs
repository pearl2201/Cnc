using System.Collections.Generic;
using System;

namespace Cnc.Shared.Messages.Responses
{


    public class AskDatetimeResponse : IResponse
    {

        public DateTime CurrentDatetime { get; set; }

    }
}