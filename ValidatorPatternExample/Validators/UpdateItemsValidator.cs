using System;
using System.Collections.Generic;
using System.Linq;
using ValidatorPatternExample.Repositories;
using ValidatorPatternExample.Models;

namespace ValidatorPatternExample.Validators
{
    public class UpdateItemsValidator : IValidator
    {
        public IList<string> ValidationMessages { get; set; }
        private IItemRepository itemRepository;
        private IEnumerable<Item> itemsToValidate;

        public UpdateItemsValidator(IItemRepository itemRepository, IEnumerable<Item> itemsToValidate)
        {
            this.itemRepository = itemRepository;
            this.itemsToValidate = itemsToValidate;
            ValidationMessages = new List<string>();
        }

        public bool Validates()
        {
            if(itemsToValidate.GroupBy(i => i.ModelNumber).Select(g => g.Key).Count() != itemsToValidate.Count())
            {
                ValidationMessages.Add("Attempted to update the same item multiple times");
            }
            foreach(var itemToValidate in itemsToValidate)
            {
                ValidateItem(itemToValidate);
            }

            return !ValidationMessages.Any();
        }

        private void ValidateItem(Item itemToValidate)
        {
            var dbItem = itemRepository.FindByModelNumber(itemToValidate.ModelNumber);
            if (dbItem == null)
            {
                ValidationMessages.Add(String.Format("Item with model # '{0}' does not exist in the database", itemToValidate.ModelNumber));
            }
        }
    }

    public static class UpdateItemsValidatorFactory
    {
        public static IValidator Validator { get; set; }
        public static IItemRepository ItemRepository { get; set; }
        public static IEnumerable<Item> ItemsToValidate { get; set; }

        public static IValidator Create()
        {
            if (Validator != null)
            {
                return Validator;
            }
            else if(ItemRepository != null && ItemsToValidate != null)
            {
                return new UpdateItemsValidator(ItemRepository, ItemsToValidate);
            }

            throw new Exception("UpdateItemsValidatorFactory requires either an instance of IValidator or IItemRepository and IEnumerable<Item>");
        }
    }
}
