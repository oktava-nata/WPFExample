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
using DBServices.UsualEntity;
using DBServices;
using Shared.UIViewRules;

namespace Shared.Forms.Panels
{
    /// <summary>
    /// Логика взаимодействия для ChangeInfoPanel.xaml
    /// </summary>
    public partial class ChangeInfoPanel : UserControl
    {
        public static DependencyProperty ChangeInfoProperty;
        public IChangeInfo ChangeInfo
        {
            get { return (IChangeInfo)GetValue(ChangeInfoProperty); }
            set { SetValue(ChangeInfoProperty, value); }
        }

        public string EditorSelfText { get { return new EntityEditorIsShipOrCompanyConverter().Convert(true, null, null, null).ToString(); } }
        public string EditorOuterText { get { return new EntityEditorIsShipOrCompanyConverter().Convert(false, null, null, null).ToString(); } }

        #region Constructors
        public ChangeInfoPanel()
        {
            InitializeComponent();
        }

        static ChangeInfoPanel()
        {
            FrameworkPropertyMetadata metadataChangeInfo = new FrameworkPropertyMetadata(null);
            ChangeInfoProperty = DependencyProperty.Register("ChangeInfo", typeof(IChangeInfo),
                typeof(ChangeInfoPanel), metadataChangeInfo);
        }
        #endregion   
    }
}
