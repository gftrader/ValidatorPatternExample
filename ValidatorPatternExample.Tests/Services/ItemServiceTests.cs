using System;
using NUnit.Framework;
using Rhino.Mocks;
using ValidatorPatternExample.Models;
using ValidatorPatternExample.ORM;
using ValidatorPatternExample.Repositories;
using ValidatorPatternExample.Services;
using ValidatorPatternExample.Validators;

namespace ValidatorPatternExample.Tests.Services
{
    [TestFixture]
    public class ItemServiceTests
    {
        private IDbContext fakeDbContext;
        private IItemRepository fakeItemRepository;
        private IValidator fakeValidator;
        private ItemService itemService;

        [SetUp]
        public void SetupTests()
        {
            fakeDbContext = MockRepository.GenerateMock<IDbContext>();
            fakeItemRepository = MockRepository.GenerateMock<IItemRepository>();
            fakeValidator = MockRepository.GenerateMock<IValidator>();
            itemService = new ItemService(fakeDbContext, fakeItemRepository);
        }

        [Test]
        public void AddItem_Validates_CallsAddAndSaveChanges()
        {
            var fakeItem = new Item() { ModelNumber = "1230498049234" };
            AddItemValidatorFactory.Validator = fakeValidator;
            fakeValidator.Stub(v => v.Validates()).Return(true);
            fakeItemRepository.Stub(fir => fir.FindByModelNumber(fakeItem.ModelNumber)).Return(null);

            itemService.AddItem(fakeItem);

            fakeItemRepository.AssertWasCalled(fir => fir.Add(fakeItem));
            fakeDbContext.AssertWasCalled(fdc => fdc.SaveChanges());
        }

        [Test]
        public void AddItem_DoesntValidate_ThrowsNotImplementedException()
        {
            AddItemValidatorFactory.Validator = fakeValidator;
            fakeValidator.Stub(v => v.Validates()).Return(false);

            Assert.Throws<NotImplementedException>(() => itemService.AddItem(new Item()));
        }
    }
}
