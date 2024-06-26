using Domain.Domain.Users;
using System;

namespace Domain.UnitOfWorkSample;

public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }

    void Commit();
    void Rollback();
}
