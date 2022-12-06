using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfPractice.AppConnection
{
    public partial class Products
    {
        public string CorrectImage
        {
            get
            {
                if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\ProductImages\\" + ProductImage))
                {
                    return "/ProductImages/" + ProductImage;
                }
                else
                {
                    return "/ProductImages/DefaultPicture.png";
                }
            }
        }
        public Brush Count
        {
            get
            {
                if (InStock <= 0)
                {
                    return Brushes.Gray;
                }
                else
                {
                    return Brushes.Transparent;
                }
            }
        }
    }
}
