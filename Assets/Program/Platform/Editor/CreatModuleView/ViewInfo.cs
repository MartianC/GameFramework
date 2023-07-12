using System.Collections.Generic;

namespace Platform
{
    public class ViewInfo
    {
        public string Name { get; }
        public List<string> Components { get; } = new List<string>();

        public ViewInfo(string name)
        {
            Name = name;
        }
    }
}