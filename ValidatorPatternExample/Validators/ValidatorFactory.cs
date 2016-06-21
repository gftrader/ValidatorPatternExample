using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidatorPatternExample.Models;
using ValidatorPatternExample.Repositories;

namespace ValidatorPatternExample.Validators
{
    public static class ValidatorFactory
    {
        public static IValidator Validator { get; set; }

        public static IValidator Create(ValidatorConfiguration validatorConfiguration)
        {
            if (Validator != null)
            {
                return Validator;
            }
            else if (validatorConfiguration != null)
            {
                return GetValidator(validatorConfiguration);
            }

            throw new Exception("AddItemValidatorFactory requires either an instance of IValidator or IItemRepository and Item");
        }

        private static IValidator GetValidator(ValidatorConfiguration validatorConfiguration)
        {
            switch(validatorConfiguration.ValidatorType)
            {
                case ValidatorTypeEnum.AddItem: return new AddItemValidator(validatorConfiguration.ItemRepository, validatorConfiguration.ItemToValidate);
                case ValidatorTypeEnum.UpdateItems: return new UpdateItemsValidator(validatorConfiguration.ItemRepository, validatorConfiguration.ItemsToValidate);
                default: throw new Exception(String.Format("No validator matches given ValidatorType '{0}'", Enum.GetName(typeof(ValidatorTypeEnum), validatorConfiguration.ValidatorType)));
            }
        }
    }

    public class ValidatorConfiguration
    {
        public ValidatorTypeEnum ValidatorType { get; set; }
        public IItemRepository ItemRepository { get; set; }
        public Item ItemToValidate { get; set; }
        public IEnumerable<Item> ItemsToValidate { get; set; }
    }

    public enum ValidatorTypeEnum
    {
        AddItem,
        UpdateItems
    }
}
