using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using TabletopSystems.Database_Access;
using TabletopSystems.Models;

namespace TabletopSystems.ViewModels
{
    public class DisplayCapabilityViewModel : ObservableObject, IViewVM
    {
        private TTRPGCapability _capability;
        private UserConnection _userConnection;
        private ObservableCollection<TTRPGTag> _tags;
        private ObservableCollection<TTRPGAttribute> _attributes;
        public string CapabilityName
        {
            get { return _capability.CapabilityName; }
            set { _capability.CapabilityName = value; OnPropertyChanged(); }
        }
        public string CapabilityDescription
        {
            get { return _capability.Description; }
            set { _capability.Description = value; OnPropertyChanged(); }
        }
        public string CapabilityArea
        {
            get { return _capability.Area; }
            set { _capability.Area = value; OnPropertyChanged(); }
        }
        public string CapabilityRange
        {
            get { return _capability.Range; }
            set { _capability.Range = value; OnPropertyChanged(); }
        }
        public string CapabilityUseTime
        {
            get { return _capability.UseTime; }
            set { _capability.UseTime = value; OnPropertyChanged(); }
        }
        public string CapabilityCost
        {
            get { return _capability.Cost; }
            set { _capability.Cost = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TTRPGTag> Tags
        {
            get { return _tags; }
            set { _tags = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TTRPGAttribute> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; OnPropertyChanged(); }
        }

        public string CategoryName
        {
            get { return "Capability"; }
        }

        public DisplayCapabilityViewModel(UserConnection conn)
        {
            _capability = new TTRPGCapability();
            _userConnection = conn;
            _tags = new ObservableCollection<TTRPGTag>();
            _attributes = new ObservableCollection<TTRPGAttribute>();
        }
        public void GetItem(string itemName, int systemID)
        {
            CapabilityRepository cr = new CapabilityRepository(_userConnection);
            _capability = cr.SearchCapability(itemName, systemID);
            Tags = new ObservableCollection<TTRPGTag>(_capability.Tags);
            Attributes = new ObservableCollection<TTRPGAttribute>(_capability.Attributes);
        }
    }
}
