﻿﻿﻿using System.Collections.Generic;

   namespace GameStore.SeedingServices.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
    }
}