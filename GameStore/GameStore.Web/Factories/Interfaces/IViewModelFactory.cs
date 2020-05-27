namespace GameStore.Web.Factories.Interfaces
{
    public interface IViewModelFactory<in TModel, out TViewModel>
    {
        TViewModel Create(TModel model);
    }
}