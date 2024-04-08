namespace Core.common;

public interface IGet<T1, T2> where T1 : class, T2 where T2 : class
{
    Task<List<T1>> GetAll(int? id = 0);
    Task<T2?> GetById(int id);
}