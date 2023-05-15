using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormLayout.Object
{
    public enum ContentType
    {
        Web = 1,
        Image = 2,
        Video = 3
    }
    public class ContentObject
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public ContentType Type { get; set; }
    }
}
