namespace Andrei15193.Edesia.DataAccess
{
	public interface ITranslator<TSource, TDestination>
	{
		TDestination Translate(TSource source);
	}
}