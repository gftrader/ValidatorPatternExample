using ValidatorPatternExample.Models;

namespace ValidatorPatternExample.Repositories
{
    public interface IItemRepository
    {
        Item FindByModelNumber(string modelNumber);
        void Add(Item item);
    }
}
