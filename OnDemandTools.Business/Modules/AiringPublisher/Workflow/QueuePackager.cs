using System;
using System.Collections.Generic;
using OnDemandTools.Business.Modules.AiringPublisher.Models;
using BLAiring = OnDemandTools.Business.Modules.Airing.Model;
using System.Linq;

namespace OnDemandTools.Business.Modules.AiringPublisher.Workflow
{
    public class QueuePackager : IPackager
    {
        public QueueAiring Package(BLAiring.Airing airing, Action action, List<BLAiring.ChangeNotification> notifications)
        {
            var queueAiring = new QueueAiring();
            queueAiring.AiringId = airing.AssetId;
            queueAiring.Action = action.ToString();

            if (notifications.Any())
            {
                ConsolidateAiringChangeNotification(queueAiring, notifications);
            }

            return queueAiring;
        }

        /// <summary>
        /// Consilidates all change notifications
        /// </summary>
        /// <param name="queueAiring">queue airing to send</param>
        /// <param name="notifications">notifications</param>
        private void ConsolidateAiringChangeNotification(QueueAiring queueAiring, List<BLAiring.ChangeNotification> notifications)
        {
            queueAiring.AiringChangeNotifications = new List<AiringChangeNotification>();

            foreach (var notification in notifications.OrderByDescending(e => e.ChangedDateTime))
            {
                if (!queueAiring.AiringChangeNotifications.Any(e => e.ChangeNotificationType == notification.ChangeNotificationType.ToString()))
                {
                    queueAiring.AiringChangeNotifications.Add(new AiringChangeNotification
                    {
                        ChangeNotificationType = notification.ChangeNotificationType.ToString(),
                        ChangedOn = notification.ChangedDateTime
                    });
                }

                if (notification.ChangedProperties != null && notification.ChangedProperties.Any())
                {
                    var airingNotification = queueAiring.AiringChangeNotifications.First(e => e.ChangeNotificationType == notification.ChangeNotificationType.ToString());

                    ConsolidateAiringChangeProperties(airingNotification, notification);
                }
            }
        }

        /// <summary>
        /// Consolidate's Airing Change Properties
        /// </summary>
        /// <param name="airingNotification">the airing notificaiton to send</param>
        /// <param name="notification">BL notificaiton</param>
        private void ConsolidateAiringChangeProperties(AiringChangeNotification airingNotification, BLAiring.ChangeNotification notification)
        {
            if (airingNotification.ChangedProperties == null)
            {
                airingNotification.ChangedProperties = new List<string>();
            }

            foreach (var changedProperty in notification.ChangedProperties)
            {
                if (!airingNotification.ChangedProperties.Contains(changedProperty))
                {
                    airingNotification.ChangedProperties.Add(changedProperty);
                }
            }
        }
    }
}