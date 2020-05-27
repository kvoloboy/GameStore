namespace GameStore.Web.WebServices.Interfaces
{
    public interface IVIewModelToDtoConverter<TViewModel, TDto>
    {
        TDto Convert(TViewModel viewModel);
    }
}