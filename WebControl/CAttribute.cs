using System;

namespace CommunityBuy.WebControl
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CAttribute : Attribute
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
