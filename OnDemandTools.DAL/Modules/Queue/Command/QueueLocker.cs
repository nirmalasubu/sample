using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.DAL.Modules.Queue.Command
{
    public class QueueLocker : IQueueLocker, IClearQueueLocker
    {
        private readonly MongoCollection<DeliveryQueueLock> _collection;
        private readonly AppSettings _appSetting;

        public QueueLocker(IODTPrimaryDatastore connection, AppSettings appSetting)
        {
            var database = connection.GetDatabase();
            _appSetting = appSetting;

            _collection = database.GetCollection<DeliveryQueueLock>("DeliveryQueueLock");           
        }

        public DeliveryQueueLock AquireLockFor(string name, string processId)
        {
            var qLocks = _collection.Find(Query.EQ("Name", name)).ToList();

            //Verifies if there is any existing locks
            if (qLocks.Any())
            {
                //cleans up expired locks
                PurgeExpiredLocks(name, qLocks);

                //Verifies if there is any locks.
                qLocks = _collection.Find(Query.EQ("Name", name)).ToList();

                //returns locked processor details
                if (qLocks.Any())
                    return new DeliveryQueueLock(name, qLocks.First().ProcessId + ".");
            }

            //create new lock
            var qLock = new DeliveryQueueLock(name, processId);
            _collection.Save(qLock);

            qLocks = _collection.Find(Query.EQ("Name", name)).ToList();

            if (IsUnique(qLocks))
            {
                return qLock;
            }
            else
            {
                //another process already locked the queue, so clean up current lock.
                ReleaseLockFor(name, processId);
            }

            return new DeliveryQueueLock(name, qLocks.First(q => q.Id != qLock.Id).ProcessId + ".");
        }

        private List<DeliveryQueueLock> PurgeExpiredLocks(string name, IEnumerable<DeliveryQueueLock> qLocks)
        {
            var expiredMinutes = _appSetting.JobSchedules.QueueLockExpireMinute;

            var expiredDateTime = DateTime.UtcNow.AddMinutes(-expiredMinutes);

            ReleaseExpiredLocksFor(name, expiredDateTime);

            return qLocks.Where(l => l.LockedOn > expiredDateTime).ToList();
        }

        public void ReleaseExpiredLocksFor(string name, DateTime expireDateTime)
        {
            _collection.Remove(Query.And(Query.EQ("Name", name), Query.LTE("LockedOn", expireDateTime)));
        }

        public  Int64 ReleaseLocksFor(int agentId)
        {
            WriteConcernResult wr = _collection.Remove(Query.Matches("ProcessId", new BsonRegularExpression(new Regex(agentId + "-(\\d)+"))));
            return wr.DocumentsAffected;
        }

        public DeliveryQueueLock ReleaseLockFor(string name, string processId)
        {
            _collection.Remove(Query.And(Query.EQ("Name", name), Query.EQ("ProcessId", processId)));

            return new DeliveryQueueLock(name, processId, false);
        }

        private bool IsUnique(IEnumerable<DeliveryQueueLock> qLocks)
        {
            return (qLocks.Count() == 1);
        }
    }

    public interface IClearQueueLocker
    {
        Int64 ReleaseLocksFor(int agentId);
    }

    public interface IQueueLocker
    {
        DeliveryQueueLock AquireLockFor(string name, string processId);
        DeliveryQueueLock ReleaseLockFor(string name, string processId);
    }

    public class DeliveryQueueLock
    {
        [BsonId]
        public virtual ObjectId Id { get; set; }
        public string Name { get; set; }
        public string ProcessId { get; set; }
        public DateTime? LockedOn { get; set; }

        public DeliveryQueueLock(string name, string processId, bool locked = true)
        {
            Name = name;
            ProcessId = processId;
            LockedOn = locked ? DateTime.UtcNow : (DateTime?)null;
        }

        public bool IsLockedBy(string processId)
        {
            return (LockedOn != null && ProcessId == processId);
        }
    }
}