using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Decorators.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IMapper _mapper;
        private readonly IPublisherDecorator _publisherDecorator;
        private readonly IUnitOfWork _unitOfWork;

        public PublisherService(
            IUnitOfWork unitOfWork,
            IPublisherDecorator publisherDecorator,
            IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _publisherDecorator = publisherDecorator;
        }

        public async Task CreateAsync(ModifyPublisherDto publisherDto)
        {
            if (publisherDto == null)
            {
                throw new InvalidServiceOperationException("Is null publisher dto");
            }

            var publisher = _mapper.Map<Publisher>(publisherDto);
            await _publisherDecorator.AddAsync(publisher);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(ModifyPublisherDto publisherDto)
        {
            if (publisherDto == null)
            {
                throw new InvalidServiceOperationException("Is null publisher dto");
            }

            var exists = await _publisherDecorator.IsExistByIdAsync(publisherDto.Id);

            if (!exists)
            {
                throw new EntityNotFoundException<Publisher>(publisherDto.Id);
            }

            var publisher = _mapper.Map<Publisher>(publisherDto);
            await _publisherDecorator.UpdateAsync(publisher);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var exists = await _publisherDecorator.IsExistByIdAsync(id);

            if (!exists)
            {
                throw new EntityNotFoundException<Publisher>(id);
            }

            await _publisherDecorator.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<PublisherDto> GetByIdAsync(string id, string culture)
        {
            var existingPublisher = await _publisherDecorator.GetByIdAsync(id);

            if (existingPublisher == null)
            {
                throw new EntityNotFoundException<Publisher>(id);
            }

            var dto = CreatePublisherDto(existingPublisher, culture);

            return dto;
        }

        public async Task<ModifyPublisherDto> GetByIdAsync(string id)
        {
            var publisher = await _publisherDecorator.GetByIdAsync(id);
            var modifyPublisherDto = _mapper.Map<ModifyPublisherDto>(publisher);

            return modifyPublisherDto;
        }

        public async Task<PublisherDto> GetByUserIdAsync(string userId, string culture = Culture.En)
        {
            var publisher = await _publisherDecorator.GetByUserIdAsync(userId);

            if (publisher == null)
            {
                return null;
            }
            
            var dto = CreatePublisherDto(publisher, culture);

            return dto;
        }

        public async Task<IEnumerable<PublisherDto>> GetAllAsync(string culture = Culture.En)
        {
            var publishers = await _publisherDecorator.GetAllAsync();
            var  publishersDto = publishers.Select(publisher => CreatePublisherDto(publisher, culture));

            return publishersDto;
        }

        public async Task<PublisherDto> GetByCompanyAsync(string companyName, string culture = "en-US")
        {
            if (string.IsNullOrEmpty(companyName))
            {
                throw new InvalidServiceOperationException("Is empty company name");
            }

            var existingPublisher = await _publisherDecorator.GetByCompanyAsync(companyName);
            var dto = CreatePublisherDto(existingPublisher, culture);

            return dto;
        }

        private PublisherDto CreatePublisherDto(Publisher publisher, string culture)
        {
            var targetLocalization =
                publisher.Localizations?.FirstOrDefault(localization => localization.CultureName == culture) ??
                publisher.Localizations?.FirstOrDefault(localization => localization.CultureName == Culture.En);

            var dto = _mapper.Map<PublisherDto>(publisher);
            _mapper.Map(targetLocalization, dto);
            dto.LocalizationId = targetLocalization?.Id;
            dto.CultureName = targetLocalization?.CultureName;

            return dto;
        }
    }
}