using System;
using System.Collections.Generic;
using ValidatorPatternExample.Models;
using ValidatorPatternExample.ORM;
using ValidatorPatternExample.Repositories;
using ValidatorPatternExample.Validators;

namespace ValidatorPatternExample.Services
{
    public class ItemService : IItemService
    {
        private IDbContext context;
        private IItemRepository itemRepository;
        
        public ItemService(IDbContext context, IItemRepository itemRepository)
        {
            this.context = context;
            this.itemRepository = itemRepository;
        }

        public void AddItem(Item item)
        {
            var validator = CreateAddItemValidator(item);
            if (validator.Validates())
            {
                itemRepository.Add(item);
                context.SaveChanges();
            }
            else
            {
                HandleFailedValidation(validator);
            }
        }

        private IValidator CreateAddItemValidator(Item item)
        {
            AddItemValidatorFactory.ItemRepository = itemRepository;
            AddItemValidatorFactory.ItemToValidate = item;
            return AddItemValidatorFactory.Create();

            //return ValidatorFactory.Create(new ValidatorConfiguration()
            //{
            //    ValidatorType = ValidatorTypeEnum.AddItem,
            //    ItemRepository = itemRepository,
            //    ItemToValidate = item
            //});
        }

        private void HandleFailedValidation(IValidator validator)
        {
            throw new NotImplementedException();
        }

        public void UpdateItems(IEnumerable<Item> items)
        {
            var validator = CreateUpdateItemsValidator(items);
            if (validator.Validates())
            {
                foreach (var item in items)
                {
                    var dbItem = itemRepository.FindByModelNumber(item.ModelNumber);
                    dbItem.Description = item.Description;
                    dbItem.Price = item.Price;
                }
                context.SaveChanges();
            }
            else
            {
                HandleFailedValidation(validator);
            }
        }

        private IValidator CreateUpdateItemsValidator(IEnumerable<Item> items)
        {
            UpdateItemsValidatorFactory.ItemRepository = itemRepository;
            UpdateItemsValidatorFactory.ItemsToValidate = items;
            return UpdateItemsValidatorFactory.Create();

            //return ValidatorFactory.Create(new ValidatorConfiguration()
            //{
            //    ValidatorType = ValidatorTypeEnum.UpdateItems,
            //    ItemRepository = itemRepository,
            //    ItemsToValidate = items,
            //});
        }
    }
}
