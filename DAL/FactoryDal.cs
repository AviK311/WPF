using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
	public class FactoryDal
	{
		static Idal dal = null;
		protected FactoryDal() { }
		public static Idal GetDAL()
		{
			if (dal == null)
				dal = new Dal_imp();
			return dal;
		}
	}
}
