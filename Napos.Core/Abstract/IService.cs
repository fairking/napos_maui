using System;
using System.Collections.Generic;
using System.Text;

namespace Napos.Core.Abstract
{
    public interface IService
    {
    }

    public interface IService<T> : IService
    {
    }

    public interface IScopedService : IService
    {
    }

    public interface IScopedService<T> : IScopedService
    { 
    }

    public interface ITransientService : IService
    { 
    }

    public interface ITransientService<T> : ITransientService
    { 
    }

    /// <summary>
    /// Many implementation services can be registered under one service type.
    /// </summary>
    public interface IMultiService
    { 
    }
}
