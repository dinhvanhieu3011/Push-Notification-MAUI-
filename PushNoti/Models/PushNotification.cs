using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNoti.Models
{
    public class PushNotification
    {
        public string Id { get; set; }   
        public string Title { get; set; }   
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
