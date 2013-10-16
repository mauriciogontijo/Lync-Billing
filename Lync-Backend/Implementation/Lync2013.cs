using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Interfaces;

namespace Lync_Backend.Implementation
{
    class Lync2013 : IDatabaseImporter
    {
        public string ConstructConnectionString()
        {
            throw new NotImplementedException();
        }

        public void ImportPhoneCalls()
        {
            throw new NotImplementedException();
        }

        public void ImportGateways()
        {
            throw new NotImplementedException();
        }

        public void ImportPools()
        {
            throw new NotImplementedException();
        }
    }
}
