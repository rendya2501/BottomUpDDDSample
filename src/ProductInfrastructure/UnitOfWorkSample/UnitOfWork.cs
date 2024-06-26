using Domain.Domain.Users;
using Domain.UnitOfWorkSample;
using System;
using System.Data;

namespace ProductInfrastructure.UnitOfWorkSample;

public class UnitOfWork : IUnitOfWork
{
    private IDbConnection _connection;
    private IDbTransaction _transaction;
    private bool _disposed;

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_connection);
    private IUserRepository _userRepository;

    public UnitOfWork(IDbConnection dbConnection)
    {
        _connection = dbConnection;
        _connection.Open();
        _transaction = dbConnection.BeginTransaction();
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }

    public void Commit()
    {
        try
        {
            _transaction.Commit();
        }
        catch
        {
            _transaction.Rollback();
            throw;
        }
        finally
        {
            _transaction.Dispose();
            _transaction = _connection.BeginTransaction();
            ResetRepositories();
        }
    }

    public void Rollback()
    {
        _transaction.Rollback();
    }

    private void ResetRepositories()
    {
        _userRepository = null!;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed || !disposing)
        {
            return;
        }

        if (_transaction != null)
        {
            _transaction.Dispose();
            _transaction = null!;
        }
        if (_connection != null)
        {
            _connection.Dispose();
            _connection = null!;
        }
        _disposed = true;
    }
}