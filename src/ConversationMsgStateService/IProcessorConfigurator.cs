using System;
using System.Collections.Generic;
using System.Text;

namespace ConversationMsgStateService
{
    public interface IProcessorConfigurator
    {
        bool IsArchiveGroupProcessTime();
    }

    public class DefaultProcessorConfigurator : IProcessorConfigurator
    {
        public bool IsArchiveGroupProcessTime()
        {
            var hour = DateTimeOffset.Now.Hour;
            return hour >= 0 && hour < 6;
        }
    }
}
