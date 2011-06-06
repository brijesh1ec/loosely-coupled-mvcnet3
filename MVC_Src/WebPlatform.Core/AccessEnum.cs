using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebPlatform.Core
{
    public enum Modules : byte
    {
        Administration = 1,
        Product = 2,
        Orders = 3
    }

    [Flags]
    public enum Functions
    {
        Access = 1,
        ManageUsers = 2,
        ManageOrders = 4,
        ManageProducts = 8

    }
}
