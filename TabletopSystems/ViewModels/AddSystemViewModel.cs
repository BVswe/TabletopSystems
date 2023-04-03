using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Transactions;
using System.Windows;
using System.Windows.Input;
using TabletopSystems.Database_Access;
using TabletopSystems.Helper_Methods;
using TabletopSystems.Models;

namespace TabletopSystems.ViewModels
{
    public class AddSystemViewModel : ObservableObject
    {
        #region Fields/Properties
        private UserConnection _userConnection;
        private ObservableCollection<TTRPGAttribute> _attributes;
        private ObservableCollection<TTRPGAction> _actions;
        private TabletopSystem _systemToAdd;
        private MainWindowViewModel _mainWindowViewModel;
        private INavigationService _navi;
        public TabletopSystem SystemToAdd
        {
            get { return _systemToAdd; }
            set
            { 
                _systemToAdd = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<TTRPGAttribute> Attributes
        {
            get { return _attributes; }
            set
            {
                _attributes = value;
            }
        }
        public ObservableCollection<TTRPGAction> Actions
        {
            get { return _actions; }
            set
            {
                _actions = value;
            }
        }

        public ICommand AddAttributesRowCommand { get; }
        public ICommand AddActionsRowCommand { get; }
        public ICommand ConfirmedCommand { get; }

        #endregion

        #region Constructor
        public AddSystemViewModel(UserConnection conn, INavigationService navi,MainWindowViewModel mainWindowViewModel)
        {
            _navi = navi;
            _systemToAdd = new TabletopSystem();
            _userConnection = conn;
            _attributes = new ObservableCollection<TTRPGAttribute>();
            TTRPGAttribute temp = new TTRPGAttribute();
            temp.AttributeName = "HELLO THERE";
            _attributes.Add(temp);
            _actions = new ObservableCollection<TTRPGAction>();
            _mainWindowViewModel = mainWindowViewModel;
            ConfirmedCommand = new RelayCommand(o => ExecuteConfirmedCommand(), o => true);
            AddActionsRowCommand = new RelayCommand(o=> { _actions.Add(new TTRPGAction()); }, o => true);
            AddAttributesRowCommand = new RelayCommand(o => { _attributes.Add(new TTRPGAttribute()); }, o => true);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Move to System Selection page
        /// </summary>
        public void ExecuteConfirmedCommand()
        {
            try
            {
                using (TransactionScope tScope = new TransactionScope())
                {
                    InsertIntoSystems();
                    foreach (TTRPGAttribute attribute in _attributes)
                    {
                        attribute.SystemID = _systemToAdd.SystemID;
                    }
                    foreach (TTRPGAction action in _actions)
                    {
                        action.SystemID = _systemToAdd.SystemID;
                    }
                    InsertIntoAttributes();
                    InsertIntoActions();
                    tScope.Complete();
                }
            }
            catch(TransactionAbortedException ex)
            {
                MessageBox.Show("Error in creating system in database. Exception " + ex);
            }
            _mainWindowViewModel.TbltopSys = _systemToAdd;
            _navi.NavigateTo<SystemMainPageViewModel>();
        }
        /// <summary>
        /// Insert a system into the database
        /// </summary>
        private void InsertIntoSystems()
        {
            TabletopSystemRepository db = new TabletopSystemRepository(_userConnection);
            db.Add(_systemToAdd);
            _systemToAdd.SystemID = db.GetIDBySystemName(_systemToAdd.SystemName);
        }

        /// <summary>
        /// Inserts attributes into the database
        /// </summary>
        private void InsertIntoAttributes()
        {
            AttributesRepository attrDB = new AttributesRepository();
            attrDB.Add(_attributes, _userConnection);
        }

        private void InsertIntoActions()
        {
            ActionsRepository actionDB = new ActionsRepository();
            actionDB.Add(_actions, _userConnection);
        }
        #endregion

    }
}
