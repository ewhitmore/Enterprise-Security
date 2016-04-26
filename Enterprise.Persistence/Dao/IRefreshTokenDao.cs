using System;
using Enterprise.Model;

namespace Enterprise.Persistence.Dao
{
    public interface IRefreshTokenDao : IRepository<RefreshToken, int>
    {
    }
}