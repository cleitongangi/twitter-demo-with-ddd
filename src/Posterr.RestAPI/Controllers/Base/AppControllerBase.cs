using Microsoft.AspNetCore.Mvc;

namespace Posterr.RestAPI.Controllers.Base
{
    public abstract class AppControllerBase : ControllerBase
    {
        protected long GetAuthenticatedUserId()
        {
            // Hard-coded because don't have authorization implemented            
            // Usually this information will come from the JWT token            
            return 1; // If you change it to 2, it will generate errors in the integration tests
        }
    }
}
