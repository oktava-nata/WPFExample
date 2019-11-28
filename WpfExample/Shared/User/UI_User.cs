using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBServices.UsualEntity;
using System.Collections.ObjectModel;

namespace Shared.User
{
    public class UI_User: DBServices.UsualEntity.User
    {
        public PMS_Access PMSAccess { get; set; }
        //NATA_2018-05: remove IAA
        //public IAA_Access IAAAccess { get; set; }

        public new bool WasPropertiesValuesChanged
        {
            get { return base.WasPropertiesValuesChanged || WasAccessChanged; }
        }

        public bool WasAccessChanged
        {
            get
            {
                return (PMSAccess != null && PMSAccess.WasPropertiesValuesChanged);
                     //NATA_2018-05: remove IAA || (IAAAccess != null && IAAAccess.WasPropertiesValuesChanged);
            }
        }

        public string AdminAccessDescription
        {
            get { return _AdminAccessDescription; }
            set { _AdminAccessDescription = value; RaisePropertyChanged(()=> this.AdminAccessDescription); }
        }
        string _AdminAccessDescription; 

        internal UI_User() : base() { }
        UI_User(int id) : base(id) { }
        UI_User(string login, string password) : base(login, password) { }
        UI_User(string login) : base(login) { }
        UI_User(object entity) : base(entity) { }

        public static new ObservableCollection<UI_User> GetAllActive()
        {
            ObservableCollection<UI_User> users = new ObservableCollection<UI_User>();
            var list = DBServices.UsualEntity.User.GetAllActive();
            foreach (var item in list)
            {
                UI_User user = new UI_User(item);
                user.LoadAccess();
                users.Add(user);
            }
            return users;
        }


        public static UI_User GetAnySystemAdministratorUserWithAccess()
        {
            var u = DBServices.UsualEntity.User.GetAnySystemAdministrator();
            if (u != null)
            {
                UI_User user = new UI_User(u);
                user.LoadAccess();
                return user;
            }
            return null;
        }

        public static UI_User GetUserWithAccess(int id)
        {
            UI_User user = new UI_User(id);
            user.LoadAccess();
            return user;
        }

        public static UI_User GetUserByLogin(string login)
        {
            UI_User user = new UI_User(login);            
            return user;
        }

        public new bool LogIn(bool onlyActive = true)
        {
            if (base.LogIn(onlyActive))
            {
                this.LoadAccess();
                return true;
            }
            else return false;
        }

        public static UI_User CreateNewUserWithAccess()
        {
            UI_User user = new UI_User();
            user.IsActive = true;
            if (BaseLib.AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.MARINOS))
            {
                user.PMSAccess = new PMS_Access();
            }
            //NATA_2018-05: remove IAA
            //if (BaseLib.AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.IAA))
            //{
            //    user.IAAAccess = new IAA_Access();
            //}
            user.CreateAccessList();
            return user;
        }

        void FormAdminAccessDescription() 
        {
            string description = Properties.Resources.txtRoleAdminInfo;
            if (BaseLib.AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.IAA))
            {
                description = string.Format("{0} {1}", description, Properties.Resources.txtRoleAdminInfo_IAA);
            }
            if (BaseLib.AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.EQUIPAGE))
            {
                description = "";
            }
            AdminAccessDescription = description;
        }

        public override void Delete()
        {
            if (this.HasReference()) this.SetInActive();
            else this.DeleteWithImitator();
        }

        void CreateAccessList()
        {
            Access = new List<DBServices.IAccess>();
            if (PMSAccess!=null) Access.Add(PMSAccess);
            //NATA_2018-05: remove IAA if (IAAAccess != null) Access.Add(IAAAccess);
            FormAdminAccessDescription();
            RaisePropertyChanged(() => this.PMSAccess);
            //NATA_2018-05: remove IAA RaisePropertyChanged(() => this.IAAAccess);
        }

        void LoadAccess()
        {
            if (BaseLib.AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.MARINOS))
            {
                PMSAccess = PMS_Access.GetForUser(this.Id);
            }
            //NATA_2018-05: remove IAA
            //if (BaseLib.AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.IAA))
            //{
            //    IAAAccess = IAA_Access.GetForUser(this.Id);
            //}
            CreateAccessList();
        }

        public new void Refresh()
        {
            base.Refresh();
            LoadAccess();
            RefreshProperties();
        }
    }
}
