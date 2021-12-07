using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sentry_back_mvc.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            //SentrySdk.CaptureMessage("Something went wrong");
            try
            {
                    throw new ArgumentNullException("Param is null!");

                throw new Exception();
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                var ioe = new InvalidOperationException("Bad POST! See Inner exception for details.", e);

                ioe.Data.Add("inventory",
                    // The following object gets serialized:
                    new object []
                    {
                         3,
                        0,
                         512
                    });

                throw ioe;
            }
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
