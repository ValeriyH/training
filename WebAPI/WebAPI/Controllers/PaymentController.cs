using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PaymentController : ApiController
    {
        static PaymentStorage _data = new PaymentStorage();
        // GET api/values

        public IEnumerable<Payment> Get()
        {
            return _data.GetEnumerator();
            //return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public Payment Get(int id)
        {
            return _data.GetPayment(id);
            //return String.Format("value {0}", id);
        }

        //TODO Create loan system. Pay date/amount. history, how much left. calculate and etc.
        // POST api/values
        public HttpResponseMessage Post([FromBody]Payment value)
        {
            _data.AddPayment(value);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]Payment value)
        {
            _data.UpdatePayment(id, value);
            //return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/values/5
        public HttpResponseMessage Delete(int id)
        {
            _data.RemovePayment(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/values/{id}/info")]
        public IEnumerable<string> GetInfo(int id)
        {
            return new string[] { "value1", "value2" };
        }
    }
}
