using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpServer
{
    public class ButtonClickedMessage : BaseMessage
    {
        public string Cmd { get; set; } = "buttonClicked";
        public string ButtonName { get; set; }
    }
}
