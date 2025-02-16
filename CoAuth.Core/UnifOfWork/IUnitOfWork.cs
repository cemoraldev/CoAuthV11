namespace CoAuth.Core.UnifOfWork;

public interface IUnitOfWork
{
    //Asenkron metodlar için
    Task CommitAsync();

    //Asenkron olmayan metodlar için
    void Commit();

}