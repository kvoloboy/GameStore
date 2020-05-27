using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services;
using GameStore.Core.Abstractions;
using GameStore.Core.Models.Identity;
using NUnit.Framework;

namespace GameStore.BusinessLayer.Tests
{
    [TestFixture]
    public class RoleServiceTests
    {
        private const string Id = "1";

        private IUnitOfWork _unitOfWork;
        private IAsyncRepository<Role> _roleRepository;
        private RoleService _roleService;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _roleRepository = A.Fake<IAsyncRepository<Role>>();
            A.CallTo(() => _unitOfWork.GetRepository<IAsyncRepository<Role>>()).Returns(_roleRepository);

            _roleService = new RoleService(_unitOfWork);
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenNullDto()
        {
            Func<Task> action = async () => await  _roleService.CreateAsync(null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenExistsWithSameName()
        {
            var roleDto = CreateRoleDto();
            A.CallTo(() => _roleRepository.AnyAsync(A<Expression<Func<Role, bool>>>._)).Returns(true);

            Func<Task> action = async () => await _roleService.CreateAsync(roleDto);

            await action.Should().ThrowAsync<EntityExistsWithKeyValueException<Role>>();
        }

        [Test]
        public void CreateAsync_SavesToRepository_WhenValidDto()
        {
            var roleDto = CreateRoleDto();

            _roleService.CreateAsync(roleDto);

            A.CallTo(() => _roleRepository.AddAsync(A<Role>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenNullDto()
        {
            Func<Task> action = async () => await  _roleService.UpdateAsync(null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenRoleNotFound()
        {
            var roleDto = CreateRoleDto();
            A.CallTo(() => _roleRepository.FindSingleAsync(A<Expression<Func<Role, bool>>>._)).Returns((Role) null);

            Func<Task> action = async () => await  _roleService.UpdateAsync(roleDto);

            await action.Should().ThrowAsync<EntityNotFoundException<Role>>();
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenChangedNameAndNameExists()
        {
            var roleDto = CreateRoleDto();
            var existingRole = CreateRole();
            A.CallTo(() => _roleRepository.FindSingleAsync(A<Expression<Func<Role, bool>>>._)).Returns(existingRole);
            A.CallTo(() => _roleRepository.AnyAsync(A<Expression<Func<Role, bool>>>._)).Returns(true);

            Func<Task> action = async () => await _roleService.UpdateAsync(roleDto);

            await action.Should().ThrowAsync<EntityExistsWithKeyValueException<Role>>();
        }

        [Test]
        public void UpdateAsync_CallsRepository_WhenValidDto()
        {
            var roleDto = CreateRoleDto();
            var existingRole = CreateRole();
            A.CallTo(() => _roleRepository.FindSingleAsync(A<Expression<Func<Role, bool>>>._)).Returns(existingRole);

            _roleService.UpdateAsync(roleDto);

            A.CallTo(() => _roleRepository.UpdateAsync(A<Role>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task DeleteAsync_ThrowsException_WhenNotFoundRole()
        {
            Func<Task> action = async () => await  _roleService.DeleteAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<Role>>();
        }

        [Test]
        public void DeleteAsync_CallsRepository_WhenRoleExists()
        {
            A.CallTo(() => _roleRepository.AnyAsync(A<Expression<Func<Role, bool>>>._)).Returns(true);

            _roleService.DeleteAsync(Id);

            A.CallTo(() => _roleRepository.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetByIdAsync_CallsRepository_Always()
        {
            _roleService.GetByIdAsync(Id);

            A.CallTo(() => _roleRepository.FindSingleAsync(A<Expression<Func<Role, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task GetByNameAsync_ThrowsException_WhenEmptyName()
        {
            Func<Task> action = async () => await  _roleService.GetByNameAsync(string.Empty);

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public void GetByNameAsync_CallsRepository_WhenValidName()
        {
            _roleService.GetByNameAsync(DefaultRoles.Guest);

            A.CallTo(() => _roleRepository.FindSingleAsync(A<Expression<Func<Role, bool>>>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task GetByNamesAsync_ThrowsException_WhenEmptyNames()
        {
            Func<Task> action = async () => await  _roleService.GetByNamesAsync(Enumerable.Empty<string>());

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public void GetByNamesAsync_CallsRepository_WhenValidName()
        {
            var names = new[] {DefaultRoles.Guest};

            _roleService.GetByNamesAsync(names);

            A.CallTo(() => _roleRepository.FindAllAsync(A<Expression<Func<Role, bool>>>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetAllAsync_CallsRepository_Always()
        {
            _roleService.GetAllAsync();

            A.CallTo(() => _roleRepository.FindAllAsync(A<Expression<Func<Role, bool>>>._)).MustHaveHappenedOnceExactly();
        }

        private static RoleDto CreateRoleDto(string name = "Name")
        {
            var roleDto = new RoleDto
            {
                Name = name,
                Permissions = new List<string>()
            };

            return roleDto;
        }

        private static Role CreateRole(string name = "Name 2")
        {
            var role = new Role
            {
                Name = name
            };

            return role;
        }
    }
}