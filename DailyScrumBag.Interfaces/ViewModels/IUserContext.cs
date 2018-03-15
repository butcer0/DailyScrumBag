using System;
using System.Collections.Generic;
using System.Text;

namespace DailyScrumBag.Interfaces.ViewModels
{
    public interface IUserContext
    {
        int UId { get; set; }

        int UserId { get; set; }

        string Username { get; set; }

        Dictionary<string, string> DSSettings { get; set; }
    }
}
