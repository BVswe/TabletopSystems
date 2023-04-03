using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TabletopSystems.Database_Access;
using TabletopSystems.Models;
using TabletopTags.Database_Access;

namespace TabletopSystems.ViewModels
{
    public class AddCapabilityViewModel : ObservableObject
    {
        //Header is for dropdown to read viewmodel's name
        public string Header { get; }
        private readonly MainWindowViewModel _mainWinViewModel;
        private CapabilityRepository _capabilityRepository;
        private UserConnection _userConnection;
        private TTRPGCapability _capability;
        private List<TTRPGTag> _allTags;
        private TTRPGTag _selectedTag;
        private TTRPGTag _removalTag;
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
        public ObservableCollection<TTRPGTag> CapabilityTags
        {
            get { return _capability.Tags; }
            set { _capability.Tags = value; OnPropertyChanged(); }
        }
        public List<TTRPGTag> AllTags
        {
            get { return _allTags; }
            set { _allTags = value; OnPropertyChanged(); }
        }
        public TTRPGTag SelectedTag
        {
            get { return _selectedTag; }
            set { _selectedTag = value; OnPropertyChanged(); }
        }
        public TTRPGTag RemovalTag
        {
            get { return _removalTag; }
            set { _removalTag = value; OnPropertyChanged(); }
        }
        public ICommand AddCapabilityCommand { get; }
        public ICommand AddToCapabilityTagsCommand { get; }
        public ICommand RemoveCapabilityTagCommand { get; }
        public AddCapabilityViewModel(UserConnection conn, MainWindowViewModel mainWinViewModel)
        {
            Header = "Capability";
            _mainWinViewModel = mainWinViewModel;
            _userConnection = conn;
            _capabilityRepository = new CapabilityRepository(_userConnection);
            _capability = new TTRPGCapability();
            _capability.SystemID = _mainWinViewModel.TbltopSys.SystemID;
            TagRepository tagRepo = new TagRepository(_userConnection);
            _allTags = new List<TTRPGTag>(tagRepo.GetTags(_mainWinViewModel.TbltopSys.SystemID));
            AddCapabilityCommand = new RelayCommand(o =>
            {
                _capabilityRepository.Add(_capability);
                CapabilityName = string.Empty;
                CapabilityDescription = string.Empty;
                CapabilityArea = string.Empty;
                CapabilityRange = string.Empty;
                CapabilityUseTime = string.Empty;
                CapabilityCost = string.Empty;
                CapabilityTags.Clear();
            });
            AddToCapabilityTagsCommand = new RelayCommand(o =>
            {
                if (_selectedTag == null || _capability.Tags.Contains(_selectedTag))
                {
                    return;
                }
                _capability.Tags.Add(SelectedTag);
            });
            RemoveCapabilityTagCommand = new RelayCommand(o =>
            {
                if (_removalTag == null)
                {
                    return;
                }
                _capability.Tags.Remove(_removalTag);
            });
        }
    }
}
