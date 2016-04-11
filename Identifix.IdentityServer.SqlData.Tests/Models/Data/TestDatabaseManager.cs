using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identifix.IdentityServer.Models.Data;
using Identifix.IdentityServer.Tests.Models.Data.Identifix.IdentityServer.Models.Data;

namespace Identifix.IdentityServer.Tests.Models.Data
{
    public static class TestDatabaseManager
    {

        public static int NumberOfCountries { get; private set; }
        
        private static readonly LockObject InitializationLock = new LockObject();

        public static void InitializeDatabase()
        {
            lock (InitializationLock)
            if (!InitializationLock.IsInitialized)
            {
                InitializationLock.IsInitialized = true;
                Database.SetInitializer(new TestUserDatabaseInitializer());
            }
        }

        /// <summary>
        /// This class is used for a lock handle and to track if things are initialized.
        /// </summary>
        internal class LockObject
        {
            internal LockObject()
            {
                IsInitialized = false;
            }

            internal bool IsInitialized { get; set; }
        }
    }
}
