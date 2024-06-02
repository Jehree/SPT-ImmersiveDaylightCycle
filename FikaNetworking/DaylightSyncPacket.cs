#if FIKA_COMPAT
using Fika.Core.Networking;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jehree.ImmersiveDaylightCycle.FikaNetworking {
    internal class DaylightSyncPacket : INetSerializable
    {
        public Vector3 hostDateTime;

        public void Deserialize(NetDataReader reader)
        {
            hostDateTime = reader.GetVector3();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(hostDateTime);
        }
    }
}
#endif