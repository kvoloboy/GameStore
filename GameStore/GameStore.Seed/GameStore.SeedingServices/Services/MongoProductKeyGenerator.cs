using System.Collections.Generic;
using System.Linq;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using GameStore.SeedingServices.Models;
using GameStore.SeedingServices.Repositories.Interfaces;
using GameStore.SeedingServices.Services.Interfaces;

namespace GameStore.SeedingServices.Services
{
    public class MongoProductKeyGenerator : IMongoProductKeyGenerator
    {
        private const string KeySeparator = ".";

        private readonly Core.Abstractions.IRepository<GameRoot> _gameRootRepository;
        private readonly Core.Abstractions.IRepository<Genre> _sqlGenreRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGameService _gameService;
        private readonly IProductRepository _mongoProductRepository;
        private readonly Repositories.Interfaces.IRepository<Genre> _mongoGenreRepository;

        public MongoProductKeyGenerator(
            IUnitOfWork unitOfWork,
            IGameService gameService,
            IProductRepository mongoProductRepository,
            Repositories.Interfaces.IRepository<Genre> mongoGenreRepository)
        {
            _unitOfWork = unitOfWork;
            _gameService = gameService;
            _mongoGenreRepository = mongoGenreRepository;
            _mongoProductRepository = mongoProductRepository;
            _gameRootRepository = _unitOfWork.GetRepository<Core.Abstractions.IRepository<GameRoot>>();
            _sqlGenreRepository = _unitOfWork.GetRepository<Core.Abstractions.IRepository<Genre>>();
        }

        public void SetupKeys()
        {
            var products = _mongoProductRepository.GetAll();
            var mongoGenres = _mongoGenreRepository.GetAll();
            var sqlGenres = mongoGenres.Select(g => new Genre {Name = g.Name}).ToList();

            SetupGenres(sqlGenres);

            foreach (var product in products)
            {
                var key = _gameService.GenerateKey(product.ProductName, KeySeparator);
                var root = CreateGameRoot(key, product, mongoGenres, sqlGenres);

                _gameRootRepository.Add(root);
                _mongoProductRepository.SetKey(product.Id, key);
            }

            _unitOfWork.Commit();
        }

        private void SetupGenres(IEnumerable<Genre> genres)
        {
            foreach (var genre in genres)
            {
                _sqlGenreRepository.Add(genre);
            }
        }

        private static GameRoot CreateGameRoot(
            string key,
            Product product,
            IEnumerable<Genre> mongoGenres,
            IEnumerable<Genre> sqlGenres)
        {
            var genreId = GetSqlGenreId(product.CategoryId, mongoGenres, sqlGenres);
            var root = new GameRoot {Key = key};

            if (genreId != null)
            {
                root.GameGenres = new[]
                {
                    new GameGenre {GenreId = genreId}
                };
            }

            return root;
        }

        private static string GetSqlGenreId(
            int categoryId,
            IEnumerable<Genre> mongoGenres,
            IEnumerable<Genre> sqlGenres)
        {
            var genre = mongoGenres.FirstOrDefault(g => g.Id == categoryId.ToString());

            if (genre == null)
            {
                return null;
            }

            var sqlId = sqlGenres.First(g => g.Name == genre.Name).Id;

            return sqlId;
        }
    }
}