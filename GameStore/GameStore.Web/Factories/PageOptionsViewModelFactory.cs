using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.Models;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Helpers.PathHelpers;
using GameStore.Web.Models;
using GameStore.Web.Models.ViewModels.FilterViewModels;
using GameStore.Web.Models.ViewModels.PageViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace GameStore.Web.Factories
{
    public class PageOptionsViewModelFactory : IViewModelFactory<PageOptions, PageOptionsViewModel>
    {
        private const string PageNumberName = nameof(FilterSelectedOptionsViewModel.PageNumber);
        private const int MaxPagesCount = 10;
        private const int DefaultPage = 1;

        private readonly IMapper _mapper;
        private readonly HttpRequest _request;

        public PageOptionsViewModelFactory(IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _request = httpContextAccessor.HttpContext.Request;
        }

        public PageOptionsViewModel Create(PageOptions model)
        {
            var hasPrevious = model.PageNumber != DefaultPage;
            var totalPages = ComputeTotalPages(model.TotalItems, model.PageSize);
            var hasNext = totalPages > model.PageNumber;
            var endPage = model.PageNumber + MaxPagesCount > totalPages
                ? totalPages
                : model.PageNumber + MaxPagesCount;

            var viewModel = _mapper.Map<PageOptionsViewModel>(model);
            viewModel.HasPrevious = hasPrevious;
            viewModel.HasNext = hasNext;
            viewModel.EndPage = endPage;
            viewModel.TotalPages = totalPages;

            AddPageLinks(viewModel);

            return viewModel;
        }

        private static int ComputeTotalPages(int itemsCount, string pageSize)
        {
            if (!int.TryParse(pageSize, out var itemsPerPage))
            {
                return default;
            }

            var totalPages = (int) Math.Ceiling((double) itemsCount / itemsPerPage);

            return totalPages;
        }

        private void AddPageLinks(PageOptionsViewModel viewModel)
        {
            for (var i = viewModel.PageNumber; i <= viewModel.EndPage; i++)
            {
                var pageLink = CreatePageLink(i, viewModel.PageNumber);

                viewModel.Pages.Add(pageLink);
            }

            if (viewModel.HasPrevious)
            {
                viewModel.PreviousPage = CreatePageLink(viewModel.PageNumber - 1, viewModel.PageNumber);
            }

            if (!viewModel.HasNext)
            {
                return;
            }

            viewModel.NextPage = CreatePageLink(viewModel.PageNumber + 1, viewModel.PageNumber);
        }

        private LinkItem CreatePageLink(int value, int currentPage)
        {
            var pageNumber = value.ToString();
            var pageLink = new LinkItem
            {
                Name = pageNumber,
                Value = pageNumber,
                Link = CreatePageNumberPath(pageNumber),
                Selected = value == currentPage
            };

            return pageLink;
        }

        private string CreatePageNumberPath(string value)
        {
            var queryCollection = QueryHelpers.ParseQuery(_request.QueryString.ToString())
                .Where(collection => collection.Key != PageNumberName);

            var path = FilterPathHelper.Insert(
                PageNumberName,
                value,
                FilterPathHelper.Path,
                queryCollection.AsEnumerable());
            
            return path;
        }
    }
}