using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebSocketTest.Services.WebSocketManager;

namespace WebSocketTest.Models.WstMessage
{
    public class WstmConnection : WstConnection
    {
        public WstmConnection(WstHandler handler) : base(handler) { }

        public string NickName { get; set; }

        public override async Task ReceiveAsync(string message)
        {
            var receiveMessage = JsonConvert.DeserializeObject<ReceiveMessage>(message);

            var receiver = Handler.Connections.FirstOrDefault(m => ((WstmConnection)m).NickName == receiveMessage.Receiver);

            if (receiver != null)
            {
                var sendMessage = JsonConvert.SerializeObject(new SendMessage
                {
                    Sender = NickName,
                    Message = receiveMessage.Message
                });

                await receiver.SendMessageAsync(sendMessage);
            }
            else
            {
                var sendMessage = JsonConvert.SerializeObject(new SendMessage
                {
                    Sender = NickName,
                    Message = "Can not seed to " + receiveMessage.Receiver
                });

                await SendMessageAsync(sendMessage);
            }
        }

        private class ReceiveMessage
        {
            public string Receiver { get; set; }

            public string Message { get; set; }
        }

        private class SendMessage
        {
            public string Sender { get; set; }

            public string Message { get; set; }
        }
    }
}
