using System.Windows;
using System.Windows.Controls;
using DBServices;
using Shared.UIViewRules;

namespace Shared.Forms.ChangeInfo
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

        public static DependencyProperty CanExpandedProperty;
        public bool CanExpanded
        {
            get { return (bool)GetValue(CanExpandedProperty); }
            set { SetValue(CanExpandedProperty, value); }
        }

        public static DependencyProperty IsExpandedProperty;
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
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

            IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(ChangeInfoPanel), new FrameworkPropertyMetadata(true));
            CanExpandedProperty = DependencyProperty.Register("CanExpanded", typeof(bool), typeof(ChangeInfoPanel), new FrameworkPropertyMetadata(false));        
        }
        #endregion   
    }
}
