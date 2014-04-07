using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace OwinSelfhostSample
{
    public class AssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<System.Reflection.Assembly> GetAssemblies()
        {
            var baseAssemblies = base.GetAssemblies();
            var assemblies = new List<Assembly>(baseAssemblies);
            var type = typeof(ApiController);
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var files = System.IO.Directory.GetFiles(path, "*api.dll");
            foreach (var file in files)
            {
                var asm = Assembly.LoadFrom(file);
                assemblies.Add(asm);
            };

            //var apiAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList()
            //    .SelectMany(a => a.GetTypes())
            //    .Where(t => type.IsAssignableFrom(t)).Select(t => t.Assembly);
            //var controllersAssembly = Assembly.LoadFrom()
            return assemblies;
        }
    }
}
