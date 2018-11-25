using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
	public class FactoryBL
	{
		static IBL bl = null;
		protected FactoryBL() { }
		public static IBL GetBL()
		{
			if (bl==null)
				bl = new Bl_imp();
			return bl;
		}
	}
}
