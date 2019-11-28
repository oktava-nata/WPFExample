using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBServices;
using UIMessager.Services.Message;

namespace Shared.Managers
{
    public static class ChangeInfoManager
    {
        /// <summary>
        /// Загрузка полного ChangeInfo. 
        /// Используется для отображения полной информации об изменениях сущности на спец. панели ChangeInfoPanel
        /// </summary>
        /// <typeparam name="CEntity">Применима для классов-сущностей ChangingEntity</typeparam>
        public static bool LoadChangeInfo<CEntity>(CEntity entity, bool loadLastExImPackageInfo = true) where CEntity : IChangingEntityExpansion
        {
            try
            {
                // bool needLoadLastExImPackageInfo = (AppManager.CommonInfo.Module_IsIndependent)? false : loadLastExImPackageInfo;
                // в модулях IndependentCompany, IndependentShip, Terminal нет смысла загружать информацию об последнем экспорте-иморте
                // необходимость загрузки в зависимости от модуля проверяется внутри метода LoadFullChangeInfo
                entity.LoadFullChangeInfo(true, loadLastExImPackageInfo);
                return (entity.ChangeInfo != null);
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return false;
        }

    }
}
