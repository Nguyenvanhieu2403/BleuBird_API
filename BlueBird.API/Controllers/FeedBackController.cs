using BlueBird.DataContext.Models;
using BlueBird.Reponsitory;
using BlueBird.Reponsitory.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTable.DataContext.Models;

namespace BlueBird.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackRepons _feedBackRepons;

        public FeedBackController(IFeedBackRepons feedBackRepons) 
        {
            _feedBackRepons = feedBackRepons;
        }
        [HttpPost]
        public async Task<MethodResult> PostFeedBack(string token, Guid IdProduct, FeedBackModel model)
        {
            var result = await _feedBackRepons.PostFeedBackAsync(token, IdProduct, model);
            if(result == null || result == "FeedBack Error") 
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpPut]
        public async Task<MethodResult> UpdateFeedBack(string token, Guid Id, FeedBackModel model)
        {
            var result = await _feedBackRepons.UpdateFeedBackAsync(token, Id, model);
            if (result == null || result == "FeedBack Error")
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpDelete]
        public async Task<MethodResult> DeleteFeedBack(Guid IdProduct, string token)
        {
            var result = await _feedBackRepons.DeleteFeedBackAsync(IdProduct, token);
            if (result == null || result == "Xóa thất bại")
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpGet]
        public async Task<MethodResult> GetALlFeedBack(Guid IdProduct, int pageIndex, int pageSize)
        {
            var result = await _feedBackRepons.GetAllFeedBack(IdProduct, pageIndex, pageSize);
            if (result.Item1 == null)
            {
                return MethodResult.ResultWithError(result.Item1, 400, "Error", result.Item2);
            }
            return MethodResult.ResultWithSuccess(result.Item1, 200, "Successfull", result.Item2);
        }
    }
}
