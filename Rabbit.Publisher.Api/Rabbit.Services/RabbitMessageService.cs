using Rabbit.Models.Entities;
using Rabbit.Repositories.Interfaces;
using Rabbit.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit.Services
{
    public class RabbitMessageService : IRabbitMessageService
    {
        private readonly IRabbitMessageRepository _repository;

        public RabbitMessageService(IRabbitMessageRepository repository) 
        { 
            _repository = repository;
        }

        public void SendMessage(RabbitMessage message)
        {
            _repository.SendMessage(message);
        }
    }
}
