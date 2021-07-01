using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using SampleDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreRESTUniformInterface.Controllers
{
    public class SampleControllerBase : ControllerBase
    {
        protected void ApplyToValidated(Action jsonPatchApply)
        {
            try
            {
                jsonPatchApply();
            }
            catch (JsonPatchException jpe)
            {
                ModelState.AddModelError("error", jpe.Message);
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("error", ane.Message);
            }
        }
    }
}
