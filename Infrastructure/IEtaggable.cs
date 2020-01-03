using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Infrastructure
{
    public interface IEtaggable
    {
        string GetEtag();
    }
}