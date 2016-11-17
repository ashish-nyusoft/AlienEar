using System;
using Nop.Services.Logging;
using Nop.Services.Tasks;
using Nop.Core;

namespace Nop.Services.application
{
    /// <summary>
    /// Represents a task for sending queued message 
    /// </summary>
    public partial class RestartApplication : ITask
    {

        private readonly IWebHelper _webHelper;
        public RestartApplication(IWebHelper webHelper)
        {
            this._webHelper = webHelper;
        }

        /// <summary>
        /// Executes a task
        /// </summary>
        public virtual void Execute()
        {
            _webHelper.RestartAppDomain();
        }
    }
}
