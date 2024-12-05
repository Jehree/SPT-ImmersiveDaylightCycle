using Comfort.Common;
using Fika.Core.Modding;
using Fika.Core.Modding.Events;
using Fika.Core.Networking;
using Jehree.ImmersiveDaylightCycle;
using Jehree.ImmersiveDaylightCycle.FikaNetworking;
using Jehree.ImmersiveDaylightCycle.Helpers;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Jehree.ImmersiveDaylightCycle.FikaNetworking {
    public class DaylightSync
    {
        private NetDataWriter _writer;

        private NetDataWriter GetNetDataWriter()
        {
            if (_writer == null) {
                _writer = new NetDataWriter();
            } else {
                _writer.Reset();
            }

            return _writer;
        }

        private static void OnHostGameStartedEventReceived(DaylightSyncPacket packet)
        {
            
            if (!Singleton<FikaClient>.Instantiated) {
                throw new Exception("Server sent OnHostGameStartedEventReceived but FikaClient is not instantiated");
            }

            Settings.SetCurrentGameTime((int)packet.hostDateTime.x, (int)packet.hostDateTime.y, (int)packet.hostDateTime.z);
            Utils.SetRaidTime(packet.hostCycleRate);
        }
        public static bool IAmFikaClient()
        {
            if (Singleton<FikaClient>.Instantiated) return true;
            return false;
        }

        public static void OnHostGameStarted(Vector3 hostDateTime)
        {
            if (!Singleton<FikaServer>.Instantiated) return;

            DaylightSyncPacket packet = new DaylightSyncPacket { hostDateTime = hostDateTime };

            Singleton<FikaServer>.Instance.SendDataToAll(ref packet, LiteNetLib.DeliveryMethod.ReliableUnordered);

            Plugin.LogSource.LogInfo("Host game ran OnHostGameStarted, packets should have sent!");
        }

        public static void InitOnEnable()
        {
            FikaEventDispatcher.SubscribeEvent<FikaNetworkManagerCreatedEvent>(OnFikaNetManagerCreatedEvent);
            FikaEventDispatcher.SubscribeEvent<FikaNetworkManagerDestroyedEvent>(OnFikaNetManagerDestroyedEvent);
        }

        public static void InitOnDisable()
        {
            FikaEventDispatcher.UnsubscribeEvent<FikaNetworkManagerCreatedEvent>(OnFikaNetManagerCreatedEvent);
            FikaEventDispatcher.UnsubscribeEvent<FikaNetworkManagerDestroyedEvent>(OnFikaNetManagerDestroyedEvent);
        }

        private static void OnFikaNetManagerCreatedEvent(FikaNetworkManagerCreatedEvent netManagerCreatedEvent)
        {
            // listen for packet from server
            netManagerCreatedEvent.Manager.RegisterPacket<DaylightSyncPacket>(OnHostGameStartedEventReceived);
        }

        private static void OnFikaNetManagerDestroyedEvent(FikaNetworkManagerDestroyedEvent netManagerDestroyedEvent)
        {
            // remove listener from client
            // ??? maybe this isn't necessary anymore?
        }
    }
}