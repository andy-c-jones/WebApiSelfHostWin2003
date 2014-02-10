using System.Configuration;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Http.SelfHost;
using log4net;
using Topshelf.Runtime;

namespace WebApiForWin2003
{
    internal class MoogleService
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly HttpSelfHostServer _server;

        private MoogleService(HttpSelfHostConfiguration configuration)
        {
            _server = new HttpSelfHostServer(configuration);
        }

        public void Start()
        {
            _server.OpenAsync().Wait();
            Log.Info("[Moogle] Service started");
        }

        public void Stop()
        {
            _server.CloseAsync().Wait();
            _server.Dispose();
            Log.Info("[Moogle] Service stopped");
        }

        public static MoogleService Create(HostSettings settings)
        {
            var configuration = new HttpSelfHostConfiguration(ConfigurationManager.AppSettings["MoogleServiceLocation"]);

            //configuration.Formatters.XmlFormatter.UseXmlSerializer = true;
            //configuration.Formatters.XmlFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/x-www-form-urlencoded"));

            configuration.Routes.MapHttpRoute("DefaultAPI",
                                              "api/{controller}/{id}",
                                              new { controller = "Moogle", id = RouteParameter.Optional });
            configuration.Routes.MapHttpRoute("MoogleHead",
                                              "{*any}",
                                              new {controller = "Moogle", action = "Head"},
                                              new {httpMethod = new HttpMethodConstraint(new[] {HttpMethod.Head})});

            return new MoogleService(configuration);
        }
    }
}