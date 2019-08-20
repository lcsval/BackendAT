using System.Collections.Generic;

namespace Backend.Domain.Commands
{
    public abstract class BaseCommand
    {
        public BaseCommand()
        {
            Notifications = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Notifications { get; set; }
    }
}
