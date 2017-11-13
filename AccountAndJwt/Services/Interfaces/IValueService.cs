using AccountAndJwt.Services.Models;
using System;

namespace AccountAndJwt.Services.Interfaces
{
    public interface IValueService
    {
        ValueDto[] GetAll();
        void Delete(Int32 id);
        void Update(ValueDto value);
        ValueDto Add(String value);
        ValueDto Get(Int32 id);
    }
}