using System;
using System.Collections.Generic;
using System.Text;

namespace DailyScrumBag.Interfaces.Extensions
{
    /// <summary>
    /// Enable/Disable Site Wide Features
    /// </summary>
    public interface IFeatureToggles
    {
        /// <summary>
        /// Enable Developer Exceptions
        /// </summary>
        bool EnableDeveloperExceptions { get; set; }
    }
}
