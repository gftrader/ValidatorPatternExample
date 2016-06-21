using ValidatorPatternExample.Models;

namespace ValidatorPatternExample.Repositories
{
    public class ItemRepository : IItemRepository
    {
        public ItemRepository()
        {
        }

        public Item FindByModelNumber(string modelNumber)
        {
            return null;
        }

        public void Add(Item item)
        {

        }
    }
}
