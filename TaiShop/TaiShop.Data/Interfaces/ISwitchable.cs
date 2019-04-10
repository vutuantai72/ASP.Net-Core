using System;
using System.Collections.Generic;
using System.Text;
using TaiShop.Data.Enums;

namespace TaiShop.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { get; set; }
    }
}
