using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncer.UI
{
    public class NumberPadViewModel:BindableBase
    {
		private double numberValue;

		public double NumberValue
		{
			get { return numberValue; }
			set { numberValue = value;  OnPropertyChanged(); }
		}

		public void Reset(double def)
		{
			this.NumberValue = def;
		}


	}
}
