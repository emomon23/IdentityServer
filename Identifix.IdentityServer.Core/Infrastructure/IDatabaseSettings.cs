using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identifix.IdentityServer.Infrastructure
{
    public interface IDatabaseSettings
    {
        string UserDatabaseConnectionString { get; }
    }
}
