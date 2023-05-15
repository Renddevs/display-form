using display.Properties;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormLayout.Object;

namespace display
{
    public partial class Form1 : Form
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        private Size _formOriginalSize;
        IConfiguration _configuration;
        private List<PanelRecObject> panelRec;
        List<ConfLocSizeObject> _confLocSizeObject;
        private int _IdConfLayout;


        public Form1()
        {
            panelRec = new List<PanelRecObject>();
            #region Get Set Config
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
            _confLocSizeObject = _configuration.GetSection("ConfLocSize").Get<List<ConfLocSizeObject>>();
            _IdConfLayout = _configuration.GetSection("IdConfLayout").Get<int>();
            #endregion
            InitializeComponent();
            _formOriginalSize = this.Size;
            List<ContentObject> data = GetContent();
            SetContent(data);
            this.Resize += form_content_resize;
        }

        private void SetContent(List<ContentObject> data)
        {
            foreach (var item in data.Select((value, i) => new { i, value }))
            {
                var dataConf = _confLocSizeObject.Where(d => d.id == _IdConfLayout).FirstOrDefault().layoutConf[item.i];

                if (item.value.Type == ContentType.Web)
                {
                    SetContentWeb(item.value, dataConf);
                }
                else if (item.value.Type == ContentType.Image)
                {
                    #region Set Content Image
                    var img = new PictureBox();
                    this.Controls.Add(img);
                    img.ImageLocation = item.value.Source;
                    img.Name = item.value.Name;
                    img.Location = new Point(dataConf.Location.x, dataConf.Location.y);
                    img.Name = "Pic" + (item.i + 1);
                    img.Size = new Size(dataConf.Size.width, dataConf.Size.height);
                    img.SizeMode = PictureBoxSizeMode.StretchImage;
                    img.TabIndex = 0;
                    SuspendLayout();

                    panelRec.Add(new PanelRecObject()
                    {
                        control = img,
                        rec = new Rectangle(img.Location, img.Size)
                    });
                    #endregion
                }
                else if (item.value.Type == ContentType.Video)
                {
                    #region Set Content Video
                    var video = new AxWMPLib.AxWindowsMediaPlayer();
                    ((System.ComponentModel.ISupportInitialize)(video)).BeginInit();
                    video.Name = item.value.Name;
                    video.Location = new Point(dataConf.Location.x, dataConf.Location.y);
                    video.Size = new Size(dataConf.Size.width, dataConf.Size.height);
                    this.Controls.Add(video);
                    ((System.ComponentModel.ISupportInitialize)(video)).EndInit();
                    video.stretchToFit = true;
                    video.uiMode = "none";
                    video.URL = item.value.Source;
                    SuspendLayout();

                    panelRec.Add(new PanelRecObject()
                    {
                        control = video,
                        rec = new Rectangle(video.Location, video.Size)
                    });
                    #endregion
                }
            }
        }

        #region Resize Control Method
        private void form_content_resize(object sender, EventArgs e)
        {
            foreach (var item in panelRec)
            {
                resize_Control(item.control, item.rec);
            }
        }

        private void resize_Control(Control c, Rectangle r)
        {
            float xRatio = (float)(this.Width) / (float)(_formOriginalSize.Width);
            float yRatio = (float)(this.Height) / (float)(_formOriginalSize.Height);
            int newX = (int)(r.X * xRatio);
            int newY = (int)(r.Y * yRatio);

            int newWidth = (int)(r.Width * xRatio);
            int newHeight = (int)(r.Height * yRatio);

            c.Location = new Point(newX, newY);
            c.Size = new Size(newWidth, newHeight);
        }
        #endregion

        #region Get Set Data Content
        public List<ContentObject> GetContent()
        {
            var data = new List<ContentObject>();
            
            data.Add(new ContentObject()
            {
                Name = "Pict Zebra 1",
                Type = ContentType.Web,
                Source = "https://www.youtube.com/watch?v=qpi9YXaChHI"
            });
            data.Add(new ContentObject()
            {
                Name = "Pict Zebra 2",
                Type = ContentType.Image,
                Source = "C:\\Users\\rendd\\OneDrive\\Pictures\\Screenshot 2023-02-27 102026.png"
            });
            //data.Add(new ContentObject()
            //{
            //    Name = "Pict Zebra 3",
            //    Type = ContentType.Image,
            //    Source = "C:\\Users\\MIT\\Pictures\\hari-zebra-internasional-2023_169.jpg"
            //});
            //data.Add(new ContentObject()
            //{
            //    Name = "Video Zebra",
            //    Type = ContentType.Video,
            //    Source = "C:\\Users\\MIT\\Videos\\Zebra - Africa's Wild Wonders - Go Wild.mp4"
            //});
            return data;
        }
        #endregion

        #region Set Content Web
        public async Task SetContentWeb(ContentObject value, LayoutObject dataConf)
        {
            var web = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)(web)).BeginInit();
            web.SuspendLayout();
            web.AllowExternalDrop = true;
            web.CreationProperties = null;
            web.DefaultBackgroundColor = System.Drawing.Color.White;
            web.Location = new System.Drawing.Point(dataConf.Location.x, dataConf.Location.y);
            web.Name = value.Name;
            web.Size = new System.Drawing.Size(dataConf.Size.width, dataConf.Size.height);
            web.TabIndex = 0;
            web.ZoomFactor = 1D;
            this.Controls.Add(web);
            ((System.ComponentModel.ISupportInitialize)(web)).EndInit();
            this.ResumeLayout(false);
            await web.EnsureCoreWebView2Async(null);
            web.CoreWebView2.Navigate(value.Source);

            panelRec.Add(new PanelRecObject()
            {
                control = web,
                rec = new Rectangle(web.Location, web.Size)
            });

        }
        #endregion
    }
}
