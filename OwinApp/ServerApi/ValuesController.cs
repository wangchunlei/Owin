using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServerApi
{
    public class ValuesController : ApiController
    {
        public IEnumerable<string> GetValues()
        {
            return new string[] { "value1", "value2" };
        }

        public string GetValue()
        {
            return "value";
        }

        public void PostValue([FromBody]string value)
        {
            
        }
    }
}
