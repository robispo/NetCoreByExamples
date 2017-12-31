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
        public bool AlreadyConnet { get; set; }

        public string RealNickName
        {
            get
            {
                int index;
                index = this.NickName.IndexOf('-');

                return
                    this.NickName.Substring(0, index);
            }
        }

        public override async Task ReceiveAsync(string message)
        {
            var receiveMessage = JsonConvert.DeserializeObject<ReceiveMessage>(message);

            //var receiver = Handler.Connections.FirstOrDefault(m => ((WstmConnection)m).NickName == receiveMessage.Receiver);
            var receivers = Handler.Connections.Where(m => ((WstmConnection)m).NickName.Contains(receiveMessage.Receiver));

            if (receivers.Any())
            {
                foreach (WstConnection receiver in receivers)
                {
                    var sendMessage = JsonConvert.SerializeObject(new SendMessage
                    {
                        Sender = RealNickName,
                        Message = receiveMessage.Message
                    });

                    await receiver.SendMessageAsync(sendMessage);
                }                
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
