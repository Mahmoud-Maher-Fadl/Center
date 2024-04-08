namespace Core.common;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> Add(T entity);
    Task<T> Update(T entity);
    Task<int> UpdateRange(List<T> entities);
    Task<T> DeleteById(int id);
    Task<int> DeleteRange(List<T> entities);
}