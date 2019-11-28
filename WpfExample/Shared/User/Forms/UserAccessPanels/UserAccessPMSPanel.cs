using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BaseLib;
using DBServices.PMS;
using DBServices.Services;
using DBServices.UsualEntity;
using UIMessager.Services.Message;

namespace Shared.User.Forms
{
    public partial class UserAccessPMSPanel
    {

        bool LoadPartitions()
        {
            try
            {
                Access.PartitionsForSave = new List<CheckedItem<PMS_User_Partition>>();
               // userPartitions = new List<CheckedItem<PMS_User_Partition>>();
                
                var partitions = PMS_Partition.GetAll();
                var partitionsForUser = PMS_User_Partition.GetAllForUser(Access.IdUser);

                foreach (var partition in partitions)
                {
                    //если у пользователя есть право на такой раздел
                    var userPartition = partitionsForUser.Where(item => item.IdPartition == partition.Id).FirstOrDefault();

                    if (userPartition == null)
                        userPartition = new PMS_User_Partition() { Partition = partition, IdPartition = partition.Id, IdUser = Access.IdUser };

                    var part = new CheckedItem<PMS_User_Partition>(userPartition);
                    Access.PartitionsForSave.Add(part);
                }
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return false;
        }

        List<CheckedItem<PMS_User_Partition>> GetUserPartitionForAdding()
        {
            return Access.PartitionsForSave.Where(item => item.State == CheckedState.CheckForAdding).ToList();
        }

        List<CheckedItem<PMS_User_Partition>> GetUserPartitionForDeleting()
        {
            return Access.PartitionsForSave.Where(item => item.State == CheckedState.CheckForDeleting).ToList();
        }


        bool PartitionListHasChanges
        {
            get 
            {
                return (GetUserPartitionForAdding().Count() > 0 || GetUserPartitionForDeleting().Count() > 0);
            }
        }

        bool LoadPersonsList()
        {
            List<Person> persons = GetPersons().ToList();
            if (persons == null) return false;

            PersonList = Shared.EntityItemManager.GenerateEntityItemList(persons, false);
            return PersonList != null;
        }


        ObservableCollection<Person> GetPersons()
        {
            try
            {
                if (AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Ship ||
                    AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.Terminal ||
                    AppManager.CommonInfo.Module == BaseLib.CommonProgrammInfo.TypeModule.IndependentShip)
                    return new ObservableCollection<Person>(DBServices.UsualEntity.Person.GetAllShipPersonal());
                else
                    return new ObservableCollection<Person>(DBServices.UsualEntity.Person.GetAllCompanyPersonal());
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return null;
        }

    }
}
