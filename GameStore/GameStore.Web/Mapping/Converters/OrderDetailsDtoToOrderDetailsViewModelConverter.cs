using System.Linq;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.ImageViewModels;
using GameStore.Web.Models.ViewModels.OrderViewModels;

namespace GameStore.Web.Mapping.Converters
{
    public class OrderDetailsDtoToOrderDetailsViewModelConverter : ITypeConverter<OrderDetailsDto, OrderDetailsViewModel>
    {
        private readonly IViewModelFactory<string, GameImageViewModel> _gameImageViewModelFactory;

        public OrderDetailsDtoToOrderDetailsViewModelConverter(
            IViewModelFactory<string, GameImageViewModel> gameImageViewModelFactory)
        {
            _gameImageViewModelFactory = gameImageViewModelFactory;
        }

        public OrderDetailsViewModel Convert(OrderDetailsDto source, OrderDetailsViewModel destination,
            ResolutionContext context)
        {
            var viewModel = new OrderDetailsViewModel
            {
                Id = source.Id,
                GameName = source.Game.Name,
                Discount = source.Discount,
                Price = source.Price,
                Quantity = source.Quantity
            };

            var imageContent = source.Game.Images.Any()
                ? context.Mapper.Map<GameImageViewModel>(source.Game.Images.First()).Content
                : _gameImageViewModelFactory.Create(source.Game.Key).Content;

            viewModel.GameImage = imageContent;

            return viewModel;
        }
    }
}