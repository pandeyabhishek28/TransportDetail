using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportDetail.Repository;

namespace TransportDetail.Model
{
    public class TransportViewModel
    {
        public TransportViewModel(transport newTransport)
        {
            ID = newTransport.ID;
            TransportDate = newTransport.transport_date.GetValueOrDefault();
            shipper_company_id = newTransport.shipper_company_id.GetValueOrDefault();
            transport_company_id = newTransport.transport_company_id.GetValueOrDefault();
        }

        public int ID { get; set; }
        public string TransportCompanyName { get; set; }
        public string ShipperCompanyName { get; set; }
        public DateTime TransportDate { get; set; }

        public int transport_company_id { get; set; }
        public int shipper_company_id { get; set; }
    }
}
