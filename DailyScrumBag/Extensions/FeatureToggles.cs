using DailyScrumBag.Interfaces.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrumBag.Extensions
{
    /// <summary>
    /// Enable/Disable Site Wide Features
    /// </summary>
    public class FeatureToggles : IFeatureToggles
    {
        /// <summary>
        /// Enable Developer Exceptions
        /// </summary>
        public bool EnableDeveloperExceptions { get; set; }
    }
}
