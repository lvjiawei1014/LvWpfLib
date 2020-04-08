using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ncer.UI
{
    /// <summary>
    /// ColormapBar.xaml 的交互逻辑
    /// </summary>
    public partial class ColormapBar : UserControl
    {
        private ColormapType colormapType;
        public ColormapType ColormapType
        {
            get => colormapType; set
            {
                colormapType = value;
                OnTypeSet(value);
            }
        }
        public ColormapBar()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        public void OnTypeSet(ColormapType colormapType)
        {
            switch (colormapType)
            {
                case ColormapType.Gray:
                    this.gridMain.Background = (Brush)this.Resources["colormapGray"];

                    break;
                case ColormapType.Jet:
                    this.gridMain.Background = (Brush)this.Resources["colormapJet"];

                    break;
                default:
                    break;
            }
        }





    }
    public enum ColormapType
    {
        Gray = 0,
        Jet = 1,
    }
}
