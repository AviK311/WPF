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
