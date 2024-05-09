using BlueBird.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.Reponsitory.Interface
{
    public interface IFeedBackRepons
    { 
        public Task<string> PostFeedBackAsync(string token, Guid IdProduct, FeedBackModel feedBackModel);
        public Task<(List<FeedBackDisplayModel>, int)> GetAllFeedBack(Guid IdProduct, int pageIndex, int pageSize);
        public Task<string> UpdateFeedBackAsync(string token, Guid Id, FeedBackModel feedBackModel);
        public Task<string> DeleteFeedBackAsync(Guid IdProduct, string token);
    }
}
