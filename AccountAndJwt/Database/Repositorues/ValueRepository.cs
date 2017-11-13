﻿using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Database.Models;

namespace AccountAndJwt.Database.Repositorues
{
    internal class ValueRepository : EfRepositoryBase<ValueDb, DataContext>, IValueRepository
    {
        public ValueRepository(DataContext context) : base(context)
        {

        }
    }
}