using System;
using System.Collections.Generic;
using System.Linq;
using ValidatorPatternExample.Repositories;
using ValidatorPatternExample.Models;

namespace ValidatorPatternExample.Validators
{
    public class AddItemValidator : IValidator
    {
        public IList<string> ValidationMessages { get; set; }
        private IItemRepository itemRepository;
        private Item itemToValidate;

        public AddItemValidator(IItemRepository itemRepository, Item itemToValidate)
        {
            this.itemRepository = itemRepository;
            this.itemToValidate = itemToValidate;
            ValidationMessages = new List<string>();
        }

        public bool Validates()
        {
            var duplicateItem = itemRepository.FindByModelNumber(itemToValidate.ModelNumber);
            if(duplicateItem != null)
            {
                ValidationMessages.Add(String.Format("Item with model # '{0}' already exists", itemToValidate.ModelNumber));
            }

            return !ValidationMessages.Any();
        }
    }

    public static class AddItemValidatorFactory
    {
        public static IValidator Validator { get; set; }
        public static IItemRepository ItemRepository { get; set; }
        public static Item ItemToValidate { get; set; }

        public static IValidator Create()
        {
            if (Validator != null)
            {
                return Validator;
            }
            else if(ItemRepository != null && ItemToValidate != null)
            {
                return new AddItemValidator(ItemRepository, ItemToValidate);
            }

            throw new Exception("AddItemValidatorFactory requires either an instance of IValidator or IItemRepository and Item");
        }
    }
}
