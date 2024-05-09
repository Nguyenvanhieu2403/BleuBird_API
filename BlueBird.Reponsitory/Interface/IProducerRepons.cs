using BlueBird.DataConext.Data;
using BlueBird.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.Reponsitory.Interface
{
    public interface IProducerRepons
    {
        public Task<string> AddProducerAsync(ProducerModel producerModel, string token);
        public Task<(List<Producer>, int)> GetAllProducerAsync(int pageIndex, int pageSize);
        public Task<(List<Producer>, int)> GetProducerBySearchAsync(string search ,int pageIndex, int pageSize);
        public Task<string> UpdateProducerAsync(Guid Id,ProducerModel producerModel, string token);
        public Task<string> DeleteProducerAsync(Guid Id);
    }
}
