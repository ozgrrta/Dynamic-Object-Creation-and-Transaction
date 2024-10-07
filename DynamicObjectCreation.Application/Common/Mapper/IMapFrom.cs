namespace DynamicObjectCreation.Application.Common.Mapper
{
	public interface IMapFrom<T>
	{
		void Mapping(AutoMapper.Profile profile)
		{
			profile.CreateMap(typeof(T), GetType());
		}
	}
}
