using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormLayout.Object
{
    public class LocationObject
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class SizeObject
    {
        public int width { get; set; }
        public int height { get; set; }
    }

    public class LayoutObject
    {
        public LocationObject Location { get; set; }
        public SizeObject Size { get; set; }
    }

    public class ConfLocSizeObject
    {
        public int id { get; set; }
        public List<LayoutObject> layoutConf { get; set; }
    }

    public class AppSettingsObject
    {
        public List<ConfLocSizeObject> ConfLocSize { get; set; }
        public int IdConfLayout { get; set; }
    }

    public class PanelRecObject
    {
        public Control control { get; set; }
        public Rectangle rec { get; set; }
    }






}
