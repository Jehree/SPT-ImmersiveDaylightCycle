using Fika.Core.Networking;
using LiteNetLib.Utils;
using UnityEngine;

namespace Jehree.ImmersiveDaylightCycle_FikaBridge {
    public class DaylightSyncPacket : INetSerializable
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

    public class RequestSyncDataPacket : INetSerializable
    {
        public void Deserialize(NetDataReader reader) { }
        public void Serialize(NetDataWriter writer) { }
    }
}