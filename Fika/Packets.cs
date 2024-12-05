using Fika.Core.Networking;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jehree.ImmersiveDaylightCycle.Fika {
    internal class DaylightSyncPacket : INetSerializable
    {
        public Vector3 HostDateTime;
        public float HostCycleRate;

        public void Deserialize(NetDataReader reader)
        {
            HostDateTime = reader.GetVector3();
            HostCycleRate = reader.GetFloat();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(HostDateTime);
            writer.Put(HostCycleRate);
        }
    }

    internal class RequestSyncDataPacket : INetSerializable
    {
        public void Deserialize(NetDataReader reader) { }
        public void Serialize(NetDataWriter writer) { }
    }
}