using System.Collections.Generic;
using BaseLib;
using DBServices.UsualEntity;

namespace Shared.User
{
    public partial class Access
    {
        //NATA_2018-05: было перемещено из файла IAA_Access
        public bool IsOwnUserAccess_modifiable
        {
            get { return _user.IsAdmin; }
        }


        #region PMS Access
        public List<int> AvailableIDPartitions { get { return _user.PMSAccess.IDPartitions; } }

        public bool IsAccessToAllPartitions { get { return _user.PMSAccess.AccessToAllPartitions; } }

        public bool IsPerson { get { return _user.PMSAccess.IsPerson; } } 
        
                
        /// <summary>
        /// судовой персонал
        /// </summary>
        public bool IsShipPersonal_modifiable
        {
            get { return _user.IsAdmin && AppManager.CommonInfo.Module != BaseLib.CommonProgrammInfo.TypeModule.Company; }
        }
                
        /// <summary>
        /// компанейский персонал
        /// </summary>
        public bool IsCompanyPersonal_modifiable
        {
            get { return _user.IsAdmin &&
                (AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Company
                || AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentCompany);
            }
        }

        public bool IsShipCatalogue_create
        {
            get { return _user.PMSAccess.ShipCatalogueManage; }
        }

        /// <summary>
        /// Редактор шаблонов
        /// </summary>
        public bool IsTemplateEditor_manage
        {
            get
            {
                return !_user.IsSysAdmin && _user.PMSAccess.TemplatesManage &&
                    (AppManager.CommonInfo.Module != BaseLib.CommonProgrammInfo.TypeModule.Ship);
            }
        }


        /// <summary>
        /// Менеджер по персоналу, доступны справочники
        /// </summary>
        public bool IsPersonalAndAdmin_manage
        {
            get
            {
                return !_user.IsSysAdmin && _user.IsAdmin && _user.PMSAccess.PersonalManage &&
                    (AppManager.CommonInfo.Module != BaseLib.CommonProgrammInfo.TypeModule.Ship);
            }
        }

        /// <summary>
        /// Менеджер по персоналу 
        /// </summary>
        public bool IsPersonal_manage
        {
            get
            {
                return !_user.IsSysAdmin  && _user.PMSAccess.PersonalManage &&
                    (AppManager.CommonInfo.Module != BaseLib.CommonProgrammInfo.TypeModule.Ship);
            }
        }



        /// <summary>
        /// Склад
        /// </summary>
        public bool IsStore_manage
        {
            get
            {
                return !_user.IsSysAdmin && _user.IsAdmin;
            }
        }

        /// <summary>
        /// Управление справочниками компании
        /// </summary>
        public bool IsCompanyDirectories_manage
        {
            get
            {
                return !_user.IsSysAdmin &&
                    (AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Company ||
                    AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentCompany);
            }
        }




        /// <summary>
        /// Управление справочниками компании
        /// для IndependentCompany модуль заявки скрыт
        /// </summary>
        public bool IsCompanyDirectoriesReq_manage
        {
            get
            {
                return !_user.IsSysAdmin && (AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Company);
            }
        }


        /// <summary>
        /// Управление справочниками флота
        /// </summary>
        public bool IsFleetDirectories_manage
        {
            get
            {
                return !_user.IsSysAdmin &&
                    (AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Company ||
                    AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentCompany
                    // в PMS пока не поддерживаем IndependentShip
                    //||
                    //AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentShip
                    );
            }
        }

        /// <summary>
        /// Управление справочниками флота, доступными только в компании (не для IndependentCompany)
        /// </summary>
        public bool IsFleetDirectoriesAvailableOnlyForCompany_manage
        {
            get
            {
                return !_user.IsSysAdmin && _user.IsAdmin && (AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Company);
            }
        }

        /// <summary>
        /// Управление справочниками судна
        /// </summary>
        public bool IsVesselDirectories_manage
        {
            get
            {
                return !_user.IsSysAdmin &&
                    (AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Ship ||
                    (AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentCompany && Shared.Module.CurrentShip != null));
            }
        }

        #endregion

        /// <summary>
        /// Управление структурой документации компании
        /// </summary>
        public bool IsCompanyDocumentationStructure_manage
        {
            get
            {
                return !_user.IsSysAdmin &&
                    (AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Company ||
                    AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentCompany);
            }
        }

        /// <summary>
        /// Управление документацией компании (сроками, файлами)
        /// </summary>
        public bool IsCompanyDocumentation_manage
        {
            get
            {
                return !_user.IsSysAdmin &&
                    (AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Company ||
                    AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentCompany);
            }
        }

        /// <summary>
        /// Управление структурой документации судовой
        /// </summary>
        public bool IsVesselDocumentationStructure_manage
        {
            get
            {
                return !_user.IsSysAdmin &&
                    (AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Company ||
                    AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentShip ||
                    AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Terminal);
            }
        }

        /// <summary>
        /// Управление документацией судовой (сроками, файлами)
        /// </summary>
        public bool IsVesselDocumentation_manage
        {
            get
            {
                return !_user.IsSysAdmin &&
                    (AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Ship ||
                    AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentShip ||
                    AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Terminal);
            }
        }


        /// <summary>
        /// Определение может ли текущий пользователь изменять сущность (объект, работу, СЗЧ, файл)
        /// основные её поля
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsEntityBaseProperties_modifiable(DBServices.IChangingEntityBase entity)
        {
            switch (entity.State.Value)
            {
                case EntityState.Waiting: return false;
                case EntityState.NormalOnlyEdit: return IsShipCatalogue_create;
                case EntityState.NormalOnlyUse: return IsShipCatalogue_create && IsSuperUser;
                case EntityState.Normal:return IsShipCatalogue_create;
                default: return false;
            }
        }

        /// <summary>
        /// Определение может ли текущий пользователь изменять сущность (объект, работу, СЗЧ, файл)
        /// дополнительные её поля
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsEntityAdditionalProperties_modifiable(DBServices.IChangingEntityBase entity)
        {
            switch (entity.State.Value)
            {
                case EntityState.Waiting: return false;
                case EntityState.NormalOnlyEdit: return IsShipCatalogue_create;
                case EntityState.NormalOnlyUse: return IsShipCatalogue_create;
                case EntityState.Normal: return IsShipCatalogue_create && AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentCompany;
                default: return false;
            }
        }
    }
}
