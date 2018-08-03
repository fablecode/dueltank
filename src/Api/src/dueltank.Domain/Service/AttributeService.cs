using dueltank.core.Models.Db;
using dueltank.core.Services;
using dueltank.Domain.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dueltank.Domain.Service
{
    public class AttributeService : IAttributeService
    {
        private readonly IAttributeRepository _attributeRepository;

        public AttributeService(IAttributeRepository attributeRepository)
        {
            _attributeRepository = attributeRepository;
        }

        public Task<IList<Attribute>> AllAttributes()
        {
            return _attributeRepository.AllAttributes();
        }
    }
}