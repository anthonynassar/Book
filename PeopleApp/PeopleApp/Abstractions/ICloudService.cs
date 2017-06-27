using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Abstractions
{
    public interface ICloudService
    {
        ICloudTable<T> GetTable<T>() where T : TableData;
    }
}