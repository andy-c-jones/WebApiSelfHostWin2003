using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using log4net;

namespace WebApiForWin2003
{
    public class MoogleController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [HttpHead]
        public HttpResponseMessage Head()
        {
            Log.Info("[Moogle] Head request received");
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            Log.Info("[Moogle] Get request received");
            return Request.CreateResponse(HttpStatusCode.OK, "Hello, yes, this is Moogle");
        }
    }
}