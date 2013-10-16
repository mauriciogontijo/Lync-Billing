using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend;

namespace Lync_Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            Interfaces.IDatabaseImporter importer = new Implementation.Lync2010();
            importer.ImportPhoneCalls();

        }
    }
}
