using SutekiTmp.Domain.Repository.IRepository;
using SutekiTmp.Models.Temp.Data;

namespace SutekiTmp.Domain.Repository.Repository
{
    public class PromisionRepository : IPromisionRepository
    {
        public int GetPromisionIdByPromisionName(string PromisionName)
        {
            return TempData.promisions.First(c => c.PromisionName == PromisionName).PromisionId;
        }
    }
}
