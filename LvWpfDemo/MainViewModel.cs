using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LvWpfDemo
{
    public class MainViewModel:INotifyPropertyChanged
    {
		private double signal=0.2;

		public double Signal
		{
			get { return signal; }
			set { signal = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Signal)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
