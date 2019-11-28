using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using BaseLib;
using DBServices.UsualEntity;

namespace Shared
{

    public static class CurrentShipManager
    {
        #region CurrentShipKeeper Class
        class CurrentShipKeeper
        {
            public Ship Ship { get; private set; }
            public Action ActionOnCurrentShipChanged;
            public Action ActionOnCurrentShipOnTryChanged;

            public bool SetShip(Ship ship)
            {
                if (ship == null && Ship == null)
                     return false;

                bool isChanged = Ship != null && !Ship.Equals(ship) || Ship == null;
                
                Ship = ship;
                if (ActionOnCurrentShipOnTryChanged != null)
                    ActionOnCurrentShipOnTryChanged();

                if (isChanged && ActionOnCurrentShipChanged != null)
                {
                    ActionOnCurrentShipChanged();
                }
                return isChanged;
            }

            public bool SetShip(int shipId)
            {
                Ship ship = null;
                try
                {
                    ship = DBServices.UsualEntity.Ship.GetShipById(shipId, false);
                  
                }
                catch (Exception ex)
                {
                    BaseLib.AppManager.WriteLog(ex);
                }
                return SetShip(ship);
            }

            public void SetSingleShip(string ShipName, string ShipNumber, string ShipFlag)
            {
                Ship = new Ship(ShipName, ShipNumber, ShipFlag);                
            }

            internal void UpdateSettings()
            {
                ShipManager.LoadSettingForShip(Ship);
            }
        }
        #endregion

        static CurrentShipKeeper _CurrentShipKeeper { get; set; }

        public static Ship CurrentShip { get { return (_CurrentShipKeeper != null) ? _CurrentShipKeeper.Ship : null; } }

        public static void Initialize()
        {
            _CurrentShipKeeper = new CurrentShipKeeper();
        }

        #region SetShip
        /// <summary>
        /// Установить новое текущее судно
        /// </summary>
        /// <param name="ship"></param>
        /// <returns>true - если текщуее судно изменилось </returns>
        public static bool SetShip(Ship ship)
        {
            if (_CurrentShipKeeper == null) throw new Exception("Inner error: CurrentShipManager don't initialize!!!");
            return _CurrentShipKeeper.SetShip(ship);
        }

        /// <summary>
        /// Загрузка из БД судна по Id и установка его как текущее
        /// </summary>
        /// <param name="shipId"></param>
        /// <returns></returns>
        public static bool SetShip(int shipId)
        {
            if (_CurrentShipKeeper == null) throw new Exception("Inner error: CurrentShipManager don't initialize!!!");
            return _CurrentShipKeeper.SetShip(shipId);
        }

        public static void UnSetShip()
        {
            if (_CurrentShipKeeper == null) throw new Exception("Inner error: CurrentShipManager don't initialize!!!");
            _CurrentShipKeeper.SetShip(null);
        }

        /// <summary>
        /// Устанавливает судно для режимов Ship, Terminal, IndependentShip
        /// </summary>
        /// <param name="ShipName">Название судна, терминала</param>
        /// <param name="ShipNumber">Номер судна, терминала</param>
        /// <param name="ShipFlag">Флаг судна, терминала</param>
        public static void SetSingleShip(string ShipName, string ShipNumber, string ShipFlag)
        {
            if (_CurrentShipKeeper == null) throw new Exception("Inner error: CurrentShipManager don't initialize!!!");
            _CurrentShipKeeper.SetSingleShip(ShipName, ShipNumber, ShipFlag);
        }
        #endregion

        public static void ReloadCurrentShip()
        {
            if (_CurrentShipKeeper == null) throw new Exception("Inner error: CurrentShipManager don't initialize!!!");
            if (CurrentShip != null) SetShip(CurrentShip.Id);
        }

        #region Actions OnCurrentShipChanged
        /// <summary>
        /// Задание действия, которое будет происходить только при удачной попытки сменить текущее судно (SetShip)
        /// (попытка сменить текущее судно считается неудачной, когда предлагаемое судно совпадает с уже установленным текущим судном) 
        /// </summary>
        /// <param name="actionOnShipChanged"></param>
        public static void SetActionOnCurrentShipChanged(Action action)
        {
            if (_CurrentShipKeeper == null) throw new Exception("Inner error: CurrentShipManager don't initialize!!!");
            _CurrentShipKeeper.ActionOnCurrentShipChanged += action;
        }

        /// <summary>
        /// Задание действия, которое будет происходить всегда при попытки сменить текущее судно (SetShip)
        /// (даже при неудачной попытке  сменить текущее судно) 
        /// </summary>
        /// <param name="action"></param>
        public static void SetActionOnCurrentShipOnTryChanged(Action action)
        {
            if (_CurrentShipKeeper == null) throw new Exception("Inner error: CurrentShipManager don't initialize!!!");
            _CurrentShipKeeper.ActionOnCurrentShipOnTryChanged += action;
            var list = _CurrentShipKeeper.ActionOnCurrentShipOnTryChanged.GetInvocationList();
            //list.Where(d=>d.Method.e)
        }

        /// <summary>
        /// Отмена действия, которое происходит только при удачной попытки сменить текущее судно 
        /// </summary>
        /// <param name="actionOnShipChanged"></param>
        public static void UnSetActionOnCurrentShipChanged(Action action)
        {
            if (_CurrentShipKeeper == null) throw new Exception("Inner error: CurrentShipManager don't initialize!!!");
            _CurrentShipKeeper.ActionOnCurrentShipChanged -= action;
        }

        /// <summary>
        /// Обновление настроек из базы (используется, например, после импорта на судне)
        /// </summary>
        public static void UpdateSettings()
        {
            if (_CurrentShipKeeper == null) throw new Exception("Inner error: CurrentShipManager don't initialize!!!");
            _CurrentShipKeeper.UpdateSettings();            
        }
        #endregion
    }

    public static class ShipFactory
    {
        //Список судов (если надо происходит загрузка из БД)
        public static List<Ship> ShipList
        {
            get
            {
                if (_ShipList == null) Load();
                return _ShipList;
            }
            private set { _ShipList = value; }
        }
        static List<Ship> _ShipList;

        public static bool Load()
        {
            if (!AppManager.CommonInfo.Module_IsTypeCompany)
            {
                CurrentShipManager.UpdateSettings();
                return false;
            }

            var shipList = ShipManager.GetShipList(false, loadSettings: true);
            ShipList = (shipList != null) ? shipList.ToList() : null;
            return shipList != null;
        }

        internal static string GetShipPrefix(int shipId)
        {
            var ship = ShipList.Where(i => i.Id == shipId).FirstOrDefault();
            if (ship == null || ship.Setting == null)
                return null;
            return ship.Setting.Prefix;
        }

        internal static DBServices.UnitOfCargoValue? GetShipUnitOfCargo(int shipId) 
        {
            var ship = ShipList.Where(i => i.Id == shipId).FirstOrDefault();
            if (ship == null || ship.Setting == null)
                return null;
            return ship.Setting.UnitOfCargo;
        }
    }
}
