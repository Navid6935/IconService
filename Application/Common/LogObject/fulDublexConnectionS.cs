using Eneter.Messaging.EndPoints.TypedMessages;
using Eneter.Messaging.MessagingSystems.MessagingSystemBase;
using Eneter.Messaging.MessagingSystems.TcpMessagingSystem;
using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Application.Common.Statics;

namespace Application.Common
{
    public class fulDublexConnectionS
    {
        public static void startDublexServer()
        {
            try
            {
                TimerLogConnect = new System.Timers.Timer();
                TimerLogConnect.Elapsed += new ElapsedEventHandler(TimerLogConnectTimedEvent);
                TimerLogConnect.Interval = 1000;
                TimerLogConnect.Enabled = true;
            }
            catch (Exception ee)
            {

            }
        }
        private static IDuplexTypedMessageSender<string, string> LLmySender;
        static System.Timers.Timer TimerLogConnect;
        public static Thread threadchap;

        public static void chap(string aaa)
        {
            try
            {
                try
                {
                    threadchap = new Thread(() => chapth(aaa));
                    threadchap.Start();
                }
                catch (Exception eee)
                {
                    //File.AppendAllText(fileNameS, eee.ToString() + Environment.NewLine);
                }
                /* LogSe ls = new LogSe();
                 ls.server = "localWebService";
                 ls.textdata = aaa;
                 ls.logtype = "1";

                 var jsonSerialiser = new JavaScriptSerializer();
                 string alltext = jsonSerialiser.Serialize(ls);
                 LLmySender.SendRequestMessage(alltext);*/
                /*TimerLogsend = new System.Timers.Timer();
                TimerLogsend.Elapsed += (sender, e) => TimerLogsendTimedEvent(sender, e, aaa);
                TimerLogsend.Interval = 500;
                TimerLogsend.Enabled = true;*/

                //File.AppendAllText(fileNameS, aaa + Environment.NewLine);
            }
            catch (Exception eee)
            {
                // File.AppendAllText(fileNameS, eee.ToString() + Environment.NewLine);
            }
        }
        private static void chapth(string tdata)
        {
            try
            {
                LogSe ls = new LogSe();
                ls.server = StaticData.LogServerName;
                //ls.server = "localWebServiceWM";
                ls.textdata = tdata;
                ls.logtype = "1";
                LLmySender.SendRequestMessage(ls.Serialize());
            }
            catch (Exception ee)
            {
                TimerLogConnect.Interval = 3000;
                TimerLogConnect.Enabled = true;
            }
        }
        private static void TimerLogsendTimedEvent(object source, ElapsedEventArgs e, string tdata)
        {
            try
            {
                LogSe ls = new LogSe();
                ls.server = StaticData.LogServerName;
                ls.textdata = tdata;
                ls.logtype = "1";
                LLmySender.SendRequestMessage(ls.Serialize());
            }
            catch (Exception ee)
            {
                //File.AppendAllText(fileNameS, ee.ToString() + Environment.NewLine);
            }
        }
        private static void TimerLogConnectTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                TimerLogConnect.Enabled = false;
                var CLaSenderFactory = new DuplexTypedMessagesFactory();
                LLmySender = CLaSenderFactory.CreateDuplexTypedMessageSender<string, string>();
                IMessagingSystemFactory LLaMessaging = new TcpMessagingSystemFactory();
                //var LLanOutputChannel = LLaMessaging.CreateDuplexOutputChannel();
                var LLanOutputChannel = LLaMessaging.CreateDuplexOutputChannel(StaticData.LogServer);
                LLanOutputChannel.ConnectionClosed += LLanOutputChannel_ConnectionClosed;
                LLanOutputChannel.ConnectionOpened += LLanOutputChannel_ConnectionOpened;
                LLmySender.AttachDuplexOutputChannel(LLanOutputChannel);
            }
            catch (Exception ee)
            {
                //File.AppendAllText(fileNameS, ee.ToString() + Environment.NewLine);
            }
        }
        private static void LLanOutputChannel_ConnectionClosed(object sender, DuplexChannelEventArgs e)
        {
            TimerLogConnect.Interval = 3000;
            TimerLogConnect.Enabled = true;
        }
        private static void LLanOutputChannel_ConnectionOpened(object sender, DuplexChannelEventArgs e)
        {
            TimerLogConnect.Enabled = false;
        }
    }
}

