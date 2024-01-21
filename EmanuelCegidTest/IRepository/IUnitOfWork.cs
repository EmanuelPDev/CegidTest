namespace APICatalogo.Repository
{
    public interface IUnitOfWork
    {
        ISalesItemsRepository SalesItemsRepository { get; }
        ICostomerRepository CostomerRepository { get; }
        Task Commit();
    }
}
