using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;

namespace Shared.User
{
    public class CH_Access
    {
        UI_User _user { get; set; }

        bool HasProject_CH { get { return AppManager.CommonInfo.HasProject(BaseLib.CommonProgrammInfo.ProjectName.EQUIPAGE); } }

        internal CH_Access(UI_User user)
        {
            _user = user;
        }

        /// <summary>
        /// Право управления контрактами экипажа (периодами работы), управления графиками должностей и экипажа,
        /// внесение фактических часов работы экипажа, управление дейсвием режимов за периоды для судна
        /// </summary>
        public bool Can_DoUsualWork
        {
            get { return HasProject_CH && AppManager.CommonInfo.Module_IsTypeShip; }
        }

        /// <summary>
        /// Право установки/назначения текущей конвенции для судна
        /// </summary>
        public bool Can_ManageCurrentConventionForShip
        {
            get { return HasProject_CH 
                && (AppManager.CommonInfo.Module!= CommonProgrammInfo.TypeModule.Ship)
                && _user.IsAdmin; }
        }

    }
}
