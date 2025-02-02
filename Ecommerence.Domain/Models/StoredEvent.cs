﻿using Ecommerence.Domain.Events.Common;
using Ecommerence.Domain.Models.Common;
using System;

namespace Ecommerence.Domain.Models
{
    public class StoredEvent : Entity
    {
        // -> Empty contructor for EF
        protected StoredEvent()
        {

        }

        public StoredEvent(Event @event, string data, string user)
        {
            Id = Guid.NewGuid();
            AggregateId = @event.AggregateId;
            MessageType = @event.MessageType;
            Data = data;
            User = user;
        }

        public virtual Guid AggregateId { get; private set; }
        public virtual string MessageType { get; private set; }
        public virtual string Data { get; private set; }
        public virtual string User { get; private set; }
    }
}
