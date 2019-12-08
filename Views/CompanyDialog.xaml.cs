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
using TransportDetail.Repository;

namespace TransportDetail.Views
{
    /// <summary>
    /// Interaction logic for CompanyDialog.xaml
    /// </summary>
    public partial class CompanyDialog : Window
    {
        public CompanyDialog()
        {
            InitializeComponent();
        }
        private List<company> companies;
        private void UpdatedUI(company company)
        {
            _companyID.Text = company.ID == 0 ? "" : company.ID.ToString();
            _companyName.Text = string.IsNullOrEmpty(company.Name) ? "" : company.Name;
        }
        public company ShowDialog(company company, bool isNew, List<company> companies)
        {
            this.companies = companies;
            UpdatedUI(company);
            _okAction.Content = isNew ? "ADD" : "Search";
            _okAction.Click += (s, e) =>
            {
                if (isNew)
                {
                    if (int.TryParse(_companyID.Text, out int ID) &&
                    !string.IsNullOrEmpty(_companyName.Text))
                    {
                        company.ID = ID;
                        company.Name = _companyName.Text;
                    }
                    this.Close();
                }
                else
                {
                    company searchResult = null;
                    if (int.TryParse(_companyID.Text, out int ID))
                    {
                        searchResult = companies.FirstOrDefault(x => x.ID == ID);
                    }
                    else if (!string.IsNullOrEmpty(_companyName.Text))
                    {
                        searchResult = companies.FirstOrDefault(x => string.Equals(x.ID, ID));
                    }
                    if (searchResult != null)
                        UpdatedUI(searchResult);
                }
            };

            _cancleAction.Click += (s, e) =>
            {
                company = null;
                this.Close();
            };

            ShowDialog();
            return company;
        }
    }
}
