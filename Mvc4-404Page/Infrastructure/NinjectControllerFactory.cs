using Mvc4_404Page.Controllers;
using Ninject;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc4_404Page
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory(IKernel kernel)
        {
            ninjectKernel = kernel;
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                if (controllerType == null)
                    return base.GetControllerInstance(requestContext, controllerType);
            }
            catch (HttpException ex)
            {
                if (ex.GetHttpCode() == 404)
                {
                    IController errorController = ninjectKernel.Get<ErrorController>();
                    ((ErrorController)errorController).InvokeHttp404(requestContext.HttpContext);

                    return errorController;
                }
                else
                    throw ex;
            }

            return ninjectKernel.Get(controllerType) as Controller;
        }

        private void AddBindings()
        {
            // put additional bindings here
        }
    }
}
