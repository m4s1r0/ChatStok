﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatStock.Application.Interfaces.RabbitMQ
{
    public interface IRabbitMQProducer
    {
        void SendMessage<T>(T message);
    }
}
