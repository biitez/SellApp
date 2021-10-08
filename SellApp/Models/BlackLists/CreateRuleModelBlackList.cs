using SellApp.Enums;

namespace SellApp.Models.BlackLists
{
    public class CreateRuleModelBlackList
    {
        public BlackListTypes RuleType { get; set; }

        /// <summary>
        /// Depending on the type you chose, you can enter an IP address, email address, or country code here.
        /// </summary>
        public string RuleValue { get; set; }

        /// <summary>
        /// A description that will help you remember why this blacklist rule was created.
        /// </summary>
        public string RuleDescription { get; set; }
    }
}
