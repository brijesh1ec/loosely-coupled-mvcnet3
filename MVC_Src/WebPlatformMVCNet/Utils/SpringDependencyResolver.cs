using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spring.Context;
using System.Collections;
using Spring.Objects.Factory.Xml;

namespace WebPlatformMVCNet.Utils
{
    public class SpringDependencyResolver : IDependencyResolver
    {
        private Spring.Objects.Factory.Config.IConfigurableListableObjectFactory springFactory;

        public SpringDependencyResolver(Spring.Objects.Factory.Config.IConfigurableListableObjectFactory factory)
        {
            this.springFactory = factory;
        }

        #region IDependencyResolver Members

        public object GetService(Type serviceType)
        {
            var instances = this.springFactory.GetObjectsOfType(serviceType);
            var enumerator = instances.GetEnumerator();

            enumerator.MoveNext();
            try
            {
                return enumerator.Value;
            }
            catch (Exception)
            {
                return null;
            }
        } 

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.springFactory.GetObjectsOfType(serviceType).Cast<object>();
        }

        #endregion
    }
}