﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OEA;
using OEA.Library.Caching;
using OEA.Module.WPF;

namespace OEA.ClientCachingProvider
{
    /// <summary>
    /// 本模块主要用于为 OEA 的缓存系统服务端提供一个数据表存放缓存相关的信息。
    /// </summary>
    internal class DCLibrary : ILibrary
    {
        public ReuseLevel ReuseLevel
        {
            get { return ReuseLevel._System; }
        }

        public void Initialize(IApp app)
        {
            EntityListVersion.SetProvider(new EntityListVersionRepository());

            //服务端在启动时，需要清空缓存信息库，这会导致所有客户端都重新下载最新的数据。
            if (OEAEnvironment.Location == OEALocation.WPFServer)
            {
                app.StartupCompleted += (o, e) => { EntityListVersion.Repository.Clear(); };
            }

            app.DbMigratingOperations += (o, e) =>
            {
                MigrationWithProgressBar.Do(ConnectionStringNames.OEAPlugins);
            };
        }
    }
}