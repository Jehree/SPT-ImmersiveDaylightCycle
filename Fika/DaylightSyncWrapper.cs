using Fika.Core.Networking;
using Jehree.ImmersiveDaylightCycle.Fika;
using Jehree.ImmersiveDaylightCycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Comfort.Common;
using LiteNetLib;
using Fika.Core.Modding.Events;
using Fika.Core.Modding;
using EFT;
using Jehree.ImmersiveDaylightCycle.Helpers;

namespace ImmersiveDaylightCycle.Fika
{
    internal class DaylightSyncWrapper
    {
        public static bool IAmFikaClient()
        {
            if (Singleton<FikaClient>.Instantiated) return true;
            return false;
        }

        public static bool IAmFikaServer()
        {
            if (Singleton<FikaServer>.Instantiated) return true;
            return false;
        }
        private static void OnDataRequestReceived(RequestSyncDataPacket packet, NetPeer peer)
        {
            DateTime localDateTime = Settings.GetSavedGameTime();
            Vector3 localDateTimeToVector = new Vector3(localDateTime.Hour, localDateTime.Minute, localDateTime.Second);
            DaylightSyncPacket dataPacket = new DaylightSyncPacket
            {
                HostDateTime = localDateTimeToVector,
                HostCycleRate = Settings.daylightCycleRate.Value
            };

            if (IAmFikaServer())
            {
                Singleton<FikaServer>.Instance.SendDataToPeer(peer, ref packet, LiteNetLib.DeliveryMethod.ReliableUnordered);
            }
            else
            {
                Singleton<FikaClient>.Instance.SendData(ref packet, LiteNetLib.DeliveryMethod.ReliableUnordered);
            }
        }

        private static void OnResponseReceived(DaylightSyncPacket packet)
        {
            Settings.SaveGameTime((int)packet.HostDateTime.x, (int)packet.HostDateTime.y, (int)packet.HostDateTime.z);
            Settings.daylightCycleRate.Value = packet.HostCycleRate;
            Utils.SetRaidTime();
        }

        public static void OnClientGameStarted()
        {
            if (IAmFikaServer()) return;
            FikaClient fikaClient = Singleton<FikaClient>.Instance;

            RequestSyncDataPacket packet = new RequestSyncDataPacket();
            Singleton<FikaClient>.Instance.SendData(ref packet, DeliveryMethod.ReliableUnordered);
        }

        public static void InitOnEnable()
        {
            FikaEventDispatcher.SubscribeEvent<FikaNetworkManagerCreatedEvent>(OnFikaNetManagerCreatedEvent);
        }

        public static void InitOnDisable()
        {
            FikaEventDispatcher.UnsubscribeEvent<FikaNetworkManagerCreatedEvent>(OnFikaNetManagerCreatedEvent);
        }

        private static void OnFikaNetManagerCreatedEvent(FikaNetworkManagerCreatedEvent netManagerCreatedEvent)
        {
            netManagerCreatedEvent.Manager.RegisterPacket<DaylightSyncPacket>(OnResponseReceived);
            netManagerCreatedEvent.Manager.RegisterPacket<RequestSyncDataPacket, NetPeer>(OnDataRequestReceived);

            if (Plugin.IAmDedicatedClient)
            {
                FikaServer server = Singleton<FikaServer>.Instance;
                RequestSyncDataPacket packet = new RequestSyncDataPacket();
                server.SendDataToPeer(server.NetServer.FirstPeer, ref packet, DeliveryMethod.ReliableUnordered);
            }
        }
    }
}
