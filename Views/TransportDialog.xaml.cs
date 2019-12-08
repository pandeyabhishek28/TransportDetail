using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TransportDetail.Model;
using TransportDetail.Repository;

namespace TransportDetail.Views
{
    /// <summary>
    /// Interaction logic for TransportDialog.xaml
    /// </summary>
    public partial class TransportDialog : Window
    {
        public TransportDialog()
        {
            InitializeComponent();
            _transportShipperCompany.DisplayMemberPath = "Name";
            _transportCompany.DisplayMemberPath = "Name";
            _transportCompany.SelectedValuePath = "ID";
            _transportShipperCompany.SelectedValuePath = "ID";
        }

        private List<company> companies;
        private void UpdatedUI(transport transport)
        {
            _transportID.Text = transport.ID == 0 ? "" : transport.ID.ToString();
            _transportDate.Text = transport.transport_date.ToString();
            _transportCompany.SelectedValue = transport.transport_company_id;
            _transportShipperCompany.SelectedValue = transport.shipper_company_id;
        }

        public transport ShowDialog(transport transport, bool isNew, List<company> companies,
            List<TransportViewModel> transports)
        {
            this.companies = companies;
            _transportCompany.ItemsSource = this.companies;
            _transportShipperCompany.ItemsSource= this.companies;

            UpdatedUI(transport);
            _okAction.Content = isNew ? "ADD" : "Search";
            _okAction.Click += (s, e) =>
            {
                try
                {
                    if (isNew)
                    {
                        if (int.TryParse(_transportID.Text, out int ID) &&
                        !string.IsNullOrEmpty(_transportDate.Text)
                        && _transportCompany.SelectedValue != null
                        && _transportShipperCompany.SelectedValue != null)
                        {
                            DateTime.TryParse(_transportDate.Text, out DateTime transportDate);
                            transport.ID = ID;
                            transport.transport_date = transportDate;
                            transport.transport_company_id = int.Parse(_transportCompany.SelectedValue.ToString());
                            transport.shipper_company_id = int.Parse(_transportShipperCompany.SelectedValue.ToString());
                        }

                        this.Close();
                    }
                    else
                    {
                        TransportViewModel searchResult = null;
                        if (int.TryParse(_transportID.Text, out int ID))
                        {
                            searchResult = transports.FirstOrDefault(x => x.ID == ID);
                        }
                        else if (!string.IsNullOrEmpty(_transportDate.Text) &&
                        DateTime.TryParse(_transportDate.Text, out DateTime transportDate))
                        {
                            searchResult = transports.FirstOrDefault(x => x.TransportDate == transportDate);
                        }
                        if (searchResult != null)
                            UpdatedUI(new transport()
                            {
                                ID=searchResult.ID,
                                transport_date=searchResult.TransportDate,
                                shipper_company_id=searchResult.shipper_company_id,
                                transport_company_id=searchResult.transport_company_id
                            });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please enter valid details : " + ex.Message, "Error!");
                }
            };

            _cancleAction.Click += (s, e) =>
            {
                transport = null;
                this.Close();
            };

            ShowDialog();
            return transport;
        }
    }
}
