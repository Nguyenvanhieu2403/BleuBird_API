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
    public class ProducerController : ControllerBase
    {
        private readonly IProducerRepons _producerRepons;

        public ProducerController(IProducerRepons producerRepons) 
        {
            _producerRepons = producerRepons;
        }
        [HttpPost]
        public async Task<MethodResult> AddProducer(ProducerModel producerModel, string token)
        {
            var result = await _producerRepons.AddProducerAsync(producerModel, token);
            if(result == null)
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpGet]
        public async Task<MethodResult> GetAllProducer(int pageIndex, int pageSize)
        {
            var result = await _producerRepons.GetAllProducerAsync(pageIndex, pageSize);
            if(result.Item1 == null)
            {
                return MethodResult.ResultWithError(result.Item1, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result.Item1, 200, "Successfull", result.Item2);
        }


        [HttpGet("Search")]
        public async Task<MethodResult> GetProducerBySearch(string search, int pageIndex, int pageSize)
        {
            var result = await _producerRepons.GetProducerBySearchAsync(search, pageIndex, pageSize);
            if (result.Item1 == null)
            {
                return MethodResult.ResultWithError(result.Item1, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result.Item1, 200, "Successfull", result.Item2);
        }

        [HttpDelete]
        public async Task<MethodResult> DeleteProducer(Guid Id)
        {
            var result = await _producerRepons.DeleteProducerAsync(Id);
            if (result == null || result == "Nhà cung cấp không thể xóa vì đang có sản phẩm kinh doanh")
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }

        [HttpPut]
        public async Task<MethodResult> UpdateProducer(Guid Id, ProducerModel producerModel, string token)
        {
            var result = await _producerRepons.UpdateProducerAsync(Id, producerModel, token);
            if (result == null || result == "Sửa nhà cung cấp không thành công")
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
    }
}
