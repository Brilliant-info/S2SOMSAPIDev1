using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S2SOMSAPI.Model;
using S2SOMSAPI.Repository.Interface;
using System.Security.Cryptography.X509Certificates;

namespace S2SOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S2SOMSOrderController : ControllerBase
    {
        public readonly IS2SOMSOrders _Injection;
        public readonly IS2SOrderHistory _History;

        public S2SOMSOrderController(IS2SOMSOrders Injection, IS2SOrderHistory history)
        {
            _Injection = Injection;
            _History = history;
        }

        [HttpPost("GetOrderList")]
        public async Task<ActionResult> GetS2SOrders(S2SOrderListReq para)
        {
            var response = await _Injection.GetS2SOrders(para);
            return Ok(response);
        }

        [HttpPost("GetOrderHistory")]

        public async Task<ActionResult> S2SOrderHistory(S2SOrderHistoryReq reqpara)
        {
            var response = await _History.S2SOrderHistory(reqpara);
            return Ok(response);
        }

    }
}
