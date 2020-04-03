using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicrosoftCSA.HelperClasses
{ 
    public sealed class ServiceBusHelper
    {
        private static readonly Lazy<ServiceBusHelper> lazy = new Lazy<ServiceBusHelper>(() => new ServiceBusHelper());
        public static ServiceBusHelper Instance { get { return lazy.Value; } }
        private static IQueueClient queueClient = null;
        public event EventHandler<Exception> ServiceBusHelperException;
        public event EventHandler<string> ServiceBusHelperReceiveMessage;

        private void OnServiceBusHelperException(Exception e)
        {
            EventHandler<Exception> handler = ServiceBusHelperException;
            handler?.Invoke(this, e);
        }

        private void OnServiceBusHelperReceiveMessage(string e)
        {
            EventHandler<string> handler = ServiceBusHelperReceiveMessage; 
            handler?.Invoke(this, e);
        }

        private ServiceBusHelper()
        {
        }

        public void ConnectToServiceBus(string ServiceBusConnectionString, string ServiceBusQueueName)
        {
            ConnectToServiceBus(ServiceBusConnectionString, ServiceBusQueueName, false);
        }

        public void ConnectToServiceBus(string ServiceBusConnectionString, string ServiceBusQueueName, bool pickupQueueMessages = false)
        {
            queueClient = new QueueClient(ServiceBusConnectionString, ServiceBusQueueName);
            if (pickupQueueMessages == true)
            {
                RegisterOnMessageHandlerAndReceiveMessages();
            }
        }

        public async Task SendSBMessage(string sbMessage)
        {
            var message = new Message(Encoding.UTF8.GetBytes(sbMessage));
            await queueClient.SendAsync(message);
        }

        private void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = true
            };

            queueClient.RegisterMessageHandler(ProcessMessage, messageHandlerOptions);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            OnServiceBusHelperException(arg.Exception);
            return Task.CompletedTask;
        }

        private async Task ProcessMessage(Message message, CancellationToken token)
        {
            OnServiceBusHelperReceiveMessage(new String(Encoding.UTF8.GetString(message.Body)));

            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been Closed, you may chose to not call CompleteAsync() or AbandonAsync() etc. calls 
            // to avoid unnecessary exceptions.
        }
    }
}