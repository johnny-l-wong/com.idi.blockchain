using System;
using IDI.BlockChain.Common.Enums;
using Newtonsoft.Json;

namespace IDI.BlockChain.Models.Transaction
{
    public class Group
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("range")]
        public KLineRange Range { get; set; }

        public string Name => $"{Symbol}/{Range}";

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(Group))
                return false;

            Group group = obj as Group;

            return this.Name.Equals(group.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
