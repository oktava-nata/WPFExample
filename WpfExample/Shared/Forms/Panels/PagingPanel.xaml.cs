using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DBServices.PMS;
using UIMessager.Services.Message;

namespace Shared.Forms.Panels
{
    /// <summary>
    /// Логика взаимодействия для PagingPanel.xaml
    /// </summary>
    public partial class PagingPanel : UserControl
    {
        #region Properties
        public static DependencyProperty PageNavigationProperty;
        public PageNavigator PageNavigation
        {
            get { return (PageNavigator)GetValue(PageNavigationProperty); }
            set { SetValue(PageNavigationProperty, value); }
        }

        List<RecordsCountOnPage> RecordsPerPageVariants
        {
            get { return (List<RecordsCountOnPage>)cmb_RecordsPerPage.ItemsSource; }
            set { cmb_RecordsPerPage.ItemsSource = value; }
        }
        #endregion

        public PagingPanel()
        {
            InitializeComponent();

            LoadRecordsPerPageVariants();
        }

        static PagingPanel()
        {
            FrameworkPropertyMetadata metadataPageNavigation = new FrameworkPropertyMetadata(null);
            PageNavigationProperty = DependencyProperty.Register("PageNavigation", typeof(PageNavigator), typeof(PagingPanel), metadataPageNavigation);
        }

        bool LoadRecordsPerPageVariants()
        {
            try
            {
                RecordsPerPageVariants = RecordsCountOnPage.GetValues();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return false;
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            PageNavigation.GoToBackPage();
        }

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            PageNavigation.GoToNextPage();
        }

        private void btn_First_Click(object sender, RoutedEventArgs e)
        {
            PageNavigation.GoToFistPage();
        }

        private void btn_Last_Click(object sender, RoutedEventArgs e)
        {
            PageNavigation.GoToLastPage();
        }
    }
}
