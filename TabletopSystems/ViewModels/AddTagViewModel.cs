using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TabletopSystems.Models;
using TabletopTags.Database_Access;

namespace TabletopSystems.ViewModels
{
    public class AddTagViewModel : ObservableObject
    {
        private readonly MainWindowViewModel _mainWinViewModel;
        private UserConnection _userConnection;
        private TagRepository _tagRepository;
        private string _tagName;
        public string TagName
        {
            get { return _tagName; }
            set { _tagName = value; OnPropertyChanged(); }
        }
        public ICommand AddTagCommand { get; }

        public AddTagViewModel(UserConnection conn, MainWindowViewModel mainWinViewModel)
        {
            _mainWinViewModel = mainWinViewModel;
            _userConnection = conn;
            _tagRepository = new TagRepository(conn);
            _tagName = "";
            AddTagCommand = new RelayCommand(o =>
            {
                _tagRepository.Add(new TTRPGTag() { TagName = _tagName, SystemID = _mainWinViewModel.TbltopSys.SystemID });
                TagName = "";
            });
        }

       
    }
}
