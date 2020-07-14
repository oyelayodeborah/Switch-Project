
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BezSwitch.Controllers
{
    public class HomeController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok("Ask Talabi");
        }
    }
}
