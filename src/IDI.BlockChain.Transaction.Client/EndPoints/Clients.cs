using System.Collections.Generic;
using System.Linq;
using IDI.BlockChain.Models.Transaction;
using IDI.Core.Infrastructure;
using IDI.Core.Logging;
using IDI.Core.Utils;

namespace IDI.BlockChain.Transaction.Client.EndPoints
{
    public sealed class Clients : Singleton<Clients>
    {
        private Dictionary<Group, List<string>> groups;
        private ILogger logger;

        public List<Group> Groups
        {
            get
            {
                return groups.Select(kvp => kvp.Key).ToList();
            }
        }

        private Clients()
        {
            groups = new Dictionary<Group, List<string>>();
            logger = Runtime.GetService<ILogger>();
        }

        public List<string> Connections(Group group) => groups.ContainsKey(group) ? groups[group] : new List<string>();

        public void Join(Group group, string connectionId)
        {
            Remove(connectionId);

            if (groups.ContainsKey(group) && !groups[group].Contains(connectionId))
            {
                groups[group].Add(connectionId);
            }

            if (!groups.ContainsKey(group))
            {
                groups.Add(group, new List<string> { connectionId });
            }

            logger.Info($"Clients.Join|{group.Name}|{connectionId}");
        }

        public void Remove(string connectionId)
        {
            foreach (var kvp in groups)
            {
                if (kvp.Value.Contains(connectionId))
                {
                    kvp.Value.Remove(connectionId);
                    logger.Info($"Clients.Remove|{kvp.Key.Name}|{connectionId}");
                    break;
                }
            }

        }
    }
}
