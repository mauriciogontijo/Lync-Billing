﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lync_Backend.Helpers;

namespace Lync_Backend.Interfaces
{
    interface IDatabaseImporter
    {
        string ConstructConnectionString();
        void ImportPhoneCalls();
        void ImportGateways();
        void ImportPools();
    }
}