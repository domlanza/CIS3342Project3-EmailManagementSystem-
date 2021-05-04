using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailLibrary
{
    public class Email
    {
        private String senderName;
        private String recieverName;
        private String subject;
        private String emailBody;
        private String createdTime;

        public String SenderName
        {
            get { return senderName; }
            set { senderName = value; }
        }

        public String ReceiverName
        {
            get { return recieverName; }
            set { recieverName = value; }
        }

        public String Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public String EmailBody
        {
            get { return emailBody; }
            set { emailBody = value; }
        }

        public String CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }


    }
}
