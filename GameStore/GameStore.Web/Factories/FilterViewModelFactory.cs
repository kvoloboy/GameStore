using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Helpers.PathHelpers;
using GameStore.Web.Models;
using GameStore.Web.Models.ViewModels.FilterViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;

namespace GameStore.Web.Factories
{
    public class FilterViewModelFactory : IAsyncViewModelFactory<FilterSelectedOptionsViewModel, FilterViewModel>
    {
        private const string PageSizeName = nameof(FilterSelectedOptionsViewModel.PageSize);
        private const string SortQueryName = nameof(FilterSelectedOptionsViewModel.SortOption);
        private const string PageNumberName = nameof(FilterSelectedOptionsViewModel.PageNumber);

        private readonly IGenreService _genreService;
        private readonly IPlatformService _platformService;
        private readonly IPublisherService _publisherService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<FilterViewModelFactory> _stringLocalizer;
        private readonly HttpRequest _httpRequest;

        public FilterViewModelFactory(
            IGenreService genreService,
            IPlatformService platformService,
            IPublisherService publisherService,
            IMapper mapper,
            IStringLocalizer<FilterViewModelFactory> stringLocalizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _genreService = genreService;
            _platformService = platformService;
            _publisherService = publisherService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _httpRequest = httpContextAccessor.HttpContext.Request;
        }

        public async Task<FilterViewModel> CreateAsync(FilterSelectedOptionsViewModel model)
        {
            var selectionDataViewModel = new FilterSelectionDataViewModel
            {
                Genres = await SetupGenresAsync(model.Genres),
                Platforms = await SetupPlatformsAsync(model.Platforms),
                Publishers = await SetupPublishersAsync(model.Publishers),
                SortOptions = SetupSortOptions(model.SortOption),
                PublishingDates = SetupCreationDate(model.CreationDate),
                PageSizes = SetupPageSizes(model.PageSize),
            };
            var viewModel = new FilterViewModel
            {
                FilterSelectionDataViewModel = selectionDataViewModel,
                FilterSelectedOptionsViewModel = model
            };

            return viewModel;
        }

        private async Task<IEnumerable<ListItem>> SetupGenresAsync(IEnumerable<string> selectedGenres)
        {
            var genres = await _genreService.GetAllAsync();
            var listItems = _mapper.Map<IEnumerable<ListItem>>(genres);

            foreach (var listItem in listItems)
            {
                listItem.Selected = IsCheckedListItem(selectedGenres, listItem);
            }

            return listItems;
        }

        private async Task<IEnumerable<ListItem>> SetupPlatformsAsync(IEnumerable<string> selectedPlatforms)
        {
            var platforms = await _platformService.GetAllAsync();
            var listItems = _mapper.Map<IEnumerable<ListItem>>(platforms);

            foreach (var listItem in listItems)
            {
                listItem.Selected = IsCheckedListItem(selectedPlatforms, listItem);
            }

            return listItems;
        }

        private async Task<IEnumerable<ListItem>> SetupPublishersAsync(IEnumerable<string> selectedPublishers)
        {
            var publishers = await _publisherService.GetAllAsync(Culture.Current);
            var listItems = _mapper.Map<IEnumerable<ListItem>>(publishers);

            foreach (var listItem in listItems)
            {
                listItem.Selected = IsCheckedListItem(selectedPublishers, listItem);
            }

            return listItems;
        }

        private IEnumerable<ListItem> SetupCreationDate(string selectedDate)
        {
            var creationDates = GetFieldValues(typeof(CreationTerm));
            var localizedDates = creationDates.Select(date => _stringLocalizer[date]).ToArray();

            var listItems = creationDates.Select((date, i) => new ListItem
            {
                Id = date,
                Name = localizedDates[i],
                Selected = date == selectedDate || localizedDates[i] == selectedDate
            });

            return listItems;
        }

        private IEnumerable<LinkItem> SetupSortOptions(string selectedSort)
        {
            var sortOptions = GetFieldValues(typeof(SortOptions));
            var localizedOptions = sortOptions.Select(option => _stringLocalizer[option]).ToArray();
            var listItems = localizedOptions.Select((so, i) =>
                new LinkItem
                {
                    Name = so,
                    Value = sortOptions[i],
                    Link = CreateSortPath(sortOptions[i]),
                    Selected = so == selectedSort || sortOptions[i] == selectedSort
                }).ToList();

            return listItems;
        }

        private static string[] GetFieldValues(IReflect targetType)
        {
            var fields = targetType.GetFields(BindingFlags.Public | BindingFlags.Static);
            var constants = fields.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
            var values = constants.Select(f => f.GetValue(null).ToString()).ToArray();

            return values;
        }

        private IEnumerable<LinkItem> SetupPageSizes(string selectedSize)
        {
            var pageSizes = GetFieldValues(typeof(PageSize));
            var listItems = pageSizes.Select(ps =>
                new LinkItem
                {
                    Name = ps,
                    Value = ps,
                    Link = CreatePageSizePath(ps),
                    Selected = ps == selectedSize
                }).ToList();

            return listItems;
        }

        private static bool IsCheckedListItem(IEnumerable<string> selectedItems, ListItem currentItem)
        {
            var isChecked = selectedItems != null && selectedItems.Contains(currentItem.Id);

            return isChecked;
        }

        private string CreateSortPath(string value)
        {
            var queryCollection = QueryHelpers.ParseQuery(_httpRequest.QueryString.ToString())
                .Where(collection => collection.Key != PageNumberName && collection.Key != SortQueryName);

            var path = FilterPathHelper.Insert(SortQueryName, value, FilterPathHelper.Path,
                queryCollection.AsEnumerable());

            return path;
        }

        private string CreatePageSizePath(string value)
        {
            var queryCollection = QueryHelpers.ParseQuery(_httpRequest.QueryString.ToString())
                .Where(collection => collection.Key != PageNumberName && collection.Key != PageSizeName);

            var path = FilterPathHelper.Insert(PageSizeName, value, FilterPathHelper.Path, queryCollection);

            return path;
        }
    }
}