using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TransportDetail.Model;
using TransportDetail.Repository;
using TransportDetail.Views;

namespace TransportDetail
{
    public class MainWindowViewModel : ViewModelBase
    {

        private company _selectedCompany;
        public company SelectedCompany
        {
            get { return _selectedCompany; }
            set
            {
                _selectedCompany = value;
                RaisePropertyChanged("SelectedCompany");
            }
        }


        private transport _selectedTransport;
        public transport SelectedTransport
        {
            get { return _selectedTransport; }
            set
            {
                _selectedTransport = value;
                RaisePropertyChanged("SelectedTransport");
            }
        }

        ObservableCollection<company> _availableCompanies;
        public ObservableCollection<company> AvailableCompanies
        {
            get { return _availableCompanies; }
            set
            {
                _availableCompanies = value;
                RaisePropertyChanged("AvailableCompanies");
            }
        }

        ObservableCollection<TransportViewModel> _allTransports;
        public ObservableCollection<TransportViewModel> AllTransports
        {
            get { return _allTransports; }
            set
            {
                _allTransports = value;
                RaisePropertyChanged("AllTransports");
            }
        }

        public RelayCommand AddNewCompany { get; set; }
        public RelayCommand AddNewTransport { get; set; }
        public RelayCommand SearchTransport { get; set; }
        public RelayCommand SearchCompany { get; set; }

        GenericRepository<company> CompanyRepo { get; set; }
        GenericRepository<transport> TransportRepo { get; set; }

        public MainWindowViewModel()
        {
            AddNewCompany = new RelayCommand(AddCompany);
            AddNewTransport = new RelayCommand(AddTransport);
            SearchTransport = new RelayCommand(SearchForTransport);
            SearchCompany = new RelayCommand(SearchForCompany);

            var datacontext = new ShipVioEntities();
            CompanyRepo = new GenericRepository<company>(datacontext);
            TransportRepo = new GenericRepository<transport>(datacontext);

            AvailableCompanies = new ObservableCollection<company>(CompanyRepo.Get(x => !string.IsNullOrEmpty(x.Name)));
            AllTransports = new ObservableCollection<TransportViewModel>();

            foreach (var item in TransportRepo.Get(x => x != null))
            {
                var shipperCompany = AvailableCompanies.FirstOrDefault(x => x.ID == item.shipper_company_id.GetValueOrDefault());
                var transportCompany = AvailableCompanies.FirstOrDefault(x => x.ID == item.transport_company_id.GetValueOrDefault());

                var newItem = new TransportViewModel(item)
                {
                    ShipperCompanyName = shipperCompany == null ? "" : shipperCompany.Name,
                    TransportCompanyName = transportCompany == null ? "" : transportCompany.Name
                };
                AllTransports.Add(newItem);
            }
        }

        private void AddCompany()
        {
            try
            {
                var dialog = new CompanyDialog();
                var newCompany = dialog.ShowDialog(new company(), true, AvailableCompanies.ToList());
                if (newCompany != null)
                {
                    if (AvailableCompanies.Any(com => com.ID == newCompany.ID ||
                     string.Equals(com.Name, newCompany.Name)))
                    {
                        MessageBox.Show("Please provide unique details", "Error!");
                    }
                    else
                    {
                        AvailableCompanies.Add(newCompany);
                        CompanyRepo.dbSet.Add(newCompany);
                        CompanyRepo.context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                // this is wrong
                // we cann't show a dialog from view model
                // as no logging is implemented so I am adding it here
                MessageBox.Show(ex.Message, "Error!");
            }
        }
        private void AddTransport()
        {
            try
            {
                var dialog = new TransportDialog();
                var newTransport = dialog.ShowDialog(new transport(), true,
                    AvailableCompanies.ToList(), AllTransports.ToList());
                if (newTransport != null)
                {
                    if (AllTransports.Any(com => com.ID == newTransport.ID))
                    {
                        MessageBox.Show("Please provide unique details", "Error!");
                    }
                    else if (!newTransport.transport_date.HasValue|| newTransport.transport_date==default(DateTime))
                    {
                        MessageBox.Show("Please provide all details for transport",
                            "Error!");
                    }
                    else if (newTransport.transport_company_id == 0 || (newTransport.shipper_company_id == 0) || newTransport.shipper_company_id == newTransport.transport_company_id)
                    {
                        MessageBox.Show("New transport entry is having same shipper company and transport company",
                            "Error!");
                    }
                    else
                    {
                        var shipperCompany = AvailableCompanies.FirstOrDefault(x => x.ID == newTransport.shipper_company_id.GetValueOrDefault());
                        var transportCompany = AvailableCompanies.FirstOrDefault(x => x.ID == newTransport.transport_company_id.GetValueOrDefault());

                        var newItem = new TransportViewModel(newTransport)
                        {
                            ShipperCompanyName = shipperCompany == null ? "" : shipperCompany.Name,
                            TransportCompanyName = transportCompany == null ? "" : transportCompany.Name
                        };
                        AllTransports.Add(newItem);

                        TransportRepo.dbSet.Add(newTransport);
                        TransportRepo.context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                // this is wrong
                // we cann't show a dialog from view model
                // as no logging is implemented so I am adding it here
                MessageBox.Show(ex.Message, "Error!");
            }
        }
        private void SearchForTransport()
        {
            try
            {
                var dialog = new TransportDialog();
                var newCompany = dialog.ShowDialog(new transport(), false,
                    AvailableCompanies.ToList(), AllTransports.ToList());
                if (newCompany != null)
                {
                    // no update 
                }
            }
            catch (Exception ex)
            {
                // this is wrong
                // we cann't show a dialog from view model
                // as no logging is implemented so I am adding it here
                MessageBox.Show(ex.Message, "Error!");
            }
        }
        private void SearchForCompany()
        {
            try
            {
                var dialog = new CompanyDialog();
                var newCompany = dialog.ShowDialog(new company(), false, AvailableCompanies.ToList());
                if (newCompany != null)
                {
                    //no update
                }
            }
            catch (Exception ex)
            {
                // this is wrong
                // we cann't show a dialog from view model
                // as no logging is implemented so I am adding it here
                MessageBox.Show(ex.Message, "Error!");
            }
        }

    }
}
